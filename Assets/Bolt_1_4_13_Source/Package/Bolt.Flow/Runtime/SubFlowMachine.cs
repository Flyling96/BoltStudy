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
            if(!_alive)
            {
                Awake();
            }
            graph.StartListening(reference);
        }

        public void TriggerEvent(string name)
        {
            if (hasGraph)
            {
                TriggerRegisteredEvent(new EventHook(name,this), new EmptyEventArgs());
            }
        }

        private Flow m_ParentFlow = null;

        public void GraphControlInput(Flow parentFlow,string key)
        {
            m_ParentFlow = parentFlow;

            Flow flow =  Flow.New(reference);

            flow.RestoreStack(m_ParentFlow.stack);

            flow.stack.EnterRoot(this);

            ControlOutput nextPort = null; 

            foreach (var unit in graph.units)
            {
                if (unit is GraphInput)
                {
                    var inputUnit = (GraphInput)unit;

                    nextPort = inputUnit.controlOutputs[key];

                    break;
                }
            }

            if(nextPort != null)
            {
                flow.Run(nextPort);
            }
        }


        private void TriggerRegisteredEvent<TArgs>(EventHook hook, TArgs args)
        {
            EventBus.Trigger(hook, args);
        }


    }
}
