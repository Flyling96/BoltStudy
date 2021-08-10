using Bolt;
using Ludiq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class CustomVariableNameInspector : Inspector
{
    public CustomVariableNameInspector(Metadata metadata, Func<IEnumerable<string>> getSuggestions, Action onValueChange) : base(metadata)
    {
        Ensure.That(nameof(getSuggestions)).IsNotNull(getSuggestions);

        this.getSuggestions = getSuggestions;
        this.onValueChange = onValueChange;
    }

    public Func<IEnumerable<string>> getSuggestions { get; }

    public Action onValueChange;


    protected override float GetHeight(float width, GUIContent label)
    {
        return HeightWithLabel(metadata, width, GetFieldHeight(width, label), label);
    }

    private float GetFieldHeight(float width, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight;
    }

    protected List<string> suggestions = new List<string>();

    protected override void OnGUI(Rect position, GUIContent label)
    {
        position = BeginBlock(metadata, position, label);

        var fieldPosition = position.VerticalSection(ref y, GetFieldHeight(position.width, GUIContent.none));

        var textFieldPosition = new Rect
        (
            fieldPosition.x,
            fieldPosition.y,
            fieldPosition.width - Styles.popup.fixedWidth,
            fieldPosition.height
        );

        var popupPosition = new Rect
        (
            textFieldPosition.xMax,
            fieldPosition.y,
            Styles.popup.fixedWidth,
            fieldPosition.height
        );

        var newValue = EditorGUI.TextField(textFieldPosition, (string)metadata.value, Styles.textField);

        // Micro optimizing memory here because it's a pretty substantial alloc

        suggestions.Clear();
        suggestions.AddRange(getSuggestions());

        EditorGUI.BeginDisabledGroup(suggestions.Count == 0);

        var suggestionsArray = getSuggestions().ToArray();
        var currentSuggestionIndex = Array.IndexOf(suggestionsArray, (string)metadata.value);

        EditorGUI.BeginChangeCheck();

        var newSuggestionIndex = EditorGUI.Popup(popupPosition, currentSuggestionIndex, suggestionsArray, Styles.popup);

        if (EditorGUI.EndChangeCheck())
        {
            newValue = suggestions[newSuggestionIndex];
        }

        EditorGUI.EndDisabledGroup();

        if (EndBlock(metadata))
        {
            metadata.RecordUndo();
            metadata.value = newValue;
            onValueChange?.Invoke();
        }
    }

    public override float GetAdaptiveWidth()
    {
        return Mathf.Max(30, EditorStyles.textField.CalcSize(new GUIContent(metadata.value?.ToString())).x + 1 + Styles.popup.fixedWidth);
    }

    public static class Styles
    {
        static Styles()
        {
            textField = new GUIStyle(EditorStyles.textField);

            popup = new GUIStyle("TextFieldDropDown");
            popup.fixedWidth = 12;
            popup.clipping = TextClipping.Clip;
            popup.normal.textColor = ColorPalette.transparent;
            popup.active.textColor = ColorPalette.transparent;
            popup.hover.textColor = ColorPalette.transparent;
            popup.focused.textColor = ColorPalette.transparent;
            popup.onNormal.textColor = ColorPalette.transparent;
            popup.onActive.textColor = ColorPalette.transparent;
            popup.onHover.textColor = ColorPalette.transparent;
            popup.onFocused.textColor = ColorPalette.transparent;
        }

        public static readonly GUIStyle textField;
        public static readonly GUIStyle popup;
    }

}
