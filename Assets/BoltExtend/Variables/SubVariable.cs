using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bolt.Extend
{
    public partial class SubVariable : MonoBehaviour
    {
        public int m_SubObjectId = 0;
    }

#if UNITY_EDITOR
    [ExecuteInEditMode]
    public partial class SubVariable
    {
        public AutoVariables Root
        {
            get
            {
                var parent = transform;

                while(parent != null)
                {
                    var autoVariables = parent.GetComponent<AutoVariables>();
                    if(autoVariables != null)
                    {
                        return autoVariables;
                    }
                    parent = parent.parent;
                }

                return null;
            }
        }

        [SerializeField,HideInInspector]
        private bool m_IsInit = false;

        private void CheckInit()
        {
            if (!Application.isPlaying)
            {
                if (!m_IsInit)
                {
                    var root = Root;
                    if (root != null)
                    {
                        m_SubObjectId = root.m_CurrentSubObjectId++;
                        m_IsInit = true;
                    }
                }
            }
        }

        public void Update()
        {
            CheckInit();
        }
    }
#endif
}
