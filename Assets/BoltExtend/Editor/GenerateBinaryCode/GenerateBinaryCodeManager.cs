using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using Ludiq;
using UnityEditor;
using System;
using System.Reflection;
using System.Linq;

namespace AutoBinary
{

    public static class GenerateBinaryCodeManager
    {
        public static string AutoBinaryPath => Path.Combine(BoltFlow.Paths.package, "Runtime/Extend/Generated/AutoBinary.cs");
        public static string AutoBinaryExtendPath => "Assets/BoltExtend/Generated/AutoBinary.cs";
        public static string AutoBinaryUnitsPath => "Assets/BoltExtend/Generated/AutoBinaryUnits.cs";

        [MenuItem("Tools/GenerateBinaryCode")]
        private static void GenerateBinaryScript()
        {
            GenerateAssembly("Bolt.Flow.Runtime", AutoBinaryPath);
            GenerateAssembly("Assembly-CSharp", AutoBinaryExtendPath);
            GenerateBinaryUnits(AutoBinaryUnitsPath);
            AssetDatabase.Refresh();
        }


        [MenuItem("Tools/DeleteBinaryCode")]
        private static void DeleteGeneratedScript()
        {
            if(File.Exists(AutoBinaryPath))
            {
                File.Delete(AutoBinaryPath);
            }

            if(File.Exists(AutoBinaryExtendPath))
            {
                File.Delete(AutoBinaryExtendPath);
            }

            if (File.Exists(AutoBinaryUnitsPath))
            {
                File.Delete(AutoBinaryUnitsPath);
            }

            AssetDatabase.Refresh();
        }

        private static void GenerateAssembly(string assemblyName, string path)
        {
            var compileUnit = new CodeCompileUnit();

            Dictionary<string, HashSet<Type>> namespaceUnitDic = new Dictionary<string, HashSet<Type>>();

            foreach (var unit in Codebase.ludiqRuntimeTypes.Where(t => typeof(Unit).IsAssignableFrom(t)))
            {
                if (unit.Assembly.GetName().Name == assemblyName)
                {
                    if(namespaceUnitDic.TryGetValue(unit.Namespace,out var hashSet))
                    {
                        hashSet.Add(unit);
                    }
                    else
                    {
                        hashSet = new HashSet<Type>();
                        hashSet.Add(unit);
                        namespaceUnitDic.Add(unit.Namespace, hashSet);
                    }
                }
            }

            foreach(var keyValue in namespaceUnitDic)
            {
                compileUnit.Namespaces.Add(GenerateNamespace(keyValue.Key,keyValue.Value));
            }

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            using (var provider = CodeDomProvider.CreateProvider("CSharp"))
            {
                var options = new CodeGeneratorOptions
                {
                    BracingStyle = "C",
                    IndentString = "\t",
                    BlankLinesBetweenMembers = true,
                    ElseOnClosing = false,
                    VerbatimOrder = true
                };

                using (var scriptWriter = new StreamWriter(path))
                {
                    provider.GenerateCodeFromCompileUnit(compileUnit, scriptWriter, options);
                }
            }

        }

        private static CodeNamespace GenerateNamespace(string namespaceName, HashSet<Type> classes)
        {
            var @namespace = new CodeNamespace(namespaceName);

            foreach(var @class in classes)
            {
                var type = GenerateClass(@class);
                if (type != null)
                {
                    @namespace.Types.Add(type);
                }
            }

            @namespace.Imports.Add(new CodeNamespaceImport("UnityEngine"));
            @namespace.Imports.Add(new CodeNamespaceImport("Ludiq"));

            return @namespace;
        }

