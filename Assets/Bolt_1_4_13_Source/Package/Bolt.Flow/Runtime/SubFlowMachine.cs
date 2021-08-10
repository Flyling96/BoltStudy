using Ludiq;
using UnityEngine;

namespace Bolt.Extend
{
    [AddComponentMenu("Bolt/Sub Flow Machine")]
    //[RequireComponent(typeof(Variables))]
    public class SubFlowMachine : Machine<FlowGraph, FlowMacro>
    {
		public override FlowGraph DefaultGraph()
		{
			return FlowGraph.WithStartUpdate();
		}

		private FlowMachine m_RootMachine;

		public FlowMachine RootMachine
        {
            get
            {
                if(m_RootMachine != null)
                {
                    return m_RootMachine;
                }

                var parent = transform;
                while (parent != null)
                {
                    m_RootMachine = parent.GetComponent<FlowMachine>();
                    if (m_RootMachine != null)
                    {
                        return m_RootMachine;
                    }
                    parent = parent.parent;
                }
                return null;
            }
        }

        public void StartListening()
        {
            graph.StartListening(reference);
        }

        public void TriggerEvent(string name)
        {
            if (hasGraph)
            {
                TriggerRegisteredEvent(new EventHook(name,this), new EmptyEventArgs());
            }
        }

        private void TriggerRegisteredEvent<TArgs>(EventHook hook, TArgs args)
        {
            EventBus.Trigger(hook, args);
        }


    }
}
