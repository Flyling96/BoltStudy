using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Ludiq;

namespace Bolt
{
    [Editor(typeof(CustomVariables))]
    public class CustomVariablesEditor : Inspector
	{
		public CustomVariablesEditor(Metadata metadata) : base(metadata) { }

		private Metadata declarationsMetadata => metadata[nameof(CustomVariables.m_Variables)];

		protected override float GetHeight(float width, GUIContent label)
		{
			return LudiqGUI.GetInspectorHeight(this, declarationsMetadata, width, GUIContent.none);
		}

		protected override void OnGUI(Rect position, GUIContent label)
		{
			LudiqGUI.Inspector(declarationsMetadata, position, GUIContent.none);
		}
	}
}
