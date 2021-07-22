using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonSlay;

namespace Bolt.Extend
{
    public partial class LevelMember : MonoBehaviour
    {
        public HashSet<SceneObjectDataShell> m_DataShells = new HashSet<SceneObjectDataShell>();
        public HashSet<LevelMember> m_SubLevelMembers = new HashSet<LevelMember>();
    }

#if UNITY_EDITOR
    [ExecuteInEditMode]
    public partial class LevelMember
    {
        private void Update()
        {
            
        }
    }
#endif
}