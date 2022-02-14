using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AutoBinary
{
    public class BsEnumBuilder : BinaryStatementBuilder
    {
        public override bool CanProcess(Type type)
        {
            return type.IsEnum;
        }

        public override IEnumerable<CodeStatement> BuildSerializeStatement(Type type, CodeExpression variable)
        {
            var writerExpression = new CodeVariableReferenceExpression(WriterName);
            var castExpression = new CodeCastExpression(typeof(int),variable);
            yield return new CodeExpressionStatement(new CodeMethodInvokeExpression(writerExpression, "Write", castExpression));
        }

        public override IEnumerable<CodeStatement> BuildDeserializeStatement(Type type, CodeExpression variable)
        {
            var readerExpression = new CodeVariableReferenceExpression(ReaderName);
            var castExpression = new CodeCastExpression(type, new CodeMethodInvokeExpression(readerExpression, "ReadInt32"));
            yield return new CodeAssignStatement(variable, castExpression);
        }
    }
}
