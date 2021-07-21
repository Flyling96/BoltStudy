using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ludiq;
using DragonSlay;

namespace Bolt.Extend
{
    public class SceneObjectFuncUnit<T>: Unit where T:ISceneObjectFunc
    {
        [DoNotSerialize]
        [PortLabelHidden]
        public ControlInput enter { get; private set; }

        [DoNotSerialize]
        [PortLabelHidden]
        public ControlOutput exit { get; private set; }

        [DoNotSerialize]
        public ValueInput @object { get; private set; } 

        protected override void Definition()
        {
            @object = ValueInput<SceneObject>(nameof(@object));
            enter = ControlInput(nameof(enter), Enter);
            exit = ControlOutput(nameof(exit));
        }

        public bool ExecuteFunc(Flow flow)
        {
            T funcObject = (T)flow.GetValue(@object);

            if (funcObject == null)
            {
                return false;
            }

            ExecuteFunc(funcObject,flow);

            return true;
        }

        public virtual void ExecuteFunc(T funcObject,Flow flow)
        {

        }

        private ControlOutput Enter(Flow flow)
        {
            if (ExecuteFunc(flow))
            {
                return exit;
            }
            else
            {
                return null;
            }
        }

    }
}
