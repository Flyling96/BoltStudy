using System.Collections.Generic;
using Ludiq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Bolt
{
	public interface ISubVariable
    {
		int SubObjectId { get; }
    }
	[AddComponentMenu("Bolt/Variables")]
	[DisableAnnotation]
	[IncludeInSettings(false)]
	public class Variables : LudiqBehaviour, IAotStubbable
	{
		[Serialize, Inspectable, VariableKind(VariableKind.Object)]
		public VariableDeclarations declarations { get; internal set; } = new VariableDeclarations();

		[Serialize, Inspectable, VariableKind(VariableKind.AutoSubFlow)]
		public VariableDeclarations subFlowDeclarations { get; internal set; } = new VariableDeclarations();

		[Serialize, Inspectable, VariableKind(VariableKind.AutoSceneObject)]
		public VariableDeclarations subSceneObjectDeclarations { get; internal set; } = new VariableDeclarations();

		public static VariableDeclarations Graph(GraphPointer pointer)
		{
			Ensure.That(nameof(pointer)).IsNotNull(pointer);

			if (pointer.hasData)
			{
				return GraphInstance(pointer);
			}
			else
			{
				return GraphDefinition(pointer);
			}
		}

		public static VariableDeclarations GraphInstance(GraphPointer pointer)
		{
			return pointer.GetGraphData<IGraphDataWithVariables>().variables;
		}

		public static VariableDeclarations GraphDefinition(GraphPointer pointer)
		{
			return GraphDefinition((IGraphWithVariables)pointer.graph);
		}

		public static VariableDeclarations GraphDefinition(IGraphWithVariables graph)
		{
			return graph.variables;
		}

		public static VariableDeclarations Object(GameObject go) => go.GetOrAddComponent<Variables>().declarations;

		public static VariableDeclarations AutoSubFlow(GameObject go) => go.GetOrAddComponent<Variables>().subFlowDeclarations;

		public static VariableDeclarations AutoSceneObject(GameObject go) => go.GetOrAddComponent<Variables>().subSceneObjectDeclarations;

		public static VariableDeclarations Object(Component component) => Object(component.gameObject);

		public static VariableDeclarations Scene(Scene? scene) => SceneVariables.For(scene);

		public static VariableDeclarations Scene(GameObject go) => Scene(go.scene);

		public static VariableDeclarations Scene(Component component) => Scene(component.gameObject);

		public static VariableDeclarations ActiveScene => Scene(SceneManager.GetActiveScene());

		public static VariableDeclarations Application => ApplicationVariables.current;

		public static VariableDeclarations Saved => SavedVariables.current;

		public static bool ExistOnObject(GameObject go) => go.GetComponent<Variables>() != null;

		public static bool ExistOnObject(Component component) => ExistOnObject(component.gameObject);

		public static bool ExistInScene(Scene? scene) => scene != null && SceneVariables.InstantiatedIn(scene.Value);

		public static bool ExistInActiveScene => ExistInScene(SceneManager.GetActiveScene());

		[ContextMenu("Show Data...")]
		protected override void ShowData()
		{
			base.ShowData();
		}

		public IEnumerable<object> aotStubs
		{
			get
			{
				// Include the constructors for AOT serialization
				// https://support.ludiq.io/communities/5/topics/3952-x
				foreach (var declaration in declarations)
				{
					var defaultConstructor = declaration.value?.GetType().GetPublicDefaultConstructor();
					
					if (defaultConstructor != null)
					{
						yield return defaultConstructor;
					}
				}
			}
		}

		#region Auto
		[HideInInspector]
		public int m_CurrentSubObjectId = 0;
        
		public void RemoveNull()
        {
			List<string> nullKeys = new List<string>();
            foreach (var variable in subFlowDeclarations)
            {
				if(variable.value == null)
                {
					nullKeys.Add(variable.name);
                }
            }

            for (int i = 0; i < nullKeys.Count; i++)
            {
				subFlowDeclarations.Remove(nullKeys[i]);
            }

			nullKeys.Clear();
			foreach (var variable in subSceneObjectDeclarations)
			{
				if (variable.value == null)
				{
					nullKeys.Add(variable.name);
				}
			}

			for (int i = 0; i < nullKeys.Count; i++)
			{
				subSceneObjectDeclarations.Remove(nullKeys[i]);
			}
		}

		public bool AddSubFlow(ISubVariable subVariable,MonoBehaviour subFlow)
        {
            foreach (var variable in subFlowDeclarations)
            {
				if(variable.value != null && variable.value is MonoBehaviour mono && mono == subFlow)
                {
					return false;
                }
            }

			var key = subFlow.gameObject.name;
			subFlowDeclarations[key] = subFlow;
			return true;
        }

		public void RemoveSubFlow(ISubVariable subVariable,MonoBehaviour subFlow)
        {
			RemoveSubFlow(subVariable, subFlow.gameObject.name);
        }

		public void RemoveSubFlow(ISubVariable subVariable,string name)
        {
			subFlowDeclarations.Remove(name);
        }

		public bool AddSceneObject(ISubVariable subVariable, MonoBehaviour subFlow)
		{
			foreach (var variable in subSceneObjectDeclarations)
			{
				if (variable.value != null && variable.value is MonoBehaviour mono && mono == subFlow)
				{
					return false;
				}
			}

			var key = subFlow.gameObject.name;
			subSceneObjectDeclarations[key] = subFlow;
			return true;
		}

		public void RemoveSceneObject(ISubVariable subVariable, MonoBehaviour subFlow)
		{
			RemoveSceneObject(subVariable, subFlow.gameObject.name);
		}

		public void RemoveSceneObject(ISubVariable subVariable, string name)
		{
			subSceneObjectDeclarations.Remove(name);
		}

		public void RenameSceneObject(string originStr,string newStr)
        {
			var value = subSceneObjectDeclarations[originStr];
			subSceneObjectDeclarations.Remove(originStr);
			if(value != null)
            {
				subSceneObjectDeclarations[newStr] = value;
            }
        }

		public void RenameSubFlow(string originStr, string newStr)
		{
			var value = subFlowDeclarations[originStr];
			subFlowDeclarations.Remove(originStr);
			if (value != null)
			{
				subFlowDeclarations[newStr] = value;
			}
		}

		#endregion
	}

}
