using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DragonSlay.Editor
{
    [CustomEditor(typeof(SceneObjectDataShell))]
    public class SceneObjectDataShellEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            SerializedProperty type = serializedObject.FindProperty("m_SceneObjectType");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(type);
            if(EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                var debugger = target as SceneObjectDataShell;
                debugger.InitSceneObjectData();
            }

            EditorGUI.BeginChangeCheck();
            SerializedProperty sceneObject = serializedObject.FindProperty("m_SceneObjectData");
            if (sceneObject != null)
            {
                SerializedProperty isExtend = sceneObject.FindPropertyRelative("m_IsExtend");
                if (isExtend != null)
                {
                    isExtend.boolValue = EditorGUILayout.Foldout(isExtend.boolValue, "Data");
                    if (isExtend.boolValue)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.Space();
                        EditorGUILayout.BeginVertical();
                        EditorGUILayout.PropertyField(sceneObject);
                        EditorGUILayout.EndVertical();
                        EditorGUILayout.EndHorizontal();
                    }
                }
            }

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
    }

    [CustomPropertyDrawer(typeof(SceneObjectData))]
    public class SceneObjectDataDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty tid = property.FindPropertyRelative("m_Tid");
            SerializedProperty pos = property.FindPropertyRelative("m_Position");
            SerializedProperty rot = property.FindPropertyRelative("m_Rotation");

            var shell = property.serializedObject.targetObject as MonoBehaviour;
            pos.vector3Value = shell.transform.position;
            rot.quaternionValue = shell.transform.rotation;

            EditorGUILayout.PropertyField(tid);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 0;
        }
    }

    [CustomPropertyDrawer(typeof(RecordPointData))]
    public class RecordPointDataPropertyDrawer : SceneObjectDataDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            base.OnGUI(position, property, label);
            SerializedProperty rebirthPointOffset = property.FindPropertyRelative("m_RebirthPointOffset");
            EditorGUILayout.PropertyField(rebirthPointOffset);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 0;
        }
    }
}
