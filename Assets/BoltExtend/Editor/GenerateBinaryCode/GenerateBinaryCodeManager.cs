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

        public static List<string> IngoreMemberNames = new List<string>()
        {
            "position",
            "guid"
        };

        [MenuItem("Tools/TestBinary")]
        private static void GenerateScript()
        {
            var compileUnit = new CodeCompileUnit();

            var @namespace = new CodeNamespace("Bolt");

            compileUnit.Namespaces.Add(@namespace);

            foreach (var unit in Codebase.ludiqRuntimeTypes.Where(t => typeof(IUnit).IsAssignableFrom(t)))
            {
                var type = GenerateClass(unit);
                if (type != null)
                {
                    @namespace.Types.Add(type);
                }
            }


            //var @class = new CodeTypeDeclaration("FlowGraphBinarySerialization")
            //{
            //    IsClass = true
            //};

            //@class.Comments.Add(new CodeCommentStatement("自动生成FlowGraph的二进制序列化相关代码"));
            //@namespace.Types.Add(@class);

            //var @method = new CodeMemberMethod
            //{
            //    Name = "AAA",
            //    ReturnType = new CodeTypeReference(typeof(void)),
            //    Attributes = MemberAttributes.Public | MemberAttributes.Static,
            //};

            //method.Parameters.Add(new CodeParameterDeclarationExpression(typeof(BinaryReader), "reader"));

            //@method.Comments.Add(new CodeCommentStatement("2333"));

            //CodeExpression invokeExpression = new CodeMethodInvokeExpression(
            //    new CodeTypeReferenceExpression("Binary"),
            //   "Read", new CodePrimitiveExpression("reader"));

            //@method.Statements.Add(new CodeExpressionStatement(invokeExpression));

            //@class.Members.Add(@method);

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

            AssetDatabase.Refresh();
        }

        private static CodeTypeDeclaration GenerateClass(Type type)
        {
            List<MemberInfo> m_MemberInfos = new List<MemberInfo>();
            type.GetFields(BindingFlags.DeclaredOnly);
            foreach (var memberInfo in type.GetMembers(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (memberInfo.HasAttribute<SerializeAttribute>() || memberInfo.HasAttribute<SerializeAsAttribute>())
                {
                    m_MemberInfos.Add(memberInfo);
                }
            }

            if (m_MemberInfos.Count > 0)
            {
                var @class = new CodeTypeDeclaration(type.Name)
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
                if (IngoreMemberNames.Contains(memberInfo.Name))
                {
                    continue;
                }

                Type type = null;
                if (memberInfo is FieldInfo fieldInfo)
                {
                    type = fieldInfo.FieldType;
                }
                else if (memberInfo is PropertyInfo propertyInfo)
                {
                    type = propertyInfo.PropertyType;
                }

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
                Type type = null;
                if (memberInfo is FieldInfo fieldInfo)
                {
                    type = fieldInfo.FieldType;
                }
                else if (memberInfo is PropertyInfo propertyInfo)
                {
                    type = propertyInfo.PropertyType;
                }

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
        };


    }
}
