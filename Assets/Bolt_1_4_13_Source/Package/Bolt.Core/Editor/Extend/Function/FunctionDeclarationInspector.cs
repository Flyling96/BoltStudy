using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ludiq;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace Bolt.Extend
{
    [Inspector(typeof(IGraphFunctionElement))]
    public class FunctionDeclarationInspector : Inspector
    {
        private Metadata nameMetadata => metadata[nameof(IGraphFunctionElement.name)];

        private Metadata sourceMetadata => metadata[nameof(IGraphFunctionElement.source)];

        private Metadata embedGraphMetadata => metadata[nameof(IGraphFunctionElement.embed)];

        private Metadata macroGraphMetadata => metadata[nameof(IGraphFunctionElement.macro)];

        protected Type graphType => ((Type)metadata[nameof(IGraphFunctionElement.graphType)].value);

        protected Type macroType => ((Type)metadata[nameof(IGraphFunctionElement.macroType)].value);

        public FunctionDeclarationInspector(Metadata metadata) : base(metadata) { }


        protected virtual GraphReference reference
        {
            get
            {
                var functionElement = (IGraphFunctionElement)metadata.value;
                functionElement.self = LudiqGraphsEditorUtility.editedContext.value.reference.gameObject;
                return LudiqGraphsEditorUtility.editedContext.value.reference.ChildReference(functionElement, false);
            }
        }

        private void UpdateActiveGraph()
        {
            if (reference?.isRoot ?? false)
            {
                GraphWindow.activeReference = reference;
            }
        }

        protected override float GetHeight(float width, GUIContent label)
        {
            var height = 0f;

            using (LudiqGUIUtility.labelWidth.Override(Styles.labelWidth))
            {
                height += Styles.padding;
                height += EditorGUIUtility.singleLineHeight;
                height += Styles.spacing;
                height += GetGraphHeight(width);
                height += Styles.padding;
            }

            return height;
        }

        private float GetGraphHeight(float width)
        {
            var height = 0f;

            height += EditorGUIUtility.standardVerticalSpacing;

            height += GetSourceHeight(width);

            var source = (GraphSource)sourceMetadata.value;

            if (source == GraphSource.Embed) { }
            else if (source == GraphSource.Macro)
            {
                height += EditorGUIUtility.standardVerticalSpacing;
                height += GetMacroHeight(width);
            }

            //if (warnBackgroundEmbed)
            //{
            //    height += EditorGUIUtility.standardVerticalSpacing;
            //    height += GetBackgroundEmbedWarningHeight(width);
            //}

            //height += EditorGUIUtility.standardVerticalSpacing;
            //height += Styles.spaceBeforeEditButton;
            //height += GetEditButtonHeight(width);
            //height += EditorGUIUtility.standardVerticalSpacing;

            //if (warnComponentPrefab)
            //{
            //    height += EditorGUIUtility.standardVerticalSpacing;
            //    height += GetComponentPrefabWarningHeight(width);
            //}

            height += EditorGUIUtility.standardVerticalSpacing;

            return height;
        }


        protected override void OnGUI(Rect position, GUIContent label)
        {
            position = BeginBlock(metadata, position, label);

            using (LudiqGUIUtility.labelWidth.Override(Styles.labelWidth))
            {
                y += Styles.padding;
                var namePosition = position.VerticalSection(ref y, EditorGUIUtility.singleLineHeight);

                OnNameGUI(namePosition);
                //OnMacroGUI(valuePosition);
                OnGraphGUI(namePosition);
                y += Styles.padding;
                OnDoubleClick(position);
            }

            EndBlock(metadata);
        }

        private void OnDoubleClick(Rect position)
        {
            var controlID = GUIUtility.GetControlID(FocusType.Passive);
            var mousePoition = Event.current.mousePosition;

            bool isInside = mousePoition.x > position.xMin && mousePoition.x <= position.xMax && mousePoition.y > position.yMin && mousePoition.y <= position.yMax;

            if (Event.current.GetTypeForControl(controlID) == EventType.MouseDown && isInside)
            {
                if (Event.current.button == (int)MouseButton.Left && Event.current.clickCount == 2)
                {
                    GraphWindow.OpenActive(reference);
                }
            }
        }

        protected void OnGraphGUI(Rect position)
        {
            position = BeginBlock(metadata, position, GUIContent.none);

            y += EditorGUIUtility.standardVerticalSpacing;

            var sourceAndConvertPosition = PrefixLabel(sourceMetadata, position.VerticalSection(ref y, GetSourceHeight(position.width)));

            var sourcePosition = new Rect
            (
                sourceAndConvertPosition.x,
                sourceAndConvertPosition.y,
                (sourceAndConvertPosition.width - Styles.spaceBeforeButton) / 2,
                sourceAndConvertPosition.height
            );

            var convertButtonPosition = new Rect
            (
                sourcePosition.xMax + Styles.spaceBeforeButton,
                sourceAndConvertPosition.y,
                (sourceAndConvertPosition.width - Styles.spaceBeforeButton) / 2,
                sourceAndConvertPosition.height - 1
            );

            OnSourceGUI(sourcePosition);

            var source = (GraphSource)sourceMetadata.value;

            if (source == GraphSource.Embed)
            {
                if (embedGraphMetadata.value == null)
                {
                    OnNewEmbedGraphButtonGUI(convertButtonPosition);
                }
                else
                {
                    OnConvertToMacroButtonGUI(convertButtonPosition);
                }
            }
            else if (source == GraphSource.Macro)
            {
                EditorGUI.BeginDisabledGroup(macroGraphMetadata.value == null);
                OnConvertToEmbedButtonGUI(convertButtonPosition);
                EditorGUI.EndDisabledGroup();
            }

            if (source == GraphSource.Embed) { }
            else if (source == GraphSource.Macro)
            {
                y += EditorGUIUtility.standardVerticalSpacing;

                if (macroGraphMetadata.value == null)
                {
                    var macroAndNewButtonPosition = PrefixLabel(macroGraphMetadata, position.VerticalSection(ref y, GetMacroHeight(position.width)));

                    var macroPosition = new Rect
                    (
                        macroAndNewButtonPosition.x,
                        macroAndNewButtonPosition.y,
                        (macroAndNewButtonPosition.width - Styles.spaceBeforeButton) / 2,
                        macroAndNewButtonPosition.height
                    );

                    var newButtonPosition = new Rect
                    (
                        macroPosition.xMax + Styles.spaceBeforeButton,
                        macroAndNewButtonPosition.y,
                        (macroAndNewButtonPosition.width - Styles.spaceBeforeButton) / 2,
                        macroAndNewButtonPosition.height - 1
                    );

                    OnMacroGUI(macroPosition);
                    OnNewMacroButtonGUI(newButtonPosition);
                }
                else
                {
                    var macroPosition = PrefixLabel(macroGraphMetadata, position.VerticalSection(ref y, GetMacroHeight(position.width)));

                    OnMacroGUI(macroPosition);
                }
            }

            //if (warnBackgroundEmbed)
            //{
            //    y += EditorGUIUtility.standardVerticalSpacing;

            //    OnBackgroundEmbedWarningGUI(position, ref y);
            //}

            EndBlock(metadata);

            //y += EditorGUIUtility.standardVerticalSpacing;

            //y += Styles.spaceBeforeEditButton;

            //OnEditButtonGUI(position.VerticalSection(ref y, GetEditButtonHeight(position.width)));

            //y += EditorGUIUtility.standardVerticalSpacing;

            //if (warnComponentPrefab)
            //{
            //    y += EditorGUIUtility.standardVerticalSpacing;

            //    var componentPrefabWarningPosition = position.VerticalSection(ref y, GetComponentPrefabWarningHeight(position.width));

            //    OnComponentPrefabWarningGUI(componentPrefabWarningPosition);
            //}
        }


        public void OnNameGUI(Rect namePosition)
        {
            namePosition = BeginBlock(nameMetadata, namePosition);

            var newName = EditorGUI.DelayedTextField(namePosition, (string)nameMetadata.value);

            if (EndBlock(nameMetadata))
            {
                var functionDeclarations = (FunctionDeclarationCollection<IGraphFunctionElement>)metadata.parent.value;

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
                functionDeclarations.EditorRename((IGraphFunctionElement)metadata.value, newName);
                nameMetadata.value = newName;
            }
        }

        private void OnSourceGUI(Rect position)
        {
            position = BeginBlock(sourceMetadata, position, GUIContent.none);

            var previousSource = (GraphSource)sourceMetadata.value;
            var newSource = (GraphSource)EditorGUI.EnumPopup(position, (Enum)sourceMetadata.value);

            if (EndBlock(sourceMetadata))
            {
                if (previousSource == GraphSource.Embed &&
                    newSource == GraphSource.Macro &&
                    embedGraphMetadata.value != null &&
                    !EditorUtility.DisplayDialog("Delete Embed Graph", "Switching to a macro source will delete the current embed graph. Are you sure you want to switch and delete the current embed graph?", "Switch", "Cancel"))
                {
                    return;
                }

                sourceMetadata.RecordUndo();

                if (previousSource == GraphSource.Embed && newSource == GraphSource.Macro)
                {
                    embedGraphMetadata.value = null;
                }
                else if (previousSource == GraphSource.Macro && newSource == GraphSource.Embed)
                {
                    macroGraphMetadata.value = null;
                    embedGraphMetadata.value = NewGraph();
                }

                sourceMetadata.value = newSource;
                UpdateActiveGraph();
            }
        }

        public void OnMacroGUI(Rect position)
        {
            EditorGUI.BeginChangeCheck();

            LudiqGUI.Inspector(macroGraphMetadata, position, GUIContent.none);

            if (EditorGUI.EndChangeCheck())
            {
                UpdateActiveGraph();
            }

        }

        private void OnConvertToMacroButtonGUI(Rect position)
        {
            if (GUI.Button(position, "Convert", Styles.convertButton))
            {
                var embedGraph = (IGraph)embedGraphMetadata.value;
                var hasSceneReferences = embedGraph.Serialize().objectReferences.Any(uo => uo.IsSceneBound());

                if (hasSceneReferences && !EditorUtility.DisplayDialog("Scene References Detected", "This graph contains references to objects in the scene that will be lost when converting to a macro. Are you sure you want to continue?", "Convert", "Cancel"))
                {
                    return;
                }

                var macro = (IMacro)ScriptableObject.CreateInstance(macroType);
                var macroObject = (UnityObject)macro;
                macro.graph = (IGraph)embedGraphMetadata.value.CloneViaSerialization();

                var path = EditorUtility.SaveFilePanelInProject("Save Macro", macroObject.name, "asset", null);

                if (!string.IsNullOrEmpty(path))
                {
                    metadata.RecordUndo();
                    AssetDatabase.CreateAsset(macroObject, path);
                    sourceMetadata.value = GraphSource.Macro;
                    macroGraphMetadata.value = macro;
                    embedGraphMetadata.value = null;
                    UpdateActiveGraph();
                }
            }
        }

        private void OnConvertToEmbedButtonGUI(Rect position)
        {
            if (GUI.Button(position, "Convert", Styles.convertButton))
            {
                if (embedGraphMetadata.value == null || EditorUtility.DisplayDialog("Overwrite Embed Graph", "Are you sure you want to overwrite the current embed graph?", "Overwrite", "Cancel"))
                {
                    metadata.RecordUndo();
                    var newEmbedGraph = (IGraph)macroGraphMetadata.value.CloneViaSerialization();
                    sourceMetadata.value = GraphSource.Embed;
                    embedGraphMetadata.value = newEmbedGraph;
                    macroGraphMetadata.value = null;
                    UpdateActiveGraph();
                }
            }
        }

        private IGraph NewGraph()
        {
            if(metadata != null)
            {
                IGraphFunctionElement element = metadata.value as IGraphFunctionElement;
                if(element != null)
                {
                    return element.DefaultGraph();
                }
            }
            return null;
        }

        private void OnNewEmbedGraphButtonGUI(Rect position)
        {
            if (GUI.Button(position, "New", Styles.newButton))
            {
                metadata.RecordUndo();
                sourceMetadata.value = GraphSource.Embed;
                embedGraphMetadata.value = NewGraph();
                macroGraphMetadata.value = null;
                UpdateActiveGraph();
            }
        }

        private void OnNewMacroButtonGUI(Rect position)
        {
            if (GUI.Button(position, "New", Styles.newButton))
            {
                var macro = (IMacro)ScriptableObject.CreateInstance(macroType);
                var macroObject = (UnityObject)macro;
                macro.graph = NewGraph();

                var path = EditorUtility.SaveFilePanelInProject("Save Macro", macroObject.name, "asset", null);

                if (!string.IsNullOrEmpty(path))
                {
                    AssetDatabase.CreateAsset(macroObject, path);
                    metadata.RecordUndo();
                    sourceMetadata.value = GraphSource.Macro;
                    macroGraphMetadata.value = macro;
                    embedGraphMetadata.value = null;
                    UpdateActiveGraph();
                }
            }
        }

        private float GetSourceHeight(float width)
        {
            return LudiqGUI.GetInspectorHeight(this, sourceMetadata, width);
        }

        private float GetMacroHeight(float width)
        {
            return LudiqGUI.GetInspectorHeight(this, macroGraphMetadata, width);
        }

        public static class Styles
        {
            public static readonly float labelWidth = SystemObjectInspector.Styles.labelWidth;
            public static readonly float padding = 2;
            public static readonly float spacing = EditorGUIUtility.standardVerticalSpacing;
            public static readonly GUIStyle newButton;
            public static readonly GUIStyle convertButton;
            public static readonly float spaceBeforeButton = 3;
            public static readonly float spaceBeforeEditButton = 3;

            static Styles()
            {
                newButton = new GUIStyle(EditorStyles.miniButton);
                convertButton = new GUIStyle(EditorStyles.miniButton);
            }

        }
    }
}
