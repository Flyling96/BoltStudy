using Ludiq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonSlay;

namespace Bolt.Extend
{
    [UnitCategory("Extend")]
    [CustomRutimeType]
    public class SceneObjectUnit : SpecifyVariableUnit
    {
        [DoNotSerialize]
        [PortLabelHidden]
        public ValueOutput value { get; private set; }

        protected override void Definition()
        {
            value = ValueOutput(nameof(value), Get);
        }

		private object Get(Flow flow)
		{
			var name = m_VariableName;

			VariableDeclarations variables = Variables.AutoSceneObject(flow.stack.self);

            var shell = variables.Get(name) as SceneObjectDataShell;

            return shell.SceneObject;
		}
	}
}
