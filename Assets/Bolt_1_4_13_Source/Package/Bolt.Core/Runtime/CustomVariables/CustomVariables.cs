using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ludiq;
using System;

namespace Bolt
{
    public class CustomVariables : LudiqMonoBehaviour
    {
        [NonSerialized]
        public VariableDeclarations m_Variables = new VariableDeclarations();

        [SerializeField]
        private List<VariableDeclaration> m_SerializeVariables = new List<VariableDeclaration>();

        public override void OnBeforeSerialize()
        {
            m_SerializeVariables.Clear();
            foreach(var variable in m_Variables)
            {
                variable.OnBeforeSerialize();
                m_SerializeVariables.Add(variable);
            }
        }

        public override void OnAfterDeserialize()
        {
            m_Variables.Clear();
            foreach (var variable in m_SerializeVariables)
            {
                variable.OnAfterDeserialize();
                m_Variables.Set(variable);
            }
            base.OnAfterDeserialize();
        }

    }
}
