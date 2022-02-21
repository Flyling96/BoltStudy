using Ludiq.FullSerializer;
using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace AutoBinary
{
    public class BsArrayListBuilder : BinaryStatementBuilder
    {
        public override bool CanProcess(Type type)
        {
            return typeof(ArrayList).IsAssignableFrom(type);
        }

        public override List<CodeStatement> BuildSerializeStatement(Type type, CodeExpression variable, ref int tempIndex)
        {
            var statementList = new List<CodeStatement>();
            //variableName.Count
            string countName = GetTemporaryName("_count", ref tempIndex);
            var arrayCount = new CodeFieldReferenceExpression(variable, "Count");
            //int _count = variableName.Count;
            //yield return new CodeVariableDeclarationStatement(typeof(int), countName, arrayCount);
            statementList.Add(new CodeVariableDeclarationStatement(typeof(int), countName, arrayCount));
            //writer.write(_count)
            var countStatements = GenerateBinaryCodeManager.BuildSerializeStatement(typeof(int), new CodeVariableReferenceExpression(countName), ref tempIndex);
            foreach (var countStatement in countStatements)
            {
                //yield return countStatement;
                statementList.Add(countStatement);
            }

            string iterationIndexName = GetTemporaryName("i", ref tempIndex);
            var elementType = typeof(object);
            var elementStatements = GenerateBinaryCodeManager.BuildSerializeStatement(elementType, new CodeIndexerExpression(variable, new CodeVariableReferenceExpression(iterationIndexName)), ref tempIndex);
            if (elementStatements != null)
            {
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
                //yield return forLoop;
                statementList.Add(forLoop);
            }

            return statementList;
        }

        public override List<CodeStatement> BuildDeserializeStatement(Type type, CodeExpression variable, ref int tempIndex)
        {
            var statementList = new List<CodeStatement>();
            //int _count = 0;
            string countName = GetTemporaryName("_count", ref tempIndex);
            statementList.Add(new CodeVariableDeclarationStatement(typeof(int), countName, new CodePrimitiveExpression(0)));
            //_count = reader.ReadInt32();
            var countStatements = GenerateBinaryCodeManager.BuildDeserializeStatement(typeof(int), new CodeVariableReferenceExpression(countName), ref tempIndex);
            foreach (var countStatement in countStatements)
            {
                statementList.Add(countStatement);
            }

            var elementType = typeof(object);
            //new List();
            CodeObjectCreateExpression objectCreate = new CodeObjectCreateExpression(type);
            //variableName = new List();
            statementList.Add(new CodeAssignStatement(variable, objectCreate));

            IEnumerable<CodeStatement> elementStatements = null;

            string iterationIndexName = GetTemporaryName("i", ref tempIndex);
            string tempName = GetTemporaryName("_temp", ref tempIndex);
            if (!elementType.IsValueType)
            {
                elementStatements = elementStatements.Add(new CodeVariableDeclarationStatement(elementType, tempName, new CodePrimitiveExpression(null)));
            }
            else
            {
                elementStatements = elementStatements.Add(new CodeVariableDeclarationStatement(elementType, tempName));
            }
            elementStatements = elementStatements.Add(GenerateBinaryCodeManager.BuildDeserializeStatement(elementType, new CodeVariableReferenceExpression(tempName), ref tempIndex));
            elementStatements = elementStatements.Add(new CodeExpressionStatement(new CodeMethodInvokeExpression(variable, "Add", new CodeVariableReferenceExpression(tempName))));
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
            statementList.Add(forLoop);

            return statementList;

        }

    }
}
