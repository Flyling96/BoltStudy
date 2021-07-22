using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DragonSlay
{
    public class SceneObjectDataShell : MonoBehaviour
    {
        public SceneObjectType m_SceneObjectType;

        [SerializeReference]
        public SceneObjectData m_SceneObjectData;

        private SceneObject m_SceneObject;

        public SceneObject SceneObject
        {
            get
            {
                if (m_SceneObject == null)
                {
                    CreateSceneObject();
                }
                return m_SceneObject;
            }
        }

        public void InitSceneObjectData()
        {
            switch (m_SceneObjectType)
            {
                case SceneObjectType.Level:
                case SceneObjectType.Actor:
                    m_SceneObjectData = new SceneObjectData();
                    break;
                case SceneObjectType.RecordPoint:
                    m_SceneObjectData = new RecordPointData();
                    break;
            }
        }

        public void CreateSceneObject()
        {
            if (m_SceneObjectData == null)
            {
                return;
            }

            switch (m_SceneObjectType)
            {
                case SceneObjectType.Level: m_SceneObject = new Level(); break;
                case SceneObjectType.Actor: m_SceneObject = new Actor(); break;
                case SceneObjectType.RecordPoint: m_SceneObject = new RecordPoint(); break;
            }

            if (m_SceneObject != null)
            {
                m_SceneObject.OnCreate(m_SceneObjectData, transform);
            }
        }
    }
}
