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
    public class BsIListBuilder : BinaryStatementBuilder
    {
        public override bool CanProcess(Type type)
        {
			return typeof(IList).IsAssignableFrom(type);
		}

        string countName = "_count";

        public override IEnumerable<CodeStatement> BuildSerializeStatement(Type type, CodeExpression variable)
        {
            //variableName.Count
            var arrayCount = new CodeFieldReferenceExpression(variable, "Count");
            //int _count = variableName.Count;
            yield return new CodeVariableDeclarationStatement(typeof(int), countName, arrayCount);
            //writer.write(_count)
            var countStatements = GenerateBinaryCodeManager.BuildSerializeStatement(typeof(int), new CodeVariableReferenceExpression(countName));
            foreach (var countStatement in countStatements)
            {
                yield return countStatement;
            }

            var elementType = type.GetGenericArguments()[0];
            var elementStatements = GenerateBinaryCodeManager.BuildSerializeStatement(elementType, new CodeIndexerExpression(variable,new CodeVariableReferenceExpression("i")));
            if (elementStatements != null)
            {
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

            var elementType = type.GetGenericArguments()[0];
            //new List();
            CodeObjectCreateExpression objectCreate = new CodeObjectCreateExpression(type);
            //variableName = new List();
            yield return new CodeAssignStatement(variable, objectCreate);

            IEnumerable<CodeStatement> elementStatements = null;
            elementStatements = elementStatements.Add(new CodeVariableDeclarationStatement(elementType, "_temp"));
            elementStatements = elementStatements.Add(GenerateBinaryCodeManager.BuildDeserializeStatement(elementType, new CodeVariableReferenceExpression("_temp")));
            elementStatements = elementStatements.Add(new CodeExpressionStatement(new CodeMethodInvokeExpression(variable, "Add", new CodeVariableReferenceExpression("_temp"))));

            if (elementStatements != null)
            {
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
}
