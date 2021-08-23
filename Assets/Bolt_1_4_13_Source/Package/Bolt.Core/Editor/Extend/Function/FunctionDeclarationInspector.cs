using System.Collections;
using System.Collections.Generic;
using Ludiq;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace Bolt.Extend
{
    [Inspector(typeof(IFunctionElement))]
    public class FunctionDeclarationInspector : Inspector
    {
        private Metadata nameMetadata => metadata[nameof(IFunctionElement.name)];

        private Metadata sourceMetadata => metadata[nameof(IFunctionElement.source)];

        private Metadata embedMetadata => metadata[nameof(IFunctionElement.embed)];

        private Metadata macroMetadata => metadata[nameof(IFunctionElement.macro)];

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
                //OnMacroGUI(valuePosition);
            }

            EndBlock(metadata);
        }

        public void OnNameGUI(Rect namePosition)
        {
            namePosition = BeginBlock(nameMetadata, namePosition);

            var newName = EditorGUI.DelayedTextField(namePosition, (string)nameMetadata.value);

            if (EndBlock(nameMetadata))
            {
                var functionDeclarations = (FunctionDeclarationCollection<IFunctionElement>)metadata.parent.value;

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
                functionDeclarations.EditorRename((IFunctionElement)metadata.value, newName);
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

        //private void OnNewEmbedGraphButtonGUI(Rect position)
        //{
        //    if (GUI.Button(position, "New", Styles.newButton))
        //    {
        //        metadata.RecordUndo();
        //        sourceMetadata.value = GraphSource.Embed;
        //        embedMetadata.value = NewGraph();
        //        macroMetadata.value = null;
        //        UpdateActiveGraph();
        //    }
        //}

        //private void OnNewMacroButtonGUI(Rect position)
        //{
        //    if (GUI.Button(position, "New", Styles.newButton))
        //    {
        //        var macro = (IMacro)ScriptableObject.CreateInstance(macroType);
        //        var macroObject = (UnityObject)macro;
        //        macro.graph = NewGraph();

        //        var path = EditorUtility.SaveFilePanelInProject("Save Macro", macroObject.name, "asset", null);

        //        if (!string.IsNullOrEmpty(path))
        //        {
        //            AssetDatabase.CreateAsset(macroObject, path);
        //            metadata.RecordUndo();
        //            sourceMetadata.value = GraphSource.Macro;
        //            macroMetadata.value = macro;
        //            embedMetadata.value = null;
        //            UpdateActiveGraph();
        //        }
        //    }
        //}

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
            public static readonly GUIStyle newButton;

            static Styles()
            {
                newButton = new GUIStyle(EditorStyles.miniButton);
            }

        }
    }
}
