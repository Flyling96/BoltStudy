﻿using Ludiq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bolt.Extend
{
    [Editor(typeof(CustomSuperUnit))]
    public class CustomSuperUnitEditor : UnitEditor
    {
        public CustomSuperUnitEditor(Metadata metadata) : base(metadata)
        {
            variableNameInspector = new VariableNameInspector(variableNameMetadata, GetNameSubFlows);
        }

        private VariableNameInspector variableNameInspector = null;

        private Metadata variableNameMetadata => metadata[nameof(CustomSuperUnit.m_VariableName)];

        protected override float GetInspectorHeight(float width)
        {
            return GetInspectorHeight();
        }

        protected override void OnInspectorGUI(Rect position)
        {
            using (LudiqGUIUtility.currentInspectorWidth.Override(position.width))
            using (Inspector.adaptiveWidth.Override(true))
            {
                variableNameInspector.Draw(position, GUIContent.none);
            }
        }

        private IEnumerable<string> GetNameSubFlows()
        {
            return EditorVariablesUtility.GetVariableNameSuggestions(VariableKind.AutoSubFlow, reference);
        }

        private float GetInspectorHeight()
        {
            var width = variableNameInspector.GetAdaptiveWidth();

            using (LudiqGUIUtility.currentInspectorWidth.Override(width))
            {
                return variableNameInspector.GetCachedHeight(width, GUIContent.none, null);
            }
        }


    }
}
