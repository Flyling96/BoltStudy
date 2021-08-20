using System.Collections;
using System.Collections.Generic;
using Ludiq;
using UnityEditor;
using UnityEngine;

namespace Bolt.Extend
{
    [Inspector(typeof(FunctionDeclaration))]
    public class FunctionDeclarationInspector : Inspector
    {
        private Metadata nameMetadata => metadata[nameof(FunctionDeclaration.name)];

        private Metadata sourceMetadata => metadata[nameof(FunctionDeclaration.source)];

        private Metadata embedMetadata => metadata[nameof(FunctionDeclaration.embed)];

        private Metadata macroMetadata => metadata[nameof(FunctionDeclaration.macro)];

        public FunctionDeclarationInspector(Metadata metadata) : base(metadata) { }


        protected override float GetHeight(float width, GUIContent label)
        {
            var height = 0f;

            using (LudiqGUIUtility.labelWidth.Override(Styles.labelWidth))
            {
                height += Styles.padding;
                height += EditorGUIUtility.singleLineHeight;
                height += Styles.spacing;
                height += EditorGUIUtility.singleLineHeight;
                height += Styles.padding;
            }

            return height;
        }

        protected override void OnGUI(Rect position, GUIContent label)
        {
            position = BeginBlock(metadata, position, label);

            using (LudiqGUIUtility.labelWidth.Override(Styles.labelWidth))
            {
                y += Styles.padding;
                var namePosition = position.VerticalSection(ref y, EditorGUIUtility.singleLineHeight);
                y += Styles.spacing;
                var valuePosition = position.VerticalSection(ref y, EditorGUIUtility.singleLineHeight);
                y += Styles.padding;

                OnNameGUI(namePosition);
                OnMacroGUI(valuePosition);
            }

            EndBlock(metadata);
        }

        public void OnNameGUI(Rect namePosition)
        {
            namePosition = BeginBlock(nameMetadata, namePosition);

            var newName = EditorGUI.DelayedTextField(namePosition, (string)nameMetadata.value);

            if (EndBlock(nameMetadata))
            {
                var functionDeclarations = (FunctionDeclarationCollection)metadata.parent.value;

                if (StringUtility.IsNullOrWhiteSpace(newName))
                {
                    EditorUtility.DisplayDialog("Edit Function Name", "Please enter a function name.", "OK");
                    return;
                }
                else if (functionDeclarations.Contains(newName))
                {
                    EditorUtility.DisplayDialog("Edit Function Name", "A function with the same name already exists.", "OK");
                    return;
                }

                nameMetadata.RecordUndo();
                functionDeclarations.EditorRename((FunctionDeclaration)metadata.value, newName);
                nameMetadata.value = newName;
            }
        }

        public void OnMacroGUI(Rect position)
        {
            position = BeginBlock(metadata, position, GUIContent.none);

            y += EditorGUIUtility.standardVerticalSpacing;

            var sourceAndConvertPosition = PrefixLabel(sourceMetadata, position.VerticalSection(ref y, GetSourceHeight(position.width)));

            var sourcePosition = new Rect
            (
                sourceAndConvertPosition.x,
                sourceAndConvertPosition.y,
                sourceAndConvertPosition.width / 2,
                sourceAndConvertPosition.height
            );

        }

        private float GetSourceHeight(float width)
        {
            return LudiqGUI.GetInspectorHeight(this, sourceMetadata, width);
        }

        private float GetMacroHeight(float width)
        {
            return LudiqGUI.GetInspectorHeight(this, macroMetadata, width);
        }

        public static class Styles
        {
            public static readonly float labelWidth = SystemObjectInspector.Styles.labelWidth;
            public static readonly float padding = 2;
            public static readonly float spacing = EditorGUIUtility.standardVerticalSpacing;

        }
    }
}
