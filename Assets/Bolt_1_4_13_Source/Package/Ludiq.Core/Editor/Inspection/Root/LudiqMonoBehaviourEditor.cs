using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Ludiq
{
	[CustomEditor(typeof(LudiqMonoBehaviour),true)]
	public class LudiqMonoBehaviourEditor : OptimizedEditor<LudiqMonoBehaviourEditor.Individual>
	{
		public class Individual : IndividualEditor
		{
			protected override void Initialize()
			{
				metadata = Metadata.Root().StaticObject(serializedObject.targetObject);
			}

			private Metadata metadata;
			private Inspector inspector;
			private bool debugFoldout;

			public override void Dispose()
			{
				inspector?.Dispose();
			}

			public override void OnGUI()
			{
				if (EditorApplication.isCompiling)
				{
					LudiqGUI.CenterLoader();
					return;
				}

				using (LudiqEditorUtility.editedObject.Override(serializedObject.targetObject))
				{
					if (PluginContainer.anyVersionMismatch)
					{
						LudiqGUI.VersionMismatchShieldLayout();
						return;
					}

					if (inspector == null)
					{
						inspector = metadata.Editor();
					}

					EditorGUI.BeginChangeCheck();

					LudiqGUI.Space(EditorGUIUtility.standardVerticalSpacing);

					inspector.DrawLayout(GUIContent.none, 20);

					if (EditorGUI.EndChangeCheck())
					{
						editorParent.Repaint();
					}

					LudiqGUI.Space(EditorGUIUtility.standardVerticalSpacing);
				}
			}
		}

		public override void OnInspectorGUI()
		{
            var mono = target as LudiqMonoBehaviour;
            if (!mono.m_HasDeserialize)
            {
                return;
            }

            base.OnInspectorGUI();
        }
	}
}
