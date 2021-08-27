using DragonSlay;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Bolt.Extend
{
    public partial class SubVariable : MonoBehaviour, ISubVariable
    {
        public int m_SubObjectId = 0;

        public int SubObjectId
        {
            get
            {
                return m_SubObjectId;
            }
        }
    }

#if UNITY_EDITOR
    [ExecuteInEditMode]
    public partial class SubVariable
    {
        public Variables Root
        {
            get
            {
                var parent = transform.parent;

                while(parent != null)
                {
                    var autoVariables = parent.GetComponent<Variables>();
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

        [SerializeField, HideInInspector]
        private string m_OriginName = null;

        private Variables m_Root = null;

        private void CheckInit()
        {
            if (!m_IsInit)
            {
                m_IsInit = true;
                var root = Root;
                if (m_Root != root)
                {
                    Remove();
                    if (root != null)
                    {
                        m_SubObjectId = root.m_CurrentSubObjectId++;
                        string name = gameObject.name;
                        int index = name.IndexOf('@');
                        if(index != -1)
                        {
                            name = name.Substring(0, index);
                        }
                        gameObject.name = string.Format("{0}@{1}", name, m_SubObjectId);
                    }
                }

                m_Root = root;
                Add();
                m_OriginName = gameObject.name;
            }
        }

        private void Add()
        {
            var shell = transform.GetComponent<SceneObjectDataShell>();
            var subFlow = transform.GetComponent<FlowMachine>();

            if(m_Root != null)
            {
                if(shell != null)
                {
                    m_Root.AddSceneObject(this, shell);
                }
                if(subFlow != null)
                {
                    m_Root.AddSubFlow(this, subFlow);
                }
            }
        }

        private void Remove()
        {
            var shell = transform.GetComponent<SceneObjectDataShell>();
            var subFlow = transform.GetComponent<FlowMachine>();

            if(m_Root != null)
            {
                if(shell != null)
                {
                    m_Root.RemoveSceneObject(this, shell);
                }
                if(subFlow != null)
                {
                    m_Root.RemoveSubFlow(this, subFlow);
                }
            }
        }

        private void RemoveNull()
        {
            if(m_Root != null)
            {
                m_Root.RemoveNull();
            }
        }

        private void Awake()
        {
            if(!Application.isPlaying)
            {
                if(m_OriginName == gameObject.name)
                {
                    m_Root = Root;
                }
                CheckInit();
            }
        }

        private void OnEnable()
        {
            if (!Application.isPlaying)
            {
                EditorApplication.hierarchyChanged += HierarchyChanged;
            }
        }

        private void OnDisable()
        {
            if(!Application.isPlaying)
            {
                EditorApplication.hierarchyChanged -= HierarchyChanged;
            }
        }

        private void OnDestroy()
        {
            Remove();
        }

        private void HierarchyChanged()
        {
            foreach (var obj in Selection.objects)
            {
                if(obj == gameObject)
                {
                    RemoveNull();
                    m_IsInit = false;
                    CheckInit();
                    return;
                }
            }
        }

        private void Rename()
        {
            if (m_Root == null)
            {
                return;
            }

            string name = gameObject.name;
            int index = name.IndexOf('@');
            if(index != -1)
            {
                name = name.Substring(0, index);
            }
            gameObject.name = string.Format("{0}@{1}", name, m_SubObjectId);

            var shell = transform.GetComponent<SceneObjectDataShell>();
            var subFlow = transform.GetComponent<FlowMachine>();
            if(shell != null)
            {
                m_Root.RenameSceneObject(m_OriginName, gameObject.name);
            }
            if(subFlow != null)
            {
                m_Root.RenameSubFlow(m_OriginName, gameObject.name);
            }

            m_OriginName = gameObject.name;
        }

        private void Update()
        {
            if(!Application.isPlaying)
            {
                if(m_OriginName != gameObject.name)
                {
                    Rename();
                }
            }
        }

    }
#endif
}
