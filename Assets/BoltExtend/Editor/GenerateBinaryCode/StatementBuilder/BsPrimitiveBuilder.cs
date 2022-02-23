using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AutoBinary
{
    public class BsPrimitiveBuilder : BinaryStatementBuilder
    {
        public override bool CanProcess(Type type)
        {
            return type.IsPrimitive|| type == typeof(string) || type == typeof(Vector2) || type == typeof(Vector3);
        }

        public override List<CodeStatement> BuildDeserializeStatement(Type type, CodeExpression variable,ref int tempIndex)
        {
            var statementList = new List<CodeStatement>();
            var deserializeExpression = DeserializeExpression(type);
            if (deserializeExpression != null)
            {
                //variableName = reader.read();
                if (variable != null)
                {
                    statementList.Add(new CodeAssignStatement(variable, deserializeExpression));
                }
                else
                {
                    statementList.Add(new CodeExpressionStatement(deserializeExpression));
                }
            }
            return statementList;
        }

        public override List<CodeStatement> BuildSerializeStatement(Type type, CodeExpression variable, ref int tempIndex)
        {
            var statementList = new List<CodeStatement>();
            var writerExpression = new CodeVariableReferenceExpression(WriterName);
            //if(string == null) string = string.Empty;
            if(type == typeof(string))
            {
                statementList.Add(new CodeConditionStatement(new CodeBinaryOperatorExpression(variable, CodeBinaryOperatorType.ValueEquality, new CodePrimitiveExpression(null)),
                    new CodeStatement[]
                    {
                        new CodeAssignStatement(variable,new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(typeof(string)),"Empty"))
                    }));
            }
            //writer.write(variableName);
            statementList.Add(new CodeExpressionStatement(new CodeMethodInvokeExpression(writerExpression, "Write", variable)));

            return statementList;
        }

        private CodeMethodInvokeExpression DeserializeExpression(Type type)
        {
            BinaryReader reader = null;
            var readerExpression = new CodeVariableReferenceExpression(ReaderName);
            if (type == typeof(Int16))
            {
                return new CodeMethodInvokeExpression(readerExpression, nameof(reader.ReadInt16));
            }
            else if (type == typeof(Int32))
            {
                return new CodeMethodInvokeExpression(readerExpression, nameof(reader.ReadInt32));
            }
            else if (type == typeof(Int64))
            {
                return new CodeMethodInvokeExpression(readerExpression, nameof(reader.ReadInt64));
            }
            else if(type == typeof(UInt16))
            {
                return new CodeMethodInvokeExpression(readerExpression, nameof(reader.ReadUInt16));
            }
            else if (type == typeof(UInt32))
            {
                return new CodeMethodInvokeExpression(readerExpression, nameof(reader.ReadUInt32));
            }
            else if (type == typeof(UInt64))
            {
                return new CodeMethodInvokeExpression(readerExpression, nameof(reader.ReadUInt64));
            }
            else if(type == typeof(Single))
            {
                return new CodeMethodInvokeExpression(readerExpression, nameof(reader.ReadSingle));
            }
            else if(type == typeof(Double))
            {
                return new CodeMethodInvokeExpression(readerExpression, nameof(reader.ReadDouble));
            }
            else if(type == typeof(String))
            {
                return new CodeMethodInvokeExpression(readerExpression, nameof(reader.ReadString));
            }
            else if(type == typeof(Boolean))
            {
                return new CodeMethodInvokeExpression(readerExpression, nameof(reader.ReadBoolean));
            }
            else if(type == typeof(Vector3))
            {
                return new CodeMethodInvokeExpression(readerExpression, "ReadVector3");
            }
            else if(type == typeof(Vector2))
            {
                return new CodeMethodInvokeExpression(readerExpression, "ReadVector2");
            }

            Debug.LogError($"BsBaseTypeBuilder error type :{type}");
            return null;
        }



    }
}
