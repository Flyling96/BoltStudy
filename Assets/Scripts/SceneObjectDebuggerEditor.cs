using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DragonSlay.Editor
{
    [CustomEditor(typeof(SceneObjectDebugger))]
    public class SceneObjectDebuggerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            SerializedProperty type = serializedObject.FindProperty("m_SceneObjectType");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(type);
            if(EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                var debugger = target as SceneObjectDebugger;
                debugger.InitSceneObject();
            }

            SerializedProperty sceneObject = serializedObject.FindProperty("m_SceneObject");
            if (sceneObject != null)
            {
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(sceneObject);
                if (EditorGUI.EndChangeCheck())
                {
                    serializedObject.ApplyModifiedProperties();
                }
            }

        }
    }

    [CustomPropertyDrawer(typeof(Actor))]
    public class ActorPropertyDrawer:PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 0;
        }
    }

    [CustomPropertyDrawer(typeof(RecordPoint))]
    public class RecordPointPropertyDrawer:PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty rebirthPoint = property.FindPropertyRelative("m_RebirthPoint");
            EditorGUILayout.PropertyField(rebirthPoint);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 0;
        }
    }
}
