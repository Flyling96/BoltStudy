using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ludiq;
using UnityEditor;
using System.Linq;

namespace Bolt.Extend
{

    public class FunctionPanel : ISidebarPanelContent
    {
        public object sidebarControlHint => typeof(FunctionPanel);

        public GUIContent titleContent { get; }

        public Vector2 minSize => new Vector2(335,200);

		private readonly List<Tab> tabs = new List<Tab>();
		private Tab _currentTab = null;

		public IGraphContext context { get; }

		public FunctionPanel(IGraphContext context)
		{
			this.context = context;

			titleContent = new GUIContent("Functions", LudiqGraphs.Icons.window?[IconSize.Small]);

			var reference = context.reference;

			if (reference != null)
			{
				if (reference.hasData)
				{
					var instanceVariables = Functions.GraphInstance(reference);

					tabs.Add(new Tab("Instance",instanceVariables, reference.serializedObject, "Instance"));
				}

				var definitionVariables = Functions.GraphDefinition(reference);

				tabs.Add(new Tab("Definition", definitionVariables, context.reference.serializedObject, "Definition"));
			}


			_currentTab = tabs.FirstOrDefault(t => t.enabled);

		}

		public float GetHeight(float width)
		{
			var height = 0f;

			height += GetTabBarHeight(width);

			if (_currentTab != null)
			{
				context?.BeginEdit(false);

				height += _currentTab.GetHeight(width);

				context?.EndEdit();
			}

			return height;
		}

		public void OnGUI(Rect position)
        {
			var y = position.y;

			var tabBarHeight = GetTabBarHeight(position.width);
			var tabButtonWidth = position.width / tabs.Count;

			if (tabs.Count > 1)
			{
				for (var i = 0; i < tabs.Count; i++)
				{
					var tab = tabs[i];

					var tabButtonPosition = new Rect
					(
						position.x + i * tabButtonWidth,
						y,
						tabButtonWidth,
						tabBarHeight
					);

					OnTabButtonGUI(tabButtonPosition, tab);
				}
			}

			y += tabBarHeight;
			y--;

			if (_currentTab != null)
			{
				context?.BeginEdit();

				_currentTab.OnGUI(position, ref y);

				context?.EndEdit();
			}

		}

		private void OnTabButtonGUI(Rect position, Tab tab)
		{
			EditorGUI.BeginDisabledGroup(!tab.enabled);

			using (LudiqGUIUtility.iconSize.Override(IconSize.Small))
			{
				if (GUI.Toggle(position, _currentTab == tab, tab.label, Styles.tab) && _currentTab != tab)
				{
					_currentTab = tab;
					GUIUtility.keyboardControl = 0;
					GUIUtility.hotControl = 0;
				}
			}

			EditorGUI.EndDisabledGroup();
		}

		private float GetTabBarHeight(float width)
		{
			return Styles.tab.fixedHeight;
		}


		public static class Styles
		{
			static Styles()
			{
				tab = new GUIStyle(EditorStyles.toolbarButton);
				tab.margin = new RectOffset(0, 0, 0, 0);
				tab.padding = new RectOffset(0, 0, 0, 0);
				tab.fixedHeight = 22;
			}

			public static readonly GUIStyle tab;
		}

		private class Tab
        {
			public Tab(string identifier, IGraphFunctions declarations, Object targetObject, string label = "Default", string tooltip = null,bool enabled = true)
			{
				Ensure.That(nameof(declarations)).IsNotNull(declarations);
				Ensure.That(nameof(label)).IsNotNull(label);

				this.label = new GUIContent(label, tooltip);
				this.targetObject = targetObject;
				this.identifier = identifier;
				this.enabled = enabled;

				metadata = Metadata.Root().StaticObject(declarations);

				inspector = metadata.Inspector<FunctionDeclarationsInspector>();
			}

			public string identifier { get; }

			public bool enabled { get; }

			private readonly Metadata metadata;

			private readonly FunctionDeclarationsInspector inspector;

			private readonly Object targetObject;


			public GUIContent label { get; }

			public float GetHeight(float width)
			{
				using (LudiqEditorUtility.editedObject.Override(targetObject))
				{
					return GetDeclarationsHeight(width);
				}
			}

			public void OnGUI(Rect position, ref float y)
			{
				using (LudiqEditorUtility.editedObject.Override(targetObject))
				{
					inspector.Draw(position.VerticalSection(ref y, GetDeclarationsHeight(position.width)), GUIContent.none);
				}
			}

			private float GetDeclarationsHeight(float width)
			{
				return inspector.GetCachedHeight(width, GUIContent.none, null);
			}
		}

    }
}
