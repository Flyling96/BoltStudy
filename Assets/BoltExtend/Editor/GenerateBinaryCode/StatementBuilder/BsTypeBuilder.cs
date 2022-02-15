using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AutoBinary
{
    public class BsTypeBuilder : BinaryStatementBuilder
    {
        public override bool CanProcess(Type type)
        {
            return type == typeof(Type);
        }

        public override List<CodeStatement> BuildSerializeStatement(Type type, CodeExpression variable,ref int tempIndex)
        {
            var statements = new List<CodeStatement>();
            string typeName = GetTemporaryName("typeName",ref tempIndex);
            var stringDefinition = new CodeVariableDeclarationStatement(typeof(string), typeName,
                new CodeMethodInvokeExpression(new CodeTypeReferenceExpression("RuntimeCodebase"), "SerializeType",variable));
            //string typeName = RuntimeCodebase.SerializeType(variable)
            statements.Add(stringDefinition);
            //writer.Write(typeName);
            statements.Add(new CodeExpressionStatement(new CodeMethodInvokeExpression(
                new CodeVariableReferenceExpression(WriterName), "Write", new CodeVariableReferenceExpression(typeName))));

            return statements;
        }

        public override List<CodeStatement> BuildDeserializeStatement(Type type, CodeExpression variable, ref int tempIndex)
        {
            var statements = new List<CodeStatement>();
            var readerExpression = new CodeVariableReferenceExpression(ReaderName);
            string typeName = GetTemporaryName("typeName", ref tempIndex); ;
            string _type = GetTemporaryName("_type", ref tempIndex); ;

            //string typeName = reader.ReadString();
            statements.Add(new CodeVariableDeclarationStatement(typeof(string), typeName, new CodeMethodInvokeExpression(readerExpression, "ReadString")));

            //if (RuntimeCodebase.TryDeserializeType(typeName, out var _type))
            //{
            //    variable = _type;aa
            //}
            //else
            //{
            //    Debug.LogError("Deserialize Fail type : {typeName}");
            //}
            statements.Add(new CodeConditionStatement(
                new CodeSnippetExpression($"RuntimeCodebase.TryDeserializeType({typeName}, out var {_type})"),
                new CodeStatement[] { new CodeAssignStatement(variable, new CodeVariableReferenceExpression(_type)) },
                new CodeStatement[] { new CodeExpressionStatement(new CodeMethodInvokeExpression(new CodeTypeReferenceExpression("Debug"),"LogError",
                new CodeSnippetExpression($"\"Deserialize Fail type : \" + {typeName}" )))}
                ));

            return statements;


        }
    }
}
