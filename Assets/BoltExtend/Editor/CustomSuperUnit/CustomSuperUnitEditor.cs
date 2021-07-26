using Ludiq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bolt.Extend
{
    [Editor(typeof(CustomSuperUnit))]
    public class CustomSuperUnitEditor : UnitEditor
    {
        public CustomSuperUnitEditor(Metadata metadata) : base(metadata) { }

        private Metadata variableNameMetadata => metadata[nameof(CustomSuperUnit.m_VariableName)];

        protected override float GetInspectorHeight(float width)
        {
            return LudiqGUI.GetEditorHeight(this, variableNameMetadata, width);
        }

        protected override void OnInspectorGUI(Rect position)
        {
            LudiqGUI.Editor(variableNameMetadata, position);
        }
    }
}
