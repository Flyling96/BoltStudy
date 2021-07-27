using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ludiq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityObject = UnityEngine.Object;

namespace Bolt
{
	public class VariablesPanel : ISidebarPanelContent
	{
		public IGraphContext context { get; }

		public Vector2 minSize => new Vector2(335, 200);

		public GUIContent titleContent { get; }

		public object sidebarControlHint => typeof(VariablesPanel);

		public VariablesPanel(IGraphContext context)
		{
			this.context = context;
			
			titleContent = new GUIContent("Variables", BoltCore.Icons.variablesWindow?[IconSize.Small]);
			
			tabs.Clear();
			
			//if (context?.graph is IGraphWithVariables)
			//{
			//	tabs.Add(Graph(context.reference));
			//}
			//else
			//{
			//	tabs.Add(Graph(null));
			//}

			tabs.Add(Object(context?.reference.gameObject ?? Selection.activeGameObject));
			tabs.Add(AutoSubGraph(context?.reference.gameObject ?? Selection.activeGameObject));
			tabs.Add(AutoSceneObject(context?.reference.gameObject ?? Selection.activeGameObject));
			//tabs.Add(Scene());
			//tabs.Add(Application());
			//tabs.Add(Saved());

			_currentTab = tabs.FirstOrDefault(t => t.enabled);
			_currentSubTabIdentifier = _currentTab?.currentSubTab?.identifier;
		}

		private readonly List<Tab> tabs = new List<Tab>();

		private Tab _currentTab;

		private string _currentSubTabIdentifier;

		public string currentSubTabIdentifier
		{
			get => _currentSubTabIdentifier;
			set
			{
				_currentSubTabIdentifier = value;

				var _currentSubTab = tabs.Where(t => t.enabled).SelectMany(t => t.subTabs).FirstOrDefault(st => st.identifier == _currentSubTabIdentifier);

				if (_currentSubTab != null)
				{
					_currentTab = _currentSubTab.tab;
					_currentTab.currentSubTab = _currentSubTab;
				}
			}
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

			y += tabBarHeight;

			y--;

			if (_currentTab != null)
			{
				context?.BeginEdit();

				_currentTab.OnGUI(position, ref y);

				context?.EndEdit();
			}
		}

		private float GetTabBarHeight(float width)
		{
			return Styles.tab.fixedHeight;
		}

		private void OnTabButtonGUI(Rect position, Tab tab)
		{
			EditorGUI.BeginDisabledGroup(!tab.enabled);

			using (LudiqGUIUtility.iconSize.Override(IconSize.Small))
			{
				if (GUI.Toggle(position, _currentTab == tab, tab.label, Styles.tab) && _currentTab != tab)
				{
					var subTab = tab.subTabs.FirstOrDefault();
					currentSubTabIdentifier = subTab?.identifier;
					GUIUtility.keyboardControl = 0;
					GUIUtility.hotControl = 0;
				}
			}

			EditorGUI.EndDisabledGroup();
		}
		
		private Tab Graph(GraphReference reference)
		{
			var tab = new Tab
			(
				this,
				"Graph",
				"Graph Variables",
				"These variables are local to the current graph.",
				BoltCore.Icons.graphVariable
			);

			if (reference != null)
			{
				if (reference.hasData)
				{
					var instanceVariables = Variables.GraphInstance(reference);
					
					tab.subTabs.Add(new SubTab("Graph.Instance", tab, VariableKind.Graph, instanceVariables, reference.serializedObject, null, "Instance"));
				}

				var definitionVariables = Variables.GraphDefinition(reference);

				tab.subTabs.Add(new SubTab("Graph.Definition", tab, VariableKind.Graph, definitionVariables, reference.serializedObject, null, "Definition"));
			}

			tab.MakeFirstSubTabCurrent();

			return tab;
		}

		private Tab Object(GameObject @object)
		{
			var tab = new Tab
			(
				this,
				"Object",
				"Object Variables",
				"These variables are shared across the current game object.",
				BoltCore.Icons.objectVariable
			);

			if (@object != null)
			{
				if (@object.IsConnectedPrefabInstance())
				{
					var instance = @object;
					var definition = instance.GetPrefabDefinition();
					
					var definitionVariables = definition.GetComponent<Variables>();
					var instanceVariables = instance.GetComponent<Variables>();

					if (instanceVariables != null)
					{
						tab.subTabs.Add(new SubTab("Object.Instance", tab, VariableKind.Object, instanceVariables.declarations, instanceVariables, null, "Prefab Instance"));
					}

					if (definitionVariables != null)
					{
						tab.subTabs.Add(new SubTab("Object.Definition", tab, VariableKind.Object, definitionVariables.declarations, definitionVariables, null, "Prefab Definition"));
					}
				}
				else
				{
					var variables = @object.GetComponent<Variables>();

					if (variables != null)
					{
						tab.subTabs.Add(new SubTab("Object.Instance", tab, VariableKind.Object, variables.declarations, variables, null));
					}
				}

				tab.MakeFirstSubTabCurrent();
			}

			return tab;
		}

