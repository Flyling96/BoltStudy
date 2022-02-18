using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using Ludiq;

namespace Bolt.Extend
{
    [CustomEditor(typeof(CustomFlowMachine))]
    public class CustomFlowMachineEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var macroBytes = serializedObject.FindProperty("m_MacroBytes");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(macroBytes);
            if(EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                var machine = target as CustomFlowMachine;
                machine.LoadGraph();
            }

            EditorGUI.BeginDisabledGroup(macroBytes.objectReferenceValue == null);

            if (GUILayout.Button("Edit Graph"))
            {
                if (macroBytes.objectReferenceValue != null)
                {
                    var root = target as IGraphRoot;
                    if (root != null)
                    {
                        var reference = GraphReference.New(root, false);
                        GraphWindow.OpenActive(reference);
                    }
                }
            }

            if(GUILayout.Button("Reload Graph"))
            {
                var machine = target as CustomFlowMachine;
                machine.LoadGraph();
            }

            EditorGUI.EndDisabledGroup();
        }
    }
}
