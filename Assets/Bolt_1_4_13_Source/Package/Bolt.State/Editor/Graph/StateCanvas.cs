using System;
using System.Collections.Generic;
using System.Linq;
using Ludiq;
using UnityEditor;
using UnityEngine;

namespace Bolt
{
	[Canvas(typeof(StateGraph))]
	public sealed class StateCanvas : Canvas<StateGraph>
	{
		public StateCanvas(StateGraph graph) : base(graph) { }
		

		#region View

		protected override bool shouldEdgePan => base.shouldEdgePan || isCreatingTransition;

		#endregion


		


		#region Drawing

		protected override void DrawBackground()
		{
			base.DrawBackground();

			if (isCreatingTransition)
			{
				var startRect = this.Widget(transitionSource).position;
				var end = mousePosition;

				Edge startEdge, endEdge;

				GraphGUI.GetConnectionEdge
				(
					startRect.center,
					end,
					out startEdge,
					out endEdge
				);

				var start = startRect.GetEdgeCenter(startEdge);

				GraphGUI.DrawConnectionArrow(Color.white, start, end, startEdge, endEdge);
			}
		}

		#endregion



		#region Clipboard

		public override void ShrinkCopyGroup(HashSet<IGraphElement> copyGroup)
		{
			copyGroup.RemoveWhere(element =>
			{
				if (element is IStateTransition)
				{
					var transition = (IStateTransition)element;

					if (!copyGroup.Contains(transition.source) ||
						!copyGroup.Contains(transition.destination))
					{
						return true;
					}
				}

				return false;
			});
		}

		#endregion



		#region Window

		public override void OnToolbarGUI()
		{
			if (graph.states.Any(u => u.GetException(reference) != null) || graph.transitions.Any(t => t.GetException(reference) != null))
			{
				if (GUILayout.Button("Clear Errors", LudiqStyles.toolbarButton))
				{
					foreach (var state in graph.states)
					{
						state.SetException(reference, null);
					}

					foreach (var transition in graph.transitions)
					{
						transition.SetException(reference, null);
					}
				}
			}

			EditorGUI.BeginChangeCheck();

			BoltCore.Configuration.dimInactiveNodes = GUILayout.Toggle(BoltCore.Configuration.dimInactiveNodes, "Dim", LudiqStyles.toolbarButton);

			if (EditorGUI.EndChangeCheck())
			{
				BoltCore.Configuration.Save();
			}

			base.OnToolbarGUI();
		}

		#endregion



		#region Context

		protected override void OnContext()
		{
			if (isCreatingTransition)
			{
				CancelTransition();
			}
			else
			{
				base.OnContext();
			}
		}

		protected override IEnumerable<DropdownOption> GetContextOptions()
		{
			yield return new DropdownOption((Action<Vector2>)CreateFlowState, "Create Flow State");
			yield return new DropdownOption((Action<Vector2>)CreateSuperState, "Create Super State");
			yield return new DropdownOption((Action<Vector2>)CreateAnyState, "Create Any State");

			foreach (var baseOption in base.GetContextOptions())
			{
				yield return baseOption;
			}
		}

		private void CreateFlowState(Vector2 position)
		{
			var flowState = FlowState.WithEnterUpdateExit();

			if (!graph.states.Any())
			{
				flowState.isStart = true;
				flowState.nest.embed.title = "Start";
			}

			AddState(flowState, position);
		}

		private void CreateSuperState(Vector2 position)
		{
			var superState = SuperState.WithStart();

			if (!graph.states.Any())
			{
				superState.isStart = true;
				superState.nest.embed.title = "Start";
			}

			AddState(superState, position);
		}

		private void CreateAnyState(Vector2 position)
		{
			AddState(new AnyState(), position);
		}

		public void AddState(IState state, Vector2 position)
		{
			UndoUtility.RecordEditedObject("Create State");
			state.position = position;
			graph.states.Add(state);
			state.position -= this.Widget(state).position.size / 2;
			state.position = state.position.PixelPerfect();
			this.Widget(state).Reposition();
			selection.Select(state);
			GUI.changed = true;
		}

