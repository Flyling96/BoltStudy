using Ludiq;
using System;
using System.Collections;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Reflection;

namespace Bolt.Extend 
 {
    public class StructClass<T> where T : struct
    {
        public StructClass(T value, Action<T> cb)
        {
            m_Value = value;
            m_VauleChangeCB = cb;
        }

        private T m_Value;

        private Action<T> m_VauleChangeCB;

        public T Value
        {
            get
            {
                return m_Value;
            }
            set
            {
                m_Value = value;
                m_VauleChangeCB?.Invoke(value);
            }
        }
    }

    public class Vector3Class : StructClass<Vector3> 
    {
        public Vector3Class(Vector3 value, Action<Vector3> cb) : base(value, cb) { }
    }

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

        [Serialize, Inspectable]
        public Vector3 m_Value
        {
            get;
            set;
        }

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
            Vector3Get =  ValueOutput<Vector3Class>(nameof(Vector3Class), GetVector3);
            Vector3SetX = ValueInput<float>("x",0f);
            Vector3SetY = ValueInput<float>("y",0f);
            Vector3SetZ = ValueInput<float>("z",0f);
        }
        private Vector3Class GetVector3(Flow flow) {
            //object target = m_Value;
            return new Vector3Class(m_Value,(value)=> { m_Value = value; });
        }
    }

    [UnitCategory("Extend/Vector3Unit")]
    [UnitSurtitle("Set X")]
    [CustomRutimeType]
    public class Vector3XSet : Unit
    {
        [DoNotSerialize]
        [PortLabelHidden]
        public ControlInput Enter { get; set; }

        [DoNotSerialize]
        [PortLabelHidden]
        public ControlOutput Exit { get; set; }

        [DoNotSerialize]
        [PortLabelHidden]
        public ValueOutput XOutput
        {
            get; private set;
        }
        [DoNotSerialize]
        [PortLabelHidden]
        public ValueInput Vector3Input
        {
            private get; set;
        }

        [DoNotSerialize]
        [PortLabelHidden]
        public ValueInput XInput
        {
            private get; set;
        }

        protected override void Definition()
        {
            Vector3Input = ValueInput<Vector3Class>("");
            XInput = ValueInput<float>(nameof(XInput));
            XOutput = ValueOutput<float>(nameof(XOutput), GetVector3X);
            Enter = ControlInput(nameof(Enter), EnterAction);
            Exit = ControlOutput(nameof(Exit));
        }
        private float GetVector3X(Flow flow)
        {
            return flow.GetValue<float>(XInput);
        }

        ControlOutput EnterAction(Flow flow)
        {
            Vector3Class target = flow.GetValue(Vector3Input) as Vector3Class;
            var x = (float)flow.GetValue(XInput);
            target.Value = new Vector3(x, target.Value.y, target.Value.z);

            //object target = flow.GetValue(Vector3Input);
            //int size = Marshal.SizeOf(target);
            //IntPtr intPtr = Marshal.AllocHGlobal(size);
            //Marshal.StructureToPtr(target, intPtr, true);

            ////FieldInfo field = typeof(Vector3).GetField("x");
            //var x = (float)flow.GetValue(XInput);
            ////field.SetValue(target, x);

            //unsafe
            //{
            //    Vector3* ptr = (Vector3*)intPtr;
            //    ptr->x = x;
            //}

            //Marshal.FreeHGlobal(intPtr);

            return Exit;
        }
    }
}

