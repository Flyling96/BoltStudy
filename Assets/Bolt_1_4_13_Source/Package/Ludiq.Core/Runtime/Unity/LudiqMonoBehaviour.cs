using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Ludiq
{
    [ExecuteInEditMode]
    public class LudiqMonoBehaviour : MonoBehaviour, ISerializationCallbackReceiver
    {
        [NonSerialized]
        public bool m_HasDeserialize = false;

        public virtual void OnBeforeSerialize()
        {

        }

        public virtual void OnAfterDeserialize()
        {
            m_HasDeserialize = true;
        }

        private void Awake()
        {
            m_HasDeserialize = true;
        }
    }
}