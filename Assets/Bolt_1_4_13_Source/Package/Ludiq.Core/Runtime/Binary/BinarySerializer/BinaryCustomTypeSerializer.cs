using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Ludiq
{
    public class BinaryCustomTypeSerializer : BinarySerializer
    {
        public override bool CanProcess(Type type)
        {
            return type.IsDefined(typeof(CustomBinaryAttribute), false);
        }

        public override void Deserialize(BinaryReader reader, ref object @object, Type type)
        {
            @object = Activator.CreateInstance(type);
            var method = type.GetMethod("BinaryDeserialize");
            if(method != null)
            {
                method.Invoke(@object, new object[] { reader});
            }
        }

        public override void Serialize(BinaryWriter writer, object @object, Type type)
        {
            var method = type.GetMethod("BinarySerialize");
            if(method != null)
            {
                method.Invoke(@object, new object[] { writer });
            }
        }
    }
}
