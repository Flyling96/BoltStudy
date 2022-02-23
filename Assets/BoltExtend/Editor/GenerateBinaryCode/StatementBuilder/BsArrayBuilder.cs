using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AutoBinary
{
    public class BsArrayBuilder : BinaryStatementBuilder
    {
        public override bool CanProcess(Type type)
        {
            return type.IsArray;
        }


        public override List<CodeStatement> BuildSerializeStatement(Type type, CodeExpression variable,ref int tempIndex)
        {
            var statements = new List<CodeStatement>();
            string countName = GetTemporaryName("_count", ref tempIndex);
            //variableName.Length
            var arrayCount = new CodeFieldReferenceExpression(variable, "Length");

            //int _count = 0;
            statements.Add(new CodeVariableDeclarationStatement(typeof(int), countName, new CodePrimitiveExpression(0)));
            //if(variableName != null)
            //{
            //    _count = variableName.Length;
            //}
            var condition = new CodeConditionStatement(new CodeBinaryOperatorExpression(variable, CodeBinaryOperatorType.IdentityInequality, new CodePrimitiveExpression(null)),
                new CodeStatement[] { 
                    new CodeAssignStatement(new CodeVariableReferenceExpression(countName),arrayCount)
                });
            statements.Add(condition);
            //writer.write(_count)
            var countStatements = GenerateBinaryCodeManager.BuildSerializeStatement(typeof(int), new CodeVariableReferenceExpression(countName), ref tempIndex);
            foreach(var countStatement in countStatements)
            {
                statements.Add(countStatement);
            }

            string iterationIndexName = GetTemporaryName("i", ref tempIndex);
            var elementType = type.GetElementType();
            var elementStatements = GenerateBinaryCodeManager.BuildSerializeStatement(elementType, new CodeIndexerExpression(variable, new CodeVariableReferenceExpression(iterationIndexName)), ref tempIndex);
            //for(int i =0 ;i< _count;i++)
            CodeIterationStatement forLoop = new CodeIterationStatement(
                new CodeVariableDeclarationStatement(typeof(int), iterationIndexName, new CodePrimitiveExpression(0)),
                new CodeBinaryOperatorExpression(new CodeVariableReferenceExpression(iterationIndexName),CodeBinaryOperatorType.LessThan,
                new CodeVariableReferenceExpression(countName)),
                new CodeAssignStatement(new CodeVariableReferenceExpression(iterationIndexName), new CodeBinaryOperatorExpression(
                new CodeVariableReferenceExpression(iterationIndexName), CodeBinaryOperatorType.Add, new CodePrimitiveExpression(1))),
                //LoopContent
                elementStatements?.ToArray()
                );
            statements.Add(forLoop);

            return statements;
        }

        public override List<CodeStatement> BuildDeserializeStatement(Type type, CodeExpression variable, ref int tempIndex)
        {
            var statements = new List<CodeStatement>();
            string countName = GetTemporaryName("_count", ref tempIndex);
            //int _count = 0;
            statements.Add(new CodeVariableDeclarationStatement(typeof(int), countName, new CodePrimitiveExpression(0)));
            //_count = reader.ReadInt32();
            var countStatements = GenerateBinaryCodeManager.BuildDeserializeStatement(typeof(int), new CodeVariableReferenceExpression(countName),ref tempIndex);
            foreach (var countStatement in countStatements)
            {
                statements.Add(countStatement);
            }

            var elementType = type.GetElementType();
            //variableName = new type[_count];
            CodeArrayCreateExpression arrayCreate = new CodeArrayCreateExpression(elementType, new CodeVariableReferenceExpression(countName));

            //variableName = new type[_count];
            statements.Add(new CodeAssignStatement(variable, arrayCreate));

            string iterationIndexName = GetTemporaryName("i", ref tempIndex);
            var elementStatements = GenerateBinaryCodeManager.BuildDeserializeStatement(elementType, new CodeIndexerExpression(variable, new CodeVariableReferenceExpression(iterationIndexName)),ref tempIndex);
            //for(int i =0 ;i< _count;i++)
            CodeIterationStatement forLoop = new CodeIterationStatement(
                new CodeVariableDeclarationStatement(typeof(int), iterationIndexName, new CodePrimitiveExpression(0)),
                new CodeBinaryOperatorExpression(new CodeVariableReferenceExpression(iterationIndexName), CodeBinaryOperatorType.LessThan,
                new CodeVariableReferenceExpression(countName)),
                new CodeAssignStatement(new CodeVariableReferenceExpression(iterationIndexName), new CodeBinaryOperatorExpression(
                new CodeVariableReferenceExpression(iterationIndexName), CodeBinaryOperatorType.Add, new CodePrimitiveExpression(1))),
                //LoopContent
                elementStatements?.ToArray()
                );
            statements.Add(forLoop);

            return statements;
        }
    }
}
