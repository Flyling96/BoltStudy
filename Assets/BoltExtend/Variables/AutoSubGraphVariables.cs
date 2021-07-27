using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonSlay;
using Ludiq;

namespace Bolt.Extend
{
    [RequireComponent(typeof(Variables))]
    [DisableAnnotation]
	[IncludeInSettings(false)]
	public partial class AutoSubGraphVariables : LudiqBehaviour
    {
        private Variables m_Variables = null;

        public Variables Variables
        {
            get
            {
                if(m_Variables == null)
                {
                    m_Variables = transform.GetComponent<Variables>();
                }

                return m_Variables;
            }
        }

        public VariableDeclarations SubFlowDeclarations
        {
            get
            {
                return Variables?.subFlowDeclarations;
            }
        }

    }

#if UNITY_EDITOR
    [ExecuteInEditMode]
    public partial class AutoSubGraphVariables
    {
        private void Update()
        {
            if(!Application.isPlaying)
            {
                var children = transform.GetComponentsInChildren<FlowMachine>();
                for(int i =0; i < children.Length;i++)
                {
                    var child = children[i];
                    var name = string.Format("{0}_{1}",child.transform.name,child.GetHashCode());
                    SubFlowDeclarations[name] = child;
                }
            }
        }
    }
#endif
}