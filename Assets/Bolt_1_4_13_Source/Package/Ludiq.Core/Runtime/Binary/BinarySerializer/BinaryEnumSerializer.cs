using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Ludiq
{
    public class BinaryEnumSerializer : BinarySerializer
    {
        public override bool CanProcess(Type type)
        {
            return type.IsEnum;
        }

        public override void Deserialize(BinaryReader reader, ref object @object, Type type)
        {
            if (type.IsEnum)
            {
                var enumIndex = reader.ReadInt32();
                @object = Convert.ChangeType(enumIndex, type);
            }
            else
            {
                Debug.LogError($"BinaryEnumSerializer deserialize fail. type : {type}");
            }
        }

        public override void Serialize(BinaryWriter writer, object @object, Type type)
        {
            writer.Write((int)@object);
        }
    }
}
