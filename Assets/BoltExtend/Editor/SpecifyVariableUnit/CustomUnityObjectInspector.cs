using Ludiq;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

public class CustomUnityObjectInspector : Inspector
{
    public CustomUnityObjectInspector(Metadata metadata) : base(metadata) { }

    protected override float GetHeight(float width, GUIContent label)
    {
        return HeightWithLabel(metadata, width, EditorGUIUtility.singleLineHeight, label);
    }

    protected override void OnGUI(Rect position, GUIContent label)
    {
        var fieldPosition = new Rect
        (
            position.x,
            position.y,
            position.width,
            EditorGUIUtility.singleLineHeight
        );

        var allowSceneObjects = LudiqEditorUtility.editedObject.value.AsUnityNull()?.IsSceneBound() ?? false;

        EditorGUI.ObjectField(fieldPosition, (UnityObject)metadata.value, metadata.definedType, allowSceneObjects);
    }

    public override float GetAdaptiveWidth()
    {
        string label;
        bool icon = false;

        if (metadata.value.IsUnityNull())
        {
            label = "None";
            icon = true;
        }
        else
        {
            label = ((UnityObject)metadata.value).name;
            icon = true;
        }

        var width = EditorStyles.objectField.CalcSize(new GUIContent(label)).x;

        if (icon)
        {
            width += 15;
        }

        return width;
    }

}