		private Tab AutoSubGraph(GameObject @object)
		{
			var tab = new Tab
			(
				this,
				"SubGraph",
				"Auto SubGraph Variables",
				"These variables are added automatically.",
				BoltCore.Icons.variable
			);

			if(@object != null)
			{
				var variables = @object.GetComponent<Variables>();

				if (variables != null)
				{
					tab.subTabs.Add(new SubTab("SubFlow", tab, VariableKind.AutoSubFlow, variables.subFlowDeclarations, variables, null));
				}
			}

			return tab;
		}

		private Tab AutoSceneObject(GameObject @object)
		{
			var tab = new Tab
			(
				this,
				"SceneObject",
				"Auto SceneObject Variables",
				"These sceneObjects are added automatically.",
				BoltCore.Icons.sceneVariable
			);

			if (@object != null)
			{
				var variables = @object.GetComponent<Variables>();

				if (variables != null)
				{
					tab.subTabs.Add(new SubTab("SubSceneObject", tab, VariableKind.AutoSceneObject, variables.subSceneObjectDeclarations, variables, null));
				}
			}

			return tab;
		}

		private Tab Scene()
		{
			var tab = new Tab
			(
				this,
				"Scene",
				"Scene Variables",
				"These variables are shared across the current scene.",
				BoltCore.Icons.sceneVariable
			);
				
			for (int i = 0; i < SceneManager.sceneCount; i++)
			{
				var scene = SceneManager.GetSceneAt(i);

				// Trying to fetch the singleton while the scene isn't completely
				// loaded seems to mess with the instantiation or handles and create duplicates
				if (!scene.isLoaded)
				{
					continue;
				}

				if (BoltCore.Configuration.createSceneVariables || SceneVariables.InstantiatedIn(scene))
				{
					var sceneVariables = SceneVariables.Instance(scene);
					var declarations = sceneVariables.GetComponent<Variables>().declarations;
					var owner = sceneVariables;
					var title = StringUtility.FallbackWhitespace(sceneVariables.gameObject.scene.name, "Untitled");

					tab.subTabs.Add(new SubTab("Scene", tab, VariableKind.Scene, declarations, owner, null, title));
				}
			}

			tab.MakeFirstSubTabCurrent();

			return tab;
		}

		private Tab Application()
		{
			if (EditorApplication.isPlaying)
			{
				var tab = new Tab
				(
					this,
					"App",
					"Application Variables",
					"These variables are shared across scenes. They will be reset once you exit playmode.",
					BoltCore.Icons.applicationVariable
				);

				if (ApplicationVariables.runtime != null)
				{
					tab.subTabs.Add(new SubTab("Application", tab, VariableKind.Application, ApplicationVariables.runtime, null, null));
				}

				tab.MakeFirstSubTabCurrent();

				return tab;
			}
			else
			{
				var tab = new Tab
				(
					this,
					"App",
					"Application Variables",
					"These variables are shared across scenes. They will be reset once the application quits.",
					BoltCore.Icons.applicationVariable
				);

				if (ApplicationVariables.asset?.declarations != null)
				{
					tab.subTabs.Add(new SubTab("Application", tab, VariableKind.Application, ApplicationVariables.asset.declarations, ApplicationVariables.asset, null));
				}

				tab.MakeFirstSubTabCurrent();

				return tab;
			}
		}

		private Tab Saved()
		{
			if (EditorApplication.isPlaying)
			{
				var tab = new Tab
				(
					this,
					"Saved",
					"Saved Variables",
					"These variables will persist even after the application quits. Unity object references are not supported.",
					BoltCore.Icons.savedVariable
				);

				if (SavedVariables.merged != null)
				{
					tab.subTabs.Add(new SubTab("Saved.Merged", tab, VariableKind.Saved, SavedVariables.merged, null, null, "Merged", "The currently merged variables."));
				}

				tab.MakeFirstSubTabCurrent();

				return tab;
			}
			else
			{
				var tab = new Tab
				(
					this,
					"Saved",
					"Saved Variables",
					"These variables will persist even after the application quits. Unity object references are not supported.",
					BoltCore.Icons.savedVariable
				);

				if (SavedVariables.asset?.declarations != null)
				{
					tab.subTabs.Add(new SubTab("Saved.Initial", tab, VariableKind.Saved, SavedVariables.asset.declarations, SavedVariables.asset, null, "Initial", "Default variables for new games."));
				}

				if (SavedVariables.saved != null)
				{
					tab.subTabs.Add(new SubTab("Saved.Saved", tab, VariableKind.Saved, SavedVariables.saved, null, () => SavedVariables.SaveDeclarations(SavedVariables.saved), "Saved", "The currently saved variables."));
				}

				tab.MakeFirstSubTabCurrent();

				return tab;
			}
		}

		public static class Styles
		{
			static Styles()
			{
				tab = new GUIStyle(EditorStyles.toolbarButton);
				tab.margin = new RectOffset(0, 0, 0, 0);
				tab.padding = new RectOffset(0, 0, 0, 0);
				tab.fixedHeight = 22;

				subTab = new GUIStyle(tab);
			}

