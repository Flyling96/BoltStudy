using Ludiq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bolt.Extend
{
    public class CustomFlowMachine : FlowMachine
    {
        public TextAsset m_MacroBytes = null;

        [ExecuteInEditMode]
        protected override void Awake()
        {
            LoadGraph();
            if (Application.isPlaying)
            {
                base.Awake();
            }
        }

        public void LoadGraph()
        {
            if (m_Macro == null)
            {
                m_Macro = ScriptableObject.CreateInstance<FlowMacro>();
            }

            if (m_MacroBytes != null)
            {
                m_Macro.graph = LevelGraphBinaryManager.Instance.DeserializeGraph(m_MacroBytes);
            }

            nest.source = GraphSource.Macro;
            nest.macro = m_Macro;
        }
    }
}
