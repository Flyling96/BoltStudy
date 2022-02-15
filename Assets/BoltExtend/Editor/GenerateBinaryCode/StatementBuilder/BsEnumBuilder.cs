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

        public override List<CodeStatement> BuildSerializeStatement(Type type, CodeExpression variable,ref int tempIndex)
        {
            var statementList = new List<CodeStatement>();
            var writerExpression = new CodeVariableReferenceExpression(WriterName);
            var castExpression = new CodeCastExpression(typeof(int),variable);
            //writer.Write((int)variable);
            statementList.Add(new CodeExpressionStatement(new CodeMethodInvokeExpression(writerExpression, "Write", castExpression)));
            return statementList;
        }

        public override List<CodeStatement> BuildDeserializeStatement(Type type, CodeExpression variable, ref int tempIndex)
        {
            var statementList = new List<CodeStatement>();
            var readerExpression = new CodeVariableReferenceExpression(ReaderName);
            var castExpression = new CodeCastExpression(type, new CodeMethodInvokeExpression(readerExpression, "ReadInt32"));
            //varialbe = (Enum)reader.ReadInt32()
            statementList.Add(new CodeAssignStatement(variable, castExpression));
            return statementList;
        }
    }
}
