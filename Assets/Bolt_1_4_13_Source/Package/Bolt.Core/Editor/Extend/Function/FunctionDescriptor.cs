using Ludiq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bolt.Extend
{
    [Descriptor(typeof(IGraphFunctionElement))]
    public class FunctionDescriptor : IDescriptor
    {
        public object target { get; }

        public IDescription description { get; private set; }

        public bool isDirty { get; set; } = true;

        public void Validate()
        {
            if (isDirty)
            {
                isDirty = false;
            }
        }
    }
}
