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

        string countName = "_count";

        public override IEnumerable<CodeStatement> BuildSerializeStatement(Type type, CodeExpression variable)
        {
            //variableName.Length
            var arrayCount = new CodeFieldReferenceExpression(variable, "Length");
            //int _count = variableName.Length;
            yield return new CodeVariableDeclarationStatement(typeof(int), countName, arrayCount);
            //writer.write(_count)
            var countStatements = GenerateBinaryCodeManager.BuildSerializeStatement(typeof(int), new CodeVariableReferenceExpression(countName));
            foreach(var countStatement in countStatements)
            {
                yield return countStatement;
            }

            var elementType = type.GetElementType();
            var elementStatements = GenerateBinaryCodeManager.BuildSerializeStatement(elementType, new CodeIndexerExpression(variable, new CodeVariableReferenceExpression("i")));
            //for(int i =0 ;i< _count;i++)
            CodeIterationStatement forLoop = new CodeIterationStatement(
                new CodeVariableDeclarationStatement(typeof(int),"i", new CodePrimitiveExpression(0)),
                new CodeBinaryOperatorExpression(new CodeVariableReferenceExpression("i"),CodeBinaryOperatorType.LessThan,
                new CodeVariableReferenceExpression(countName)),
                new CodeAssignStatement(new CodeVariableReferenceExpression("i"), new CodeBinaryOperatorExpression(
                new CodeVariableReferenceExpression("i"), CodeBinaryOperatorType.Add, new CodePrimitiveExpression(1))),
                //LoopContent
                elementStatements?.ToArray()
                );
            yield return forLoop;


        }

        public override IEnumerable<CodeStatement> BuildDeserializeStatement(Type type, CodeExpression variable)
        {
            //int _count = 0;
            yield return new CodeVariableDeclarationStatement(typeof(int), countName, new CodePrimitiveExpression(0));
            //_count = reader.ReadInt32();
            var countStatements = GenerateBinaryCodeManager.BuildDeserializeStatement(typeof(int), new CodeVariableReferenceExpression(countName));
            foreach (var countStatement in countStatements)
            {
                yield return countStatement;
            }

            var elementType = type.GetElementType();
            //variableName = new type[_count];
            CodeArrayCreateExpression arrayCreate = new CodeArrayCreateExpression(elementType, new CodeVariableReferenceExpression(countName));

            //variableName = new type[_count];
            yield return new CodeAssignStatement(variable, arrayCreate);

            var elementStatements = GenerateBinaryCodeManager.BuildDeserializeStatement(elementType, new CodeIndexerExpression(variable, new CodeVariableReferenceExpression("i")));
            //for(int i =0 ;i< _count;i++)
            CodeIterationStatement forLoop = new CodeIterationStatement(
                new CodeVariableDeclarationStatement(typeof(int), "i", new CodePrimitiveExpression(0)),
                new CodeBinaryOperatorExpression(new CodeVariableReferenceExpression("i"), CodeBinaryOperatorType.LessThan,
                new CodeVariableReferenceExpression(countName)),
                new CodeAssignStatement(new CodeVariableReferenceExpression("i"), new CodeBinaryOperatorExpression(
                new CodeVariableReferenceExpression("i"), CodeBinaryOperatorType.Add, new CodePrimitiveExpression(1))),
                //LoopContent
                elementStatements?.ToArray()
                );
            yield return forLoop;
        }
    }
}