			public static readonly GUIStyle tab;
			public static readonly GUIStyle subTab;
		}

		private class Tab
		{
			public Tab(VariablesPanel panel, string shortTitle, string title, string description, EditorTexture icon, params SubTab[] subTabs)
			{
				Ensure.That(nameof(panel)).IsNotNull(panel);
				Ensure.That(nameof(shortTitle)).IsNotNull(shortTitle);
				Ensure.That(nameof(title)).IsNotNull(title);
				Ensure.That(nameof(description)).IsNotNull(description);
				Ensure.That(nameof(subTabs)).IsNotNull(subTabs);

				this.panel = panel;
				
				header = new GUIContent(title, icon?[IconSize.Medium], description);
				label = new GUIContent(" " + shortTitle, icon?[IconSize.Small]);

				this.subTabs = new List<SubTab>(subTabs.NotNull());
				currentSubTab = this.subTabs.FirstOrDefault();
			}

			public readonly VariablesPanel panel;

			public readonly List<SubTab> subTabs;

			public SubTab currentSubTab { get; set; }

			public GUIContent label { get; }

			public GUIContent header { get; }

			public bool enabled => subTabs.Count > 0;
			
			public float GetHeight(float width)
			{
				var height = 0f;

				if (BoltCore.Configuration.showVariablesHelp)
				{
					height += LudiqGUI.GetHeaderHeight(header, width, false);
					height--;
				}

				if (subTabs.Count > 1)
				{
					height += GetSubTabBarHeight(width);
				}

				if (enabled && currentSubTab != null)
				{
					height += currentSubTab.GetHeight(width);
				}

				return height;
			}

			public void OnGUI(Rect position, ref float y)
			{
				if (BoltCore.Configuration.showVariablesHelp)
				{
					EditorGUI.BeginDisabledGroup(!enabled);
					LudiqGUI.OnHeaderGUI(header, position, ref y, false);
					EditorGUI.EndDisabledGroup();
				}

				if (subTabs.Count > 1)
				{
					var subTabBarHeight = GetSubTabBarHeight(position.width);
					var subTabButtonWidth = position.width / subTabs.Count;

					for (var i = 0; i < subTabs.Count; i++)
					{
						var subTab = subTabs[i];

						var subTabButtonPosition = new Rect
						(
							position.x + i * subTabButtonWidth,
							y,
							subTabButtonWidth,
							subTabBarHeight
						);

						OnSubTabButtonGUI(subTabButtonPosition, subTab);
					}

					y += subTabBarHeight;
				}

				if (BoltCore.Configuration.showVariablesHelp || subTabs.Count > 1)
				{
					y--;
				}

				if (enabled)
				{
					currentSubTab?.OnGUI(position, ref y);
				}
			}

			private float GetSubTabBarHeight(float width)
			{
				return Styles.subTab.fixedHeight;
			}

			private void OnSubTabButtonGUI(Rect position, SubTab subTab)
			{
				if (GUI.Toggle(position, currentSubTab == subTab, subTab.label, Styles.subTab) && currentSubTab != subTab)
				{
					panel.currentSubTabIdentifier = subTab.identifier;
					GUIUtility.keyboardControl = 0;
					GUIUtility.hotControl = 0;
				}
			}

			public void MakeFirstSubTabCurrent()
			{
				currentSubTab = subTabs.FirstOrDefault();
			}
		}

		// Implementation note: We're being extra careful with null checks when instantiating
		// subtabs because the variable declarations have wildly different, hard to predict lifetimes.
		// This way, the tab will just show up as disabled in the first frame if the declarations
		// aren't yet deserialized/fetched/merged/etc.

		private class SubTab
		{
			public SubTab(string identifier, Tab tab, VariableKind kind, VariableDeclarations declarations, UnityObject targetObject, Action save, string label = "Default", string tooltip = null)
			{
				Ensure.That(nameof(tab)).IsNotNull(tab);
				Ensure.That(nameof(declarations)).IsNotNull(declarations);
				Ensure.That(nameof(label)).IsNotNull(label);

				this.tab = tab;

				this.label = new GUIContent(label, tooltip);

				this.targetObject = targetObject;

				this.identifier = identifier;

				metadata = Metadata.Root().StaticObject(declarations);

				inspector = metadata.Inspector<VariableDeclarationsInspector>();

				inspector.kind = kind;

				this.save = save;
			}
			
			public string identifier { get; }

			public Tab tab { get; }

			private readonly Metadata metadata;

			private readonly VariableDeclarationsInspector inspector;
			
			private readonly UnityObject targetObject;

			private readonly Action save;

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
					EditorGUI.BeginChangeCheck();
						
					inspector.Draw(position.VerticalSection(ref y, GetDeclarationsHeight(position.width)), GUIContent.none);

					if (EditorGUI.EndChangeCheck())
					{
						save?.Invoke();
					}
				}
			}

			private float GetDeclarationsHeight(float width)
			{
				return inspector.GetCachedHeight(width, GUIContent.none, null);
			}
		}
	}
}