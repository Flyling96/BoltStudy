using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AutoBinary
{
    public class BsObjectBuilder : BinaryStatementBuilder
    {
        public override bool CanProcess(Type type)
        {
            return type == typeof(object);
        }

        public override List<CodeStatement> BuildDeserializeStatement(Type type, CodeExpression variable, ref int tempIndex)
        {
            var statements = new List<CodeStatement>();
            statements.Add(new CodeExpressionStatement(new CodeMethodInvokeExpression(
                new CodePropertyReferenceExpression(new CodeTypeReferenceExpression("BinaryManager"),"Instance"), "DeserializeObject",
                new CodeVariableReferenceExpression(ReaderName), new CodeDirectionExpression(FieldDirection.Ref, variable))));

            return statements;
        }

        public override List<CodeStatement> BuildSerializeStatement(Type type, CodeExpression variable, ref int tempIndex)
        {
            var statements = new List<CodeStatement>();
            statements.Add(new CodeExpressionStatement(new CodeMethodInvokeExpression(
                new CodePropertyReferenceExpression(new CodeTypeReferenceExpression("BinaryManager"), "Instance"), "SerializeObject",
                new CodeVariableReferenceExpression(WriterName), variable)));

            return statements;
        }
    }
}
