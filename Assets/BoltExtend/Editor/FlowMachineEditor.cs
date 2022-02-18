using Ludiq;
using System.IO;
using UnityEditor;
using UnityEngine;
using Bolt.Extend;

namespace Bolt
{
	[CustomEditor(typeof(FlowMachine))]
	public class FlowMachineEditor : Editor
	{
        public override void OnInspectorGUI()
        {
            Rect rect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight * 3 + 10);
            rect.height = EditorGUIUtility.singleLineHeight;
            Rect buttonRect = rect;
            buttonRect.y += EditorGUIUtility.singleLineHeight + 5;
            float newButtonWidth = 60;

            rect.width -= newButtonWidth + 20;
            var macroProperty = serializedObject.FindProperty("m_Macro");
            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(rect, macroProperty);
            if(EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }

            rect.x += rect.width + 20;
            rect.width = newButtonWidth;
            if(GUI.Button(rect,"New"))
            {
                var macro = (IMacro)ScriptableObject.CreateInstance(typeof(FlowMacro));
                var macroObject = (Object)macro;
                var path = EditorUtility.SaveFilePanelInProject("Save Macro", macroObject.name, "asset", null);

                if(!string.IsNullOrEmpty(path))
                {
                    AssetDatabase.CreateAsset(macroObject, path);
                    macroProperty.objectReferenceValue = macroObject;
                    serializedObject.ApplyModifiedProperties();
                }
            }

            EditorGUI.BeginDisabledGroup(macroProperty.objectReferenceValue == null);

            if(GUI.Button(buttonRect, "Edit Graph"))
            {
                if(macroProperty.objectReferenceValue != null)
                {
                    var root = target as IGraphRoot;
                    if(root != null)
                    {
                        var reference = GraphReference.New(root, false);
                        GraphWindow.OpenActive(reference);
                    }
                }
            }

            buttonRect.y += EditorGUIUtility.singleLineHeight + 5;

            if(GUI.Button(buttonRect, "Save Binary"))
            {
                var machine = target as FlowMachine;
                var macro = machine.m_Macro;
                if (macro == null)
                {
                    Debug.LogError("[SaveBinary] m_Macro is null.");
                }

                var path = AssetDatabase.GetAssetPath(macro);
                var directory = Path.GetDirectoryName(path);
                var name = Path.GetFileNameWithoutExtension(path);
                var savePath = Path.Combine(directory, $"Binary/{name}.bytes");
                LevelGraphBinaryManager.Instance.SerializeGraph(macro.graph, savePath);
                AssetDatabase.ImportAsset(savePath);
            }

            EditorGUI.EndDisabledGroup();
        }
    }
}