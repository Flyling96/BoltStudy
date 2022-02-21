using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Ludiq
{
    public class BinaryArrayListSerializer : BinarySerializer
    {
        public override bool CanProcess(Type type)
        {
            return typeof(ArrayList).IsAssignableFrom(type);
        }

        public override void Deserialize(BinaryReader reader, ref object @object, Type type)
        {
            int count = reader.ReadInt32();
            @object = Activator.CreateInstance(type);
            for (int i = 0; i < count; i++)
            {
                object element = null;
                BinaryManager.Instance.DeserializeObject(reader, ref element);
                ((ArrayList)@object).Add(element);
            }

        }

        public override void Serialize(BinaryWriter writer, object @object, Type type)
        {
            var array = (IList)@object;
            writer.Write(array.Count);
            for (int i = 0; i < array.Count; i++)
            {
                BinaryManager.Instance.SerializeObject(writer, array[i]);
            }
        }
    }
}
