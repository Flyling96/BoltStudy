using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;

namespace AutoBinary
{
    public class BsOrderedDictionaryBuilder : BinaryStatementBuilder
    {
        public override bool CanProcess(Type type)
        {
            return typeof(OrderedDictionary).IsAssignableFrom(type);
        }

        public override List<CodeStatement> BuildSerializeStatement(Type type, CodeExpression variable, ref int tempIndex)
        {
            var statementList = new List<CodeStatement>();
            var dicKeysName = GetTemporaryName("dicKeys", ref tempIndex);
            var dicValuesName = GetTemporaryName("dicValues", ref tempIndex);
            //var dicKeys = variable.keys;
            statementList.Add(new CodeVariableDeclarationStatement("var", dicKeysName, new CodeMethodInvokeExpression(new CodePropertyReferenceExpression(variable, "Keys"), "GetEnumerator")));
            //var dicValues = variable.values;
            statementList.Add(new CodeVariableDeclarationStatement("var", dicValuesName, new CodeMethodInvokeExpression(new CodePropertyReferenceExpression(variable, "Values"), "GetEnumerator")));
            var dicKeysReference = new CodeVariableReferenceExpression(dicKeysName);
            var dicValuesReference = new CodeVariableReferenceExpression(dicValuesName);
            //writer.Write(variable.Count);
            statementList.Add(new CodeExpressionStatement(new CodeMethodInvokeExpression(new CodeArgumentReferenceExpression(WriterName), "Write",
                new CodePropertyReferenceExpression(variable,"Count"))));

            //for(;dicKeys.MoveNext();)
            //{
            //    BinaryManage.Instance.SerializeObject(dicKeys.Current);
            //}
            var whileStatement = new CodeIterationStatement();
            whileStatement.TestExpression = new CodeMethodInvokeExpression(dicKeysReference, "MoveNext");
            whileStatement.Statements.Add(new CodeExpressionStatement(new CodeMethodInvokeExpression(
                new CodePropertyReferenceExpression(new CodeTypeReferenceExpression("BinaryManager"), "Instance"), "SerializeObject",
                new CodeVariableReferenceExpression(WriterName), new CodePropertyReferenceExpression(dicKeysReference, "Current"))));
            whileStatement.IncrementStatement = new CodeExpressionStatement(new CodeSnippetExpression(""));
            whileStatement.InitStatement = new CodeExpressionStatement(new CodeSnippetExpression(""));
            statementList.Add(whileStatement);

            //writer.Write(variable.Count);
            statementList.Add(new CodeExpressionStatement(new CodeMethodInvokeExpression(new CodeArgumentReferenceExpression(WriterName), "Write",
                new CodePropertyReferenceExpression(variable, "Count"))));

            //for(;dicValues.MoveNext();)
            //{
            //    BinaryManager.Instance.SerializeObject(dicValues.Current);
            //}
            whileStatement = new CodeIterationStatement();
            whileStatement.TestExpression = new CodeMethodInvokeExpression(dicValuesReference, "MoveNext");
            whileStatement.Statements.Add(new CodeExpressionStatement(new CodeMethodInvokeExpression(
                new CodePropertyReferenceExpression(new CodeTypeReferenceExpression("BinaryManager"), "Instance"), "SerializeObject",
                new CodeVariableReferenceExpression(WriterName), new CodePropertyReferenceExpression(dicValuesReference, "Current"))));
            whileStatement.IncrementStatement = new CodeExpressionStatement(new CodeSnippetExpression(""));
            whileStatement.InitStatement = new CodeExpressionStatement(new CodeSnippetExpression(""));
            statementList.Add(whileStatement);

            return statementList;
        }

