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
	public partial class AutoVariables : LudiqBehaviour
    {
        [HideInInspector]
        public int m_CurrentSubObjectId = 10000;

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

        public VariableDeclarations SceneObjectDeclarations
        {
            get
            {
                return Variables?.subSceneObjectDeclarations;
            }
        }
    }

#if UNITY_EDITOR
    [ExecuteInEditMode]
    public partial class AutoVariables
    {
        private void Update()
        {
            if(!Application.isPlaying)
            {
                SubFlowDeclarations.Clear();
                var children = transform.GetComponentsInChildren<FlowMachine>();
                for(int i =0; i < children.Length;i++)
                {
                    var child = children[i];
                    if (child.transform == transform)
                    {
                        continue;
                    }
                    var subVariable = child.transform.GetOrAddComponent<SubVariable>();
                    var name = string.Format("{0}_Graph_{1}",child.transform.name, subVariable.m_SubObjectId);
                    SubFlowDeclarations[name] = child;
                }

                SceneObjectDeclarations.Clear();
                var shells = transform.GetComponentsInChildren<SceneObjectDataShell>();
                for (int i = 0; i < shells.Length; i++)
                {
                    var shell = shells[i];
                    var subVariable = shell.transform.GetOrAddComponent<SubVariable>();
                    var name = string.Format("{0}_SceneObject_{1}", shell.transform.name, subVariable.m_SubObjectId);
                    SceneObjectDeclarations[name] = shell;
                }
            }
        }
    }
#endif
}