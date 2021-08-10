using Ludiq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bolt.Extend
{
    public abstract class SpecifyVariableUnit : Unit
    {
        [Serialize]
        public string VariableName { get; set; }

    }
}
