using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonSlay;
using Ludiq;

namespace Bolt.Extend
{
	[DisableAnnotation]
	[IncludeInSettings(false)]
	public partial class AutoSubGraphVariables : LudiqBehaviour
    {
        [Serialize, Inspectable, VariableKind(VariableKind.AutoSubFlow)]
        public VariableDeclarations subGraphDeclarations { get; internal set; } = new VariableDeclarations();

        public static VariableDeclarations AutoSubGraph(GameObject go) => go.GetOrAddComponent<AutoSubGraphVariables>().subGraphDeclarations;


	}

#if UNITY_EDITOR
    [ExecuteInEditMode]
    public partial class AutoSubGraphVariables
    {
        private void Update()
        {
            
        }
    }
#endif
}