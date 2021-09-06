using Ludiq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bolt.Extend
{
    public abstract class SpecifyVariableUnit : Unit
    {
        public SpecifyVariableUnit(string name) : base()
        {
            variableName = name;
        }

        public SpecifyVariableUnit() : base() { }

        [Serialize]
        public string variableName { get; set; }

        [DoNotSerialize]
        [PortLabelHidden]
        public ValueInput name { get; private set; }

        protected override void Definition()
        {
            name = ValueInput(nameof(name), variableName);
        }
    }
}
