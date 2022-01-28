using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace AutoBinary
{
    public abstract class BinaryStatementBuilder
    {
        public abstract bool CanProcess(Type type);

        public abstract IEnumerable<CodeStatement> BuildSerializeStatement(Type type, string variableName);

        public abstract IEnumerable<CodeStatement> BuildDeserializeStatement(Type type, string variableName);


    }
}
