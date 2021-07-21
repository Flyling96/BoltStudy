using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DragonSlay
{
    public class SceneObjectDebugger : MonoBehaviour
    {
        public SceneObjectType m_SceneObjectType;

        [SerializeReference]
        public SceneObject m_SceneObject;
    
        public void InitSceneObject()
        {
            switch(m_SceneObjectType)
            {
                case SceneObjectType.Level:
                    m_SceneObject = new Level(gameObject);
                    break;
                case SceneObjectType.Actor:
                    m_SceneObject = new Actor(gameObject);
                    break;
                case SceneObjectType.RecordPoint:
                    m_SceneObject = new RecordPoint(gameObject);
                    break;
            }
        }
    }
}
