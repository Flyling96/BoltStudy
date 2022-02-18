using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Ludiq
{
    public class BinaryTypeSerializer : BinarySerializer
    {
        public override bool CanProcess(Type type)
        {
            return typeof(Type).IsAssignableFrom(type);
        }

        public override void Deserialize(BinaryReader reader, ref object @object, Type type)
        {
            @object = type;
        }

        public override void Serialize(BinaryWriter writer, object @object, Type type)
        {

        }
    }
}
