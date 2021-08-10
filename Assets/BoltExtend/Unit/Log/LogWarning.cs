using Ludiq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Bolt.Extend {
    public class LogWarning : Unit {

        [DoNotSerialize]
        public ControlInput Update {
            get; private set;
        }

        [DoNotSerialize]
        public ValueInput MinNum {
            set; private get;
        }
        [DoNotSerialize]
        public ValueInput MaxNum {
            set; private get;
        }
        [DoNotSerialize]
        public ValueOutput RandomFloat {
            private set; get;
        }
        protected override void Definition() {
          
        }
    }
}

