using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DragonSlay
{
    [Serializable]
    public class SceneObjectData
    {
        public int m_Tid;
        public Vector3 m_Position;
        public Quaternion m_Rotation;

#if UNITY_EDITOR
        public bool m_IsExtend;
#endif
    }

    [Serializable]
    public class RecordPointData : SceneObjectData
    {
        public Vector3 m_RebirthPointOffset;
    }



}
