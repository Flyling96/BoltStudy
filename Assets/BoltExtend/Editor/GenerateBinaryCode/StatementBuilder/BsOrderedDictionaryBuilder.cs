using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
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
            //var dicKeys = variable.keys;
            statementList.Add(new CodeVariableDeclarationStatement(typeof(IEnumerator), "dicKeys", new CodeMethodInvokeExpression(new CodePropertyReferenceExpression(variable, "Keys"), "GetEnumerator")));
            //var dicValues = variable.values;
            statementList.Add(new CodeVariableDeclarationStatement(typeof(IEnumerator), "dicValues", new CodeMethodInvokeExpression(new CodePropertyReferenceExpression(variable, "Values"), "GetEnumerator")));
            var dicKeysReference = new CodeVariableReferenceExpression("dicKeys");
            var dicValuesReference = new CodeVariableReferenceExpression("dicValues");
            //writer.Write(variable.Count);
            statementList.Add(new CodeExpressionStatement(new CodeMethodInvokeExpression(new CodeArgumentReferenceExpression(WriterName), "Write",
                new CodePropertyReferenceExpression(variable,"Count"))));
            
            //for(;keys.MoveNext();)
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

            //for(;keys.MoveNext();)
            //{
            //    BinaryManager.Instance.SerializeObject(dicKeys.Current);
            //}
            whileStatement = new CodeIterationStatement();
            whileStatement.TestExpression = new CodeMethodInvokeExpression(dicKeysReference, "MoveNext");
            whileStatement.Statements.Add(new CodeExpressionStatement(new CodeMethodInvokeExpression(
                new CodePropertyReferenceExpression(new CodeTypeReferenceExpression("BinaryManager"), "Instance"), "SerializeObject",
                new CodeVariableReferenceExpression(WriterName), new CodePropertyReferenceExpression(dicKeysReference, "Current"))));
            whileStatement.IncrementStatement = new CodeExpressionStatement(new CodeSnippetExpression(""));
            whileStatement.InitStatement = new CodeExpressionStatement(new CodeSnippetExpression(""));
            statementList.Add(whileStatement);

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

            //var keys = new List<object>();
            statementList.Add(new CodeVariableDeclarationStatement(typeof(List<object>), "keys", new CodeObjectCreateExpression(typeof(List<object>))));
            //var values = new List<object>();
            statementList.Add(new CodeVariableDeclarationStatement(typeof(List<object>), "values", new CodeObjectCreateExpression(typeof(List<object>))));
            var keysReference = new CodeVariableReferenceExpression("keys");
            var valuesReference = new CodeVariableReferenceExpression("values");

            var forLoopStatements = GenerateBinaryCodeManager.BuildDeserializeStatement(typeof(List<object>),
                keysReference, ref tempIndex);
            foreach(var forLoop in forLoopStatements)
            {
                statementList.Add(forLoop);
            }

            forLoopStatements = GenerateBinaryCodeManager.BuildDeserializeStatement(typeof(List<object>),
                valuesReference, ref tempIndex);
            foreach (var forLoop in forLoopStatements)
            {
                statementList.Add(forLoop);
            }

            return statementList;
        }
    }
}
