using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ludiq;

namespace Bolt.Extend
{
    [UnitCategory("Extend")]
    [CustomRutimeType]
    public class SubFlowUnit : SpecifyVariableUnit, ISubFlowUnit
    {
        [DoNotSerialize]
        [PortLabelHidden]
        [NullMeansSelf]
        public ValueInput target { get; private set; }

        [DoNotSerialize]
        [PortLabelHidden]
        public ValueOutput value { get; private set; }

        protected override void Definition()
        {
            base.Definition();

            value = ValueOutput(nameof(value), Get);

            target = ValueInput<GameObject>(nameof(target), null).NullMeansSelf();
        }

        private GameObject Get(Flow flow)
        {
            GameObject go = flow.GetValue(target) as GameObject;

            VariableDeclarations variables = Variables.AutoSubFlow(go);
            string nameStr = (string)flow.GetValue(name);
            var subFlow = variables.Get(nameStr) as FlowMachine;

            if(subFlow != null)
            {
                return subFlow.gameObject;
            }
            else
            {
                return null;
            }
        }
    }
}
