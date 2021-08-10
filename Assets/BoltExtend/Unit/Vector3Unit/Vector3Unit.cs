using Ludiq;
using System;
using System.Collections;
using UnityEngine;

namespace Bolt.Extend 
 {
    [UnitCategory("Extend/Vector3Unit")]
    [UnitSurtitle("X")]
    [CustomRutimeType]
    public class Vector3X : Unit {
        [DoNotSerialize]
        [PortLabelHidden]
        public ValueOutput Vector3XGet {
             get;private set;
        }
        [DoNotSerialize]
        [PortLabelHidden]
        public ValueInput Vector3Set {
            private get; set;
        }
        protected override void Definition() {
            Vector3Set = ValueInput<Vector3>(nameof(Vector3));
            Vector3XGet = ValueOutput<float>("x", GetVector3X);
        }
        private float GetVector3X(Flow flow)
        {
            Vector3 vector3Get = flow.GetValue<Vector3>(Vector3Set);
            return vector3Get.x;
        }
    }
    [UnitCategory("Extend/Vector3Unit")]
    [UnitSurtitle("Y")]
    [CustomRutimeType]
    public class Vector3Y : Unit {
        [DoNotSerialize]
        [PortLabelHidden]
        public ValueOutput Vector3XGet {
             get; private set;
        }
        [DoNotSerialize]
        [PortLabelHidden]
        public ValueInput Vector3Set {
            private get;  set;
        }
        protected override void Definition() {
            Vector3Set = ValueInput<Vector3>(nameof(Vector3));
            Vector3XGet = ValueOutput<float>("y", GetVector3Y);
        }
        private float GetVector3Y(Flow flow) {
            Vector3 vector3Get = flow.GetValue<Vector3>(Vector3Set);
            return vector3Get.y;
        }
    }
    [UnitCategory("Extend/Vector3Unit")]
    [UnitSurtitle("Z")]
    [CustomRutimeType]
    public class Vector3Z : Unit {
        [DoNotSerialize]
        [PortLabelHidden]
        public ValueOutput Vector3ZGet {
            get; private set;
        }
        [DoNotSerialize]
        [PortLabelHidden]
        public ValueInput Vector3Set {
            get; private set;
        }
        protected override void Definition() {
            Vector3Set = ValueInput<Vector3>(nameof(Vector3));
            Vector3ZGet = ValueOutput<float>("z", GetVector3Z);

        }
        private float GetVector3Z(Flow flow) {
            Vector3 vector3Get = flow.GetValue<Vector3>(Vector3Set);
            return vector3Get.z;
        }
    }
    [UnitCategory("Extend/Vector3Unit")]
    [CustomRutimeType]
    public class Vector3New : Unit {
        [DoNotSerialize]
        [PortLabelHidden]
        public ValueInput Vector3SetX {
           private get; set;
        }
        [DoNotSerialize]
        [PortLabelHidden]
        public ValueInput Vector3SetY {
            private get; set;
        }
        [DoNotSerialize]
        [PortLabelHidden]
        public ValueInput Vector3SetZ {
            private get; set;
        }

        [DoNotSerialize]
        [PortLabelHidden]
        public ValueOutput Vector3Get {
            get; private set;
        }
        protected override void Definition() {
            Vector3Get =  ValueOutput<Vector3>(nameof(Vector3), GetVector3);
            Vector3SetX = ValueInput<float>("x",0f);
            Vector3SetY = ValueInput<float>("y",0f);
            Vector3SetZ = ValueInput<float>("z",0f);
        }
        private Vector3 GetVector3(Flow flow) {
            return new Vector3(flow.GetValue<float>(Vector3SetX), flow.GetValue<float>(Vector3SetY), flow.GetValue<float>(Vector3SetZ));
        }
    }
}

