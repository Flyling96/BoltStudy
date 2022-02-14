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

        [MenuItem("Tools/TestBinary")]
        private static void GenerateScript()
        {
            var compileUnit = new CodeCompileUnit();

            var @namespace = new CodeNamespace("Bolt");

            compileUnit.Namespaces.Add(@namespace);

            foreach (var unit in Codebase.ludiqRuntimeTypes.Where(t => typeof(IUnit).IsAssignableFrom(t)))
            {
                if (unit.Assembly.GetName().Name == "Bolt.Flow.Runtime")
                {
                    var type = GenerateClass(unit);
                    if (type != null)
                    {
                        @namespace.Types.Add(type);
                    }
                }
            }

            if (File.Exists(AutoBinaryPath))
            {
                File.Delete(AutoBinaryPath);
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

                using (var scriptWriter = new StreamWriter(AutoBinaryPath))
                {
                    provider.GenerateCodeFromCompileUnit(compileUnit, scriptWriter, options);
                }
            }

            //Bolt.Extend
            compileUnit = new CodeCompileUnit();

            @namespace = new CodeNamespace("Bolt.Extend");

            compileUnit.Namespaces.Add(@namespace);

            foreach (var unit in Codebase.ludiqRuntimeTypes.Where(t => typeof(IUnit).IsAssignableFrom(t)))
            {
                if (Codebase.IsUserAssembly(unit.Assembly))
                {
                    var type = GenerateClass(unit);
                    if (type != null)
                    {
                        @namespace.Types.Add(type);
                    }
                }
            }

            if (File.Exists(AutoBinaryExtendPath))
            {
                File.Delete(AutoBinaryExtendPath);
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

                using (var scriptWriter = new StreamWriter(AutoBinaryExtendPath))
                {
                    provider.GenerateCodeFromCompileUnit(compileUnit, scriptWriter, options);
                }
            }


            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/DeleteBinary")]
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

            AssetDatabase.Refresh();
        }

        private static CodeTypeDeclaration GenerateClass(Type type)
        {
            List<MemberInfo> m_MemberInfos = new List<MemberInfo>();
            var flags = type.BaseType != null && type.BaseType.IsGenericType ? BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic :
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
                        var arguments = memberType.GetGenericArguments();
                        bool isGenericParameter = false;
                        if (arguments != null)
                        {
                            foreach(var argument in arguments)
                            {
                                if(argument.IsGenericParameter)
                                {
                                    isGenericParameter = true;
                                    break;
                                }
                            }
                        }
                        if (!isGenericParameter)
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

            foreach (var memberInfo in memberInfos)
            {
                Type type = GetMemberInfoType(memberInfo);

                if (type != null)
                {
                    var statements = BuildSerializeStatement(type, new CodeVariableReferenceExpression(memberInfo.Name));
                    if (statements != null)
                    {
                        method.Statements.AddRange(statements);
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

            foreach (var memberInfo in memberInfos)
            {
                Type type = GetMemberInfoType(memberInfo);

                if (type != null)
                {
                    var statements = BuildDeserializeStatement(type, new CodeVariableReferenceExpression(memberInfo.Name));
                    if (statements != null)
                    {
                        method.Statements.AddRange(statements);
                    }
                }
            }

            return method;
        }


        public static IEnumerable<CodeStatement> BuildSerializeStatement(Type type, CodeExpression variable)
        {
            if (type != null)
            {
                var builder = GetBuilder(type);
                if(builder != null)
                {
                    return builder.BuildSerializeStatement(type, variable);
                }
            }

            return null;

        }

        public static IEnumerable<CodeStatement> BuildDeserializeStatement(Type type, CodeExpression variable)
        {
            if (type != null)
            {
                var builder = GetBuilder(type);
                if (builder != null)
                {
                    return builder.BuildDeserializeStatement(type, variable);
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

            Debug.LogError($"GetBuilder error type :{type}");
            TypeBuilderDic.Add(type, null);
            return null;
        }

        private static List<BinaryStatementBuilder> StatementBuilders = new List<BinaryStatementBuilder>()
        {
            new BsArrayBuilder(),
            new BsBaseTypeBuilder(),
            new BsIListBuilder(),
            new BsEnumBuilder()
        };


    }
}
