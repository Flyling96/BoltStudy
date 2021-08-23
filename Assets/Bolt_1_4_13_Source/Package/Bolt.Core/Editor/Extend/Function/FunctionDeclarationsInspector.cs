using Ludiq;
using UnityEditor;
using UnityEngine;

namespace Bolt.Extend
{
	[Inspector(typeof(IFunctions))]
	public class FunctionDeclarationsInspector : Inspector
	{
		public FunctionDeclarationsInspector(Metadata metadata) : base(metadata) { }

		public override void Initialize()
		{
			base.Initialize();

			adaptor = new ListAdaptor(metadata["collection"], this);
		}

		private ListAdaptor adaptor;
		private string newName;
		private bool highlightPlaceholder;
		private bool highlightNewNameField;

		protected override void OnGUI(Rect drawerPosition, GUIContent label)
		{
			bool editable = ((IFunctions)metadata.value).Editable;
			if (!editable)
			{
				GUI.enabled = false;
			}
			adaptor.Field(drawerPosition, GUIContent.none);

			drawerPosition.x = 0;
			drawerPosition.width = LudiqGUIUtility.currentInspectorWidthWithoutScrollbar;

			var newNamePosition = new Rect
			(
				drawerPosition.x,
				drawerPosition.yMax - 20,
				drawerPosition.width - Styles.addButtonWidth,
				18
			);

			if (adaptor.Count == 0)
			{
				newNamePosition.y -= 1;
				newNamePosition.height += 1;
			}

			newNamePosition.height += 1;

			OnNewNameGUI(newNamePosition);
			if (!editable)
			{
				GUI.enabled = true;
			}
		}

		protected override float GetHeight(float width, GUIContent label)
		{
			return adaptor.GetHeight(width, GUIContent.none);
		}

		private void OnNewNameGUI(Rect newNamePosition)
		{
			EditorGUI.BeginChangeCheck();

			GUI.SetNextControlName(newNameFieldControl);
			newName = EditorGUI.TextField(newNamePosition, newName, highlightNewNameField ? Styles.newNameFieldHighlighted : Styles.newNameField);

			var e = UnityEngine.Event.current;
			if (GUI.GetNameOfFocusedControl() == newNameFieldControl && e.type == EventType.KeyUp && e.keyCode == KeyCode.Return)
			{
				adaptor.Add();
				GUI.FocusControl(newNameFieldControl);
				GUI.changed = true;
			}

			if (EditorGUI.EndChangeCheck())
			{
				highlightNewNameField = false;
				highlightPlaceholder = false;
			}

			if (string.IsNullOrEmpty(newName))
			{
				GUI.Label(newNamePosition, "(New Function Name)", highlightPlaceholder ? Styles.placeholderHighlighted : Styles.placeholder);
			}
		}

		private const string newNameFieldControl = "newNameField";

		public static class Styles
		{
			static Styles()
			{
				newNameField = new GUIStyle(EditorStyles.textField);
				newNameField.alignment = TextAnchor.MiddleRight;
				newNameField.padding = new RectOffset(4, 4, 0, 0);

				placeholder = new GUIStyle(EditorStyles.label);
				placeholder.normal.textColor = EditorStyles.centeredGreyMiniLabel.normal.textColor;
				placeholder.alignment = newNameField.alignment;
				placeholder.padding = newNameField.padding;

				placeholderHighlighted = new GUIStyle(placeholder);
				placeholderHighlighted.normal.textColor = Color.red;
				placeholderHighlighted.active.textColor = Color.red;
				placeholderHighlighted.focused.textColor = Color.red;
				placeholderHighlighted.hover.textColor = Color.red;

				newNameFieldHighlighted = new GUIStyle(newNameField);
				newNameFieldHighlighted.normal.textColor = Color.red;
				newNameFieldHighlighted.active.textColor = Color.red;
				newNameFieldHighlighted.focused.textColor = Color.red;
				newNameFieldHighlighted.hover.textColor = Color.red;
			}

			public static readonly float invocationSpacing = 7;
			public static readonly float addButtonWidth = 29;
			public static readonly GUIStyle newNameField;
			public static readonly GUIStyle newNameFieldHighlighted;
			public static readonly GUIStyle placeholder;
			public static readonly GUIStyle placeholderHighlighted;
		}

		internal class ListAdaptor : MetadataListAdaptor
		{
			public ListAdaptor(Metadata metadata, FunctionDeclarationsInspector parentInspector) : base(metadata, parentInspector)
			{
				this.parentInspector = parentInspector;
				alwaysDragAndDrop = true;
			}

			public new readonly FunctionDeclarationsInspector parentInspector;

			protected override bool CanDrop(object item)
			{
				var functionDeclaration = (IFunctionElement)item;

				if (((IFunctions)parentInspector.metadata.value).IsDefined(functionDeclaration.name))
				{
					EditorUtility.DisplayDialog("Dragged function", "A function with the same name already exists.", "OK");
					return false;
				}

				return base.CanDrop(item);
			}

			protected override bool CanAdd()
			{
				if (StringUtility.IsNullOrWhiteSpace(parentInspector.newName))
				{
					parentInspector.highlightPlaceholder = true;
					EditorUtility.DisplayDialog("New function", "Please enter a function name.", "OK");
					return false;
				}
				else if (((IFunctions)parentInspector.metadata.value).IsDefined(parentInspector.newName))
				{
					parentInspector.highlightNewNameField = true;
					EditorUtility.DisplayDialog("New function", "A function with the same name already exists.", "OK");
					return false;
				}

				return true;
			}

			protected override object ConstructItem()
			{
				var newItem = ((IFunctions)parentInspector.metadata.value).CreateFunction(parentInspector.newName);
				parentInspector.newName = null;
				parentInspector.highlightPlaceholder = false;
				parentInspector.highlightNewNameField = false;
				return newItem;
			}
		}

	}
}