		#endregion



		#region Lifecycle

		public override void Close()
		{
			base.Close();

			CancelTransition();
		}

		protected override void HandleHighPriorityInput()
		{
			if (isCreatingTransition)
			{
				if (e.IsMouseDrag(MouseButton.Left))
				{
					// Priority over lasso
					e.Use();
				}
				else if (e.IsKeyDown(KeyCode.Escape))
				{
					CancelTransition();
					e.Use();
				}
				if (e.IsMouseDown(MouseButton.Left) || e.IsMouseUp(MouseButton.Left))
				{
					CompleteTransitionToNewState();
					e.Use();
				}
			}

			base.HandleHighPriorityInput();
		}

		public void CompleteTransitionToNewState()
		{
			var startRect = this.Widget(transitionSource).position;
			var end = mousePosition;
			
			GraphGUI.GetConnectionEdge
			(
				startRect.center,
				end,
				out var startEdge,
				out var endEdge
			);

			var destination = FlowState.WithEnterUpdateExit();
			graph.states.Add(destination);
			
			Vector2 offset;

			var size = this.Widget(destination).position.size;

			switch (endEdge)
			{
				case Edge.Left:
					offset = new Vector2(0, -size.y / 2);
					break;
				case Edge.Right:
					offset = new Vector2(-size.x, -size.y / 2);
					break;
				case Edge.Top:
					offset = new Vector2(-size.x / 2, 0);
					break;
				case Edge.Bottom:
					offset = new Vector2(-size.x / 2, -size.y);
					break;
				default:
					throw new UnexpectedEnumValueException<Edge>(endEdge);
			}

			destination.position = mousePosition + offset;

			destination.position = destination.position.PixelPerfect();

			EndTransition(destination);
		}

		#endregion



		#region Drag & Drop

		public override bool AcceptsDragAndDrop()
		{
			return DragAndDropUtility.Is<FlowMacro>() || DragAndDropUtility.Is<StateMacro>();
		}

		public override void PerformDragAndDrop()
		{
			if (DragAndDropUtility.Is<FlowMacro>())
			{
				var flowMacro = DragAndDropUtility.Get<FlowMacro>();
				var flowState = new FlowState(flowMacro);
				AddState(flowState, DragAndDropUtility.position);
			}
			else if (DragAndDropUtility.Is<StateMacro>())
			{
				var asset = DragAndDropUtility.Get<StateMacro>();
				var superState = new SuperState(asset);
				AddState(superState, DragAndDropUtility.position);
			}
		}

		public override void DrawDragAndDropPreview()
		{
			if (DragAndDropUtility.Is<FlowMacro>())
			{
				GraphGUI.DrawDragAndDropPreviewLabel(DragAndDropUtility.offsetedPosition, DragAndDropUtility.Get<FlowMacro>().name, typeof(FlowMacro).Icon());
			}
			else if (DragAndDropUtility.Is<StateMacro>())
			{
				GraphGUI.DrawDragAndDropPreviewLabel(DragAndDropUtility.offsetedPosition, DragAndDropUtility.Get<StateMacro>().name, typeof(StateMacro).Icon());
			}
		}

		#endregion


		#region Transition Creation
		
		public IState transitionSource { get; set; }

		public bool isCreatingTransition => transitionSource != null;

		public void StartTransition(IState source)
		{
			transitionSource = source;
			window.Focus();
		}

		public void EndTransition(IState destination)
		{
			UndoUtility.RecordEditedObject("Create State Transition");

			var transition = FlowStateTransition.WithDefaultTrigger(transitionSource, destination);
			graph.transitions.Add(transition);
			transitionSource = null;
			this.Widget(transition).BringToFront();
			selection.Select(transition);
			GUI.changed = true;
		}

		public void CancelTransition()
		{
			transitionSource = null;
		}

		#endregion
	}
}