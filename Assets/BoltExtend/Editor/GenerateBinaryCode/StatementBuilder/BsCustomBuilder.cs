using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ludiq;

namespace AutoBinary
{
    public class BsCustomBuilder : BinaryStatementBuilder
    {
        public override bool CanProcess(Type type)
        {
            return type.IsDefined(typeof(CustomBinaryAttribute), false);
        }

        public override List<CodeStatement> BuildSerializeStatement(Type type, CodeExpression variable, ref int tempIndex)
        {
            List<CodeStatement> statements = new List<CodeStatement>();
            statements.Add(new CodeExpressionStatement(new CodeMethodInvokeExpression(variable, GenerateBinaryCodeManager.SerializeMethodName,
                new CodeArgumentReferenceExpression(WriterName))));
            return statements;
        }

        public override List<CodeStatement> BuildDeserializeStatement(Type type, CodeExpression variable, ref int tempIndex)
        {
            List<CodeStatement> statements = new List<CodeStatement>();
            statements.Add(new CodeAssignStatement(variable, new CodeObjectCreateExpression(type)));
            statements.Add(new CodeExpressionStatement(new CodeMethodInvokeExpression(variable, GenerateBinaryCodeManager.DeserializeMethodName,
                new CodeArgumentReferenceExpression(ReaderName))));
            return statements;
        }
    }
}
