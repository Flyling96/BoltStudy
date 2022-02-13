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

        public abstract IEnumerable<CodeStatement> BuildSerializeStatement(Type type, CodeExpression variable);

        public abstract IEnumerable<CodeStatement> BuildDeserializeStatement(Type type, CodeExpression variable);


    }
}
