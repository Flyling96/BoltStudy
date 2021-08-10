using Ludiq;
using System.Collections.Generic;
using UnityEngine;

namespace Bolt
{
	[AddComponentMenu("Bolt/Flow Machine")]
	[RequireComponent(typeof(Variables))]
	[DisableAnnotation]
	public sealed class FlowMachine : EventMachine<FlowGraph, FlowMacro>
	{
		public override FlowGraph DefaultGraph()
		{
			return FlowGraph.WithStartUpdate();
		}

		private void SubFlowTriggerEvent(string eventName)
        {
			var subFlows = Variables.AutoSubFlow(gameObject);
			foreach (var subFlow in subFlows)
			{
				var subFlowMachine = subFlow.value as Bolt.Extend.SubFlowMachine;
				if (subFlowMachine != null)
				{
					subFlowMachine.TriggerEvent(eventName);
				}
			}
		}

		protected override void OnEnable()
		{
			if (hasGraph)
			{
				graph.StartListening(reference);
			}

			var subFlows = Variables.AutoSubFlow(gameObject);
			foreach (var subFlow in subFlows)
			{
				var subFlowMachine = subFlow.value as Bolt.Extend.SubFlowMachine;
				if (subFlowMachine != null)
				{
					subFlowMachine.StartListening();
				}
			}

			base.OnEnable();

			SubFlowTriggerEvent(EventHooks.OnEnable);
		}

        protected override void Update()
        {
            base.Update();

			SubFlowTriggerEvent(EventHooks.Update);
		}

        protected override void OnInstantiateWhileEnabled()
		{
			if (hasGraph)
			{
				graph.StartListening(reference);
			}

			base.OnInstantiateWhileEnabled();
		}

		protected override void OnUninstantiateWhileEnabled()
		{
			base.OnUninstantiateWhileEnabled();

			if (hasGraph)
			{
				graph.StopListening(reference);
			}
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			if (hasGraph)
			{
				graph.StopListening(reference);
			}
		}

		[ContextMenu("Show Data...")]
		protected override void ShowData()
		{
			base.ShowData();
		}
	}
}
