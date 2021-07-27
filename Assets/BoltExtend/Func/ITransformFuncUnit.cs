using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ludiq;
using DragonSlay;

namespace Bolt.Extend
{
    [UnitCategory("Extend")]
    [UnitOrder(0)]
    [CustomRutimeType]
    public class ITransformFuncUnit:SceneObjectFuncUnit<ITransform>
    {
        [DoNotSerialize]
        public ValueInput pos { get; private set; }
        [DoNotSerialize]
        public ValueInput rot { get; private set; }

        protected override void Definition()
        {
            base.Definition();

            pos = ValueInput<Vector3>(nameof(pos));

            rot = ValueInput<Vector3>(nameof(rot),Vector3.zero);
        }

        public override void ExecuteFunc(ITransform funcObject, Flow flow)
        {
            base.ExecuteFunc(funcObject, flow);
            Vector3 pos = flow.GetValue<Vector3>(this.pos);
            Vector3 elur = flow.GetValue<Vector3>(this.rot);
            Quaternion rot = Quaternion.Euler(elur);
            funcObject.Transform(pos, rot);
        }


    }
}
