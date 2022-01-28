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

    public static class AutoBinaryBuilder
    {
        public static string AutoBinaryPath => Path.Combine(BoltCore.Paths.persistentGenerated, "AutoBinary.cs");

        [MenuItem("Tools/TestBinary")]
        private static void GenerateScript()
        {
            var unit = new CodeCompileUnit();

            var @namespace = new CodeNamespace("Ludiq.Generated.Binary");

            unit.Namespaces.Add(@namespace);

            var @class = new CodeTypeDeclaration("FlowGraphBinarySerialization")
            {
                IsClass = true
            };

            @class.Comments.Add(new CodeCommentStatement("自动生成FlowGraph的二进制序列化相关代码"));
            @namespace.Types.Add(@class);

            var @method = new CodeMemberMethod
            {
                Name = "AAA",
                ReturnType = new CodeTypeReference(typeof(void)),
                Attributes = MemberAttributes.Public | MemberAttributes.Static,
            };

            method.Parameters.Add(new CodeParameterDeclarationExpression(typeof(BinaryReader), "reader"));

            @method.Comments.Add(new CodeCommentStatement("2333"));

            CodeExpression invokeExpression = new CodeMethodInvokeExpression(
                new CodeTypeReferenceExpression("Binary"),
               "Read", new CodePrimitiveExpression("reader"));

            @method.Statements.Add(new CodeExpressionStatement(invokeExpression));

            @class.Members.Add(@method);

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
                    provider.GenerateCodeFromCompileUnit(unit, scriptWriter, options);
                }
            }

            AssetDatabase.Refresh();
        }

        private static CodeTypeDeclaration GenerateClass(Type type)
        {
            var @class = new CodeTypeDeclaration(type.Name)
            {
                IsClass = true,
                IsPartial = true
            };

            List<MemberInfo> m_MemberInfos = new List<MemberInfo>();
            foreach (var memberInfo in type.GetMembers())
            {
                if (memberInfo.HasAttribute<SerializeAttribute>() || memberInfo.HasAttribute<SerializeAsAttribute>())
                {
                    m_MemberInfos.Add(memberInfo);
                }
            }



            return @class;
        }

        //private static CodeMemberMethod GenerateSerializeMethod(List<MemberInfo> memberInfos)
        //{

        //}

        //private static CodeStatement GenerateSerializeStatement(MemberInfo memberInfo)
        //{
        //    Type type = null;
        //    if (memberInfo is FieldInfo fieldInfo)
        //    {
        //        type = fieldInfo.FieldType;
        //    }
        //    else if (memberInfo is PropertyInfo propertyInfo)
        //    {
        //        type = propertyInfo.PropertyType;
        //    }

        //    if (type.IsArray)
        //    {

        //    }

        //}

        private static Dictionary<Type, BinaryStatementBuilder> TypeBuilderDic = new Dictionary<Type, BinaryStatementBuilder>();

        public static IEnumerable<CodeStatement> BuildSerializeStatement(Type type,string variableName)
        {
            if (type != null)
            {
                var builder = GetBuilder(type);
                if(builder != null)
                {
                    return builder.BuildSerializeStatement(type, variableName);
                }
            }

            return null;

        }

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
        };
    }
}
