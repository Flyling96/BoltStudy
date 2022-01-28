using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AutoBinary
{
    public class BsArrayBuilder : BinaryStatementBuilder
    {
        public override bool CanProcess(Type type)
        {
            return type.IsArray;
        }

        public override IEnumerable<CodeStatement> BuildSerializeStatement(Type type, string variableName)
        {
            string collectionName = "collection";
            CodeVariableReferenceExpression variable =new CodeVariableReferenceExpression(variableName);
            //(ICollection)variableName;
            var convert = new CodeCastExpression(typeof(ICollection), variable);
            //ICollection collection = (ICollection)variableName;
            yield return new CodeVariableDeclarationStatement(typeof(ICollection), collectionName, convert);

            var elementType = type.GetElementType();
            var elementStatements = AutoBinaryBuilder.BuildSerializeStatement(elementType, variableName);
            //for(int i =0 ;i< collection.Count;i++)
            CodeIterationStatement forLoop = new CodeIterationStatement(
                new CodeVariableDeclarationStatement(typeof(int),"i", new CodePrimitiveExpression(0)),
                new CodeBinaryOperatorExpression(new CodeVariableReferenceExpression("i"),CodeBinaryOperatorType.LessThan, 
            new CodeFieldReferenceExpression(new CodeVariableReferenceExpression("collection"),"Count")),
                new CodeAssignStatement(new CodeVariableReferenceExpression("i"), new CodeBinaryOperatorExpression(
        new CodeVariableReferenceExpression("i"), CodeBinaryOperatorType.Add, new CodePrimitiveExpression(1))),
                //LoopContent
                
                );



        }

        public override IEnumerable<CodeStatement> BuildDeserializeStatement(Type type, string variableName)
        {
            throw new NotImplementedException();
        }
    }
}