        private static CodeTypeDeclaration GenerateClass(Type type)
        {
            List<MemberInfo> m_MemberInfos = new List<MemberInfo>();
            var flags = type.BaseType != null && type.BaseType.IsGenericType && !type.IsGenericTypeDefinition ? 
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic :
                BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            foreach (var memberInfo in type.GetMembers(flags))
            {
                if(memberInfo.DeclaringType.IsAssignableFrom(typeof(Unit)))
                {
                    continue;
                }

                if (memberInfo.HasAttribute<SerializeAttribute>() || memberInfo.HasAttribute<SerializeAsAttribute>())
                {
                    Type memberType = GetMemberInfoType(memberInfo);
                    if (!memberType.IsGenericParameter)
                    {
                        //子类字段或属性只有泛型特化才加入列表
                        if (memberInfo.DeclaringType != type && memberInfo.DeclaringType.IsGenericType)
                        {
                            var definitionType = memberInfo.DeclaringType.GetGenericTypeDefinition();
                            Type declaringMemberType = null;
                            if (memberInfo is FieldInfo)
                            {
                                declaringMemberType = definitionType.GetField(memberInfo.Name).FieldType;
                            }
                            else if(memberInfo is PropertyInfo)
                            {
                                declaringMemberType = definitionType.GetProperty(memberInfo.Name).PropertyType;
                            }

                            if(declaringMemberType == memberType)
                            {
                                continue;
                            }
                        }
                        //var arguments = memberType.GetGenericArguments();
                        //bool isGenericParameter = false;
                        //if (arguments != null)
                        //{
                        //    foreach(var argument in arguments)
                        //    {
                        //        if(argument.IsGenericParameter)
                        //        {
                        //            isGenericParameter = true;
                        //            break;
                        //        }
                        //    }
                        //}
                        if (!memberType.ContainsGenericParameters)
                        {
                            m_MemberInfos.Add(memberInfo);
                        }
                    }
                }
            }

            if (m_MemberInfos.Count > 0)
            {
                var @class = new CodeTypeDeclaration(type.DisplayName(true))
                {
                    IsClass = true,
                    IsPartial = true
                };

                @class.Members.Add(GenerateSerializeMethod(m_MemberInfos, type == typeof(Unit)));
                @class.Members.Add(GenerateDeserializeMethod(m_MemberInfos, type == typeof(Unit)));
                return @class;
            }
            else
            {

                return null;
            }

        }

        private static Type GetMemberInfoType(MemberInfo memberInfo)
        {
            Type type = null;
            if (memberInfo is FieldInfo fieldInfo)
            {
                type = fieldInfo.FieldType;
            }
            else if (memberInfo is PropertyInfo propertyInfo)
            {
                type = propertyInfo.PropertyType;
            }

            return type;
        }

        private static string SerializeMethodName = "BinarySerialize";
        private static string DeserializeMethodName = "BinaryDeserialize";

        private static CodeMemberMethod GenerateSerializeMethod(List<MemberInfo> memberInfos,bool isBase)
        {
            CodeMemberMethod method = new CodeMemberMethod
            {
                Name = SerializeMethodName,
                ReturnType = new CodeTypeReference(typeof(void)),
                Attributes = MemberAttributes.Public,
            };

            method.Parameters.Add(new CodeParameterDeclarationExpression(typeof(BinaryWriter), BinaryBuilder.WriterName));

            if (!isBase)
            {
                method.Attributes |= MemberAttributes.Override;
                method.Statements.Add(new CodeExpressionStatement(new CodeMethodInvokeExpression(
                    new CodeBaseReferenceExpression(), SerializeMethodName, new CodeVariableReferenceExpression(BinaryBuilder.WriterName))));
            }

            int tempIndex = 0;
            foreach (var memberInfo in memberInfos)
            {
                Type type = GetMemberInfoType(memberInfo);

                if (type != null)
                {
                    var statements = BuildSerializeStatement(type, new CodeVariableReferenceExpression(memberInfo.Name),ref tempIndex);
                    if (statements != null)
                    {
                        method.Statements.AddRange(statements);
                    }
                    else
                    {
                        Debug.LogError($"BuildSerializeStatement error type :" +
                            $"{memberInfo.DeclaringType} memberInfo : {memberInfo.Name}");
                    }
                }
            }

            return method;
        }