        public override List<CodeStatement> BuildDeserializeStatement(Type type, CodeExpression variable, ref int tempIndex)
        {
            var statementList = new List<CodeStatement>();
            var keysName = GetTemporaryName("keys", ref tempIndex);
            var valuesName = GetTemporaryName("values", ref tempIndex);
            //var keys = new List<object>();
            statementList.Add(new CodeVariableDeclarationStatement(typeof(List<object>), keysName, new CodeObjectCreateExpression(typeof(List<object>))));
            //var values = new List<object>();
            statementList.Add(new CodeVariableDeclarationStatement(typeof(List<object>), valuesName, new CodeObjectCreateExpression(typeof(List<object>))));
            var keysReference = new CodeVariableReferenceExpression(keysName);
            var valuesReference = new CodeVariableReferenceExpression(valuesName);
            var keyType = typeof(object);
            var valueType = typeof(object);

            //int _count = 0;
            string countName = GetTemporaryName("_count", ref tempIndex);
            statementList.Add(new CodeVariableDeclarationStatement(typeof(int), countName, new CodePrimitiveExpression(0)));
            //_count = reader.ReadInt32();
            var countStatements = GenerateBinaryCodeManager.BuildDeserializeStatement(typeof(int), new CodeVariableReferenceExpression(countName), ref tempIndex);
            foreach (var countStatement in countStatements)
            {
                statementList.Add(countStatement);
            }

            IEnumerable<CodeStatement> elementStatements = null;

            string iterationIndexName = GetTemporaryName("i", ref tempIndex);
            var iterationIndexReference = new CodeVariableReferenceExpression(iterationIndexName);
            string tempName = GetTemporaryName("_temp", ref tempIndex);
            if (!keyType.IsValueType)
            {
                elementStatements = elementStatements.Add(new CodeVariableDeclarationStatement(keyType, tempName, new CodePrimitiveExpression(null)));
            }
            else
            {
                elementStatements = elementStatements.Add(new CodeVariableDeclarationStatement(keyType, tempName));
            }
            elementStatements = elementStatements.Add(GenerateBinaryCodeManager.BuildDeserializeStatement(keyType, new CodeVariableReferenceExpression(tempName), ref tempIndex));
            elementStatements = elementStatements.Add(new CodeExpressionStatement(new CodeMethodInvokeExpression(keysReference, "Add", new CodeVariableReferenceExpression(tempName))));
            //for(int i =0 ;i< _count;i++)
            CodeIterationStatement forLoop = new CodeIterationStatement(
            new CodeVariableDeclarationStatement(typeof(int), iterationIndexName, new CodePrimitiveExpression(0)),
            new CodeBinaryOperatorExpression(iterationIndexReference, CodeBinaryOperatorType.LessThan,
            new CodeVariableReferenceExpression(countName)),
            new CodeAssignStatement(iterationIndexReference, new CodeBinaryOperatorExpression(
            iterationIndexReference, CodeBinaryOperatorType.Add, new CodePrimitiveExpression(1))),
            //LoopContent
            elementStatements?.ToArray()
            );
            statementList.Add(forLoop);

            elementStatements = null;
            iterationIndexName = GetTemporaryName("i", ref tempIndex);
            iterationIndexReference = new CodeVariableReferenceExpression(iterationIndexName);
            tempName = GetTemporaryName("_temp", ref tempIndex);
            if (!valueType.IsValueType)
            {
                elementStatements = elementStatements.Add(new CodeVariableDeclarationStatement(valueType, tempName, new CodePrimitiveExpression(null)));
            }
            else
            {
                elementStatements = elementStatements.Add(new CodeVariableDeclarationStatement(valueType, tempName));
            }
            elementStatements = elementStatements.Add(GenerateBinaryCodeManager.BuildDeserializeStatement(valueType, new CodeVariableReferenceExpression(tempName), ref tempIndex));
            elementStatements = elementStatements.Add(new CodeExpressionStatement(new CodeMethodInvokeExpression(valuesReference, "Add", new CodeVariableReferenceExpression(tempName))));
            //for(int i =0 ;i< _count;i++)
            forLoop = new CodeIterationStatement(
            new CodeVariableDeclarationStatement(typeof(int), iterationIndexName, new CodePrimitiveExpression(0)),
            new CodeBinaryOperatorExpression(iterationIndexReference, CodeBinaryOperatorType.LessThan,
            new CodeVariableReferenceExpression(countName)),
            new CodeAssignStatement(iterationIndexReference, new CodeBinaryOperatorExpression(
            iterationIndexReference, CodeBinaryOperatorType.Add, new CodePrimitiveExpression(1))),
            //LoopContent
            elementStatements?.ToArray()
            );
            statementList.Add(forLoop);

            iterationIndexName = GetTemporaryName("i", ref tempIndex);
            iterationIndexReference = new CodeVariableReferenceExpression(iterationIndexName);


            //for(int i = 0; i< keys.Count;i++)
            var forStatement = new CodeIterationStatement(
                new CodeVariableDeclarationStatement(typeof(int), iterationIndexName, new CodePrimitiveExpression(0)),
                new CodeBinaryOperatorExpression(iterationIndexReference, CodeBinaryOperatorType.LessThan,
                new CodePropertyReferenceExpression(keysReference, "Count")),
                new CodeAssignStatement(iterationIndexReference, new CodeBinaryOperatorExpression(
               iterationIndexReference, CodeBinaryOperatorType.Add, new CodePrimitiveExpression(1))),
                //variable.Add(keys[i],values[i]);
                new CodeStatement[]
                {
                    new CodeExpressionStatement(new CodeMethodInvokeExpression(variable,"Add",
                    new CodeIndexerExpression(keysReference,iterationIndexReference),
                    new CodeIndexerExpression(valuesReference,iterationIndexReference)))
                }
                );

            statementList.Add(forStatement);

            return statementList;
        }
    }
}
