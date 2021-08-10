using Ludiq;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace Bolt.Extend
{
    [Editor(typeof(SpecifyVariableUnit))]
    public class SpecifyVariableUnitEditor : UnitEditor
    {
        public SpecifyVariableUnitEditor(Metadata metadata) : base(metadata)
        {
            variableNameInspector = new VariableNameInspector(variableNameMetadata, GetVariablesName);
        }

        protected VariableNameInspector variableNameInspector = null;

        protected Metadata variableNameMetadata => metadata[nameof(SpecifyVariableUnit.VariableName)];

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

        protected IEnumerable<string> GetVariablesName()
        {
            if (unit is CustomSuperUnit)
            {
                return EditorVariablesUtility.GetVariableNameSuggestions(VariableKind.AutoSubFlow, reference);
            }
            else if(unit is SceneObjectUnit)
            {
                return EditorVariablesUtility.GetVariableNameSuggestions(VariableKind.AutoSceneObject, reference);
            }

            return null;
        }

        protected virtual float GetInspectorHeight()
        {
            var width = variableNameInspector.GetAdaptiveWidth();

            using (LudiqGUIUtility.currentInspectorWidth.Override(width))
            {
                return variableNameInspector.GetCachedHeight(width, GUIContent.none, null);
            }
        }

    }
}
