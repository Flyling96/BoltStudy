using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AutoBinary
{
    public class BsBaseTypeBuilder : BinaryStatementBuilder
    {
        public override bool CanProcess(Type type)
        {
            return type.IsValueType && !type.IsEnum || type == typeof(string);
        }

        public override IEnumerable<CodeStatement> BuildDeserializeStatement(Type type, CodeExpression variable)
        {
            var deserializeExpression = DeserializeExpression(type);
            if (deserializeExpression != null)
            {
                //variableName = reader.read();
                if (variable != null)
                {
                    yield return new CodeAssignStatement(variable, deserializeExpression);
                }
                else
                {
                    yield return new CodeExpressionStatement(deserializeExpression);
                }
            }
        }

        public override IEnumerable<CodeStatement> BuildSerializeStatement(Type type, CodeExpression variable)
        {
            var writerExpression = new CodeVariableReferenceExpression(WriterName);
            //writer.write(variableName);
            yield return new CodeExpressionStatement(new CodeMethodInvokeExpression(writerExpression, "Write", variable));
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
            else if(type == typeof(Single))
            {
                return new CodeMethodInvokeExpression(readerExpression, nameof(reader.ReadSingle));
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
