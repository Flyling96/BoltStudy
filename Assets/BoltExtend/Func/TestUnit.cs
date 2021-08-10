using Ludiq;
using UnityEngine;

namespace Bolt
{
    public class TestUnit : Unit
    {
        [DoNotSerialize]
        [PortLabelHidden]
        public ControlInput Enter { get; private set; }

        [DoNotSerialize]
        [PortLabelHidden]
        public ControlOutput Exit { get; private set; }

        protected override void Definition()
        {
            Enter = ControlInput(nameof(Enter), OnEnter);
            Exit = ControlOutput(nameof(Exit));
        }

        private ControlOutput OnEnter(Flow flow)
        {
            Debug.Log($"[_TestUnit.OnEnter] {{{this}({GetType()}): {GetHashCode()}}}, {{{graph}({graph.GetType()}): {graph.GetHashCode()}}}");
            return Exit;
        }

    }
}