        private static CodeMemberMethod GenerateDeserializeMethod(List<MemberInfo> memberInfos, bool isBase)
        {
            CodeMemberMethod method = new CodeMemberMethod
            {
                Name = DeserializeMethodName,
                ReturnType = new CodeTypeReference(typeof(void)),
                Attributes = MemberAttributes.Public ,
            };
            method.Parameters.Add(new CodeParameterDeclarationExpression(typeof(BinaryReader), BinaryBuilder.ReaderName));

            if (!isBase)
            {
                method.Attributes |= MemberAttributes.Override;
                method.Statements.Add(new CodeExpressionStatement(new CodeMethodInvokeExpression(
                    new CodeBaseReferenceExpression(), DeserializeMethodName, new CodeVariableReferenceExpression(BinaryBuilder.ReaderName))));
            }

            int tempIndex = 0;
            foreach (var memberInfo in memberInfos)
            {
                Type type = GetMemberInfoType(memberInfo);

                if (type != null)
                {
                    var statements = BuildDeserializeStatement(type, new CodeVariableReferenceExpression(memberInfo.Name),ref tempIndex);
                    if (statements != null)
                    {
                        method.Statements.AddRange(statements);
                    }
                    else
                    {
                        Debug.LogError($"BuildDeserializeStatement error type :" +
                            $"{memberInfo.DeclaringType} memberInfo : {memberInfo.Name}");
                    }
                }
            }

            return method;
        }

        public static List<CodeStatement> BuildSerializeStatement(Type type, CodeExpression variable,ref int tempIndex)
        {
            if (type != null)
            {
                var builder = GetBuilder(type);
                if(builder != null)
                {
                    return builder.BuildSerializeStatement(type, variable,ref tempIndex);
                }
            }

            return null;

        }

        public static List<CodeStatement> BuildDeserializeStatement(Type type, CodeExpression variable, ref int tempIndex)
        {
            if (type != null)
            {
                var builder = GetBuilder(type);
                if (builder != null)
                {
                    return builder.BuildDeserializeStatement(type, variable, ref tempIndex);
                }
            }

            return null;
        }

        private static Dictionary<Type, BinaryStatementBuilder> TypeBuilderDic = new Dictionary<Type, BinaryStatementBuilder>();

        private static BinaryStatementBuilder GetBuilder(Type type)
        {
            if (!TypeBuilderDic.TryGetValue(type, out var value))
            {
                foreach (var builder in StatementBuilders)
                {
                    if (builder.CanProcess(type))
                    {
                        TypeBuilderDic.Add(type, builder);
                        return builder;
                    }
                }
            }
            else
            {
                return value;
            }

            TypeBuilderDic.Add(type, null);
            return null;
        }

        private static List<BinaryStatementBuilder> StatementBuilders = new List<BinaryStatementBuilder>()
        {
            new BsArrayBuilder(),
            new BsPrimitiveBuilder(),
            new BsIListBuilder(),
            new BsEnumBuilder(),
            new BsObjectBuilder(),
            new BsTypeBuilder(),
        };

        private static void GenerateBinaryUnits(string path)
        {
            var compileUnit = new CodeCompileUnit();
            var @namespace = new CodeNamespace("Bolt.Extend");
            compileUnit.Namespaces.Add(@namespace);
            var @class = new CodeTypeDeclaration("AutoBinaryUnits");
            @namespace.Types.Add(@class);
            string unitName = "unitName";

            var method = new CodeMemberMethod()
            {
                Name = "GetUnit",
                ReturnType = new CodeTypeReference(typeof(Unit)),
                Attributes = MemberAttributes.Public | MemberAttributes.Static,
            };

            @class.Members.Add(method);

            method.Parameters.Add(new CodeParameterDeclarationExpression(typeof(string),unitName));
            method.Statements.Add(new CodeSnippetStatement($"\t\t\tswitch ({unitName})"));
            method.Statements.Add(new CodeSnippetStatement("\t\t\t{"));
            foreach (var unit in Codebase.ludiqRuntimeTypes.Where(t => typeof(Unit).IsAssignableFrom(t)))
            {
                if (!unit.IsGenericType && !unit.IsAbstract)
                {
                    method.Statements.Add(new CodeSnippetStatement($"\t\t\t\tcase nameof({unit.Name}) : return new {unit.Name}();"));
                }
            }
            method.Statements.Add(new CodeSnippetStatement("\t\t\t}"));
            method.Statements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(null)));

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            using (var provider = CodeDomProvider.CreateProvider("CSharp"))
            {
                var options = new CodeGeneratorOptions
                {
                    BracingStyle = "C",
                    IndentString = "\t",
                    BlankLinesBetweenMembers = true,
                    ElseOnClosing = false,
                    VerbatimOrder = true
                };

                using (var scriptWriter = new StreamWriter(path))
                {
                    provider.GenerateCodeFromCompileUnit(compileUnit, scriptWriter, options);
                }
            }
        }
    }
}
