using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace AutoBinary
{
    public abstract class BinaryStatementBuilder: BinaryBuilder
    {
        public abstract bool CanProcess(Type type);

        public abstract List<CodeStatement> BuildSerializeStatement(Type type, CodeExpression variable,ref int tempIndex);

        public abstract List<CodeStatement> BuildDeserializeStatement(Type type, CodeExpression variable, ref int tempIndex);

        protected string GetTemporaryName(string name,ref int tempIndex)
        {
            return string.Format("{0}_{1}", name, tempIndex++);
        }
    }
}
