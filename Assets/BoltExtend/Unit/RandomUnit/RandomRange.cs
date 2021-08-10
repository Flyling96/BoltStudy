using Ludiq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bolt.Extend {
    [UnitCategory("Extend/Random")]
    [CustomRutimeType]
    public class RandomRange : Unit {
        [DoNotSerialize]
        public ValueInput MinNum{
            set;private get;
        }
        [DoNotSerialize]
        public ValueInput MaxNum {
            set;private get;
        }
        [DoNotSerialize]
        public ValueOutput RandomFloat {
           private set;get;
        }
        protected override void Definition() {
            MinNum = ValueInput<float>("Min",0f);
            MaxNum = ValueInput<float>("Max", 0f);
            RandomFloat = ValueOutput<float>("",GetRandom);
        }
        private float GetRandom(Flow flow) {
            float randomf = Random.Range(flow.GetValue<float>(MinNum), flow.GetValue<float>(MaxNum));
            return randomf;
        }
    }
}

