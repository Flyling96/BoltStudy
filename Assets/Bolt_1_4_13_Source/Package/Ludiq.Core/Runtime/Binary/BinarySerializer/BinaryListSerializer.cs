using Ludiq.FullSerializer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Ludiq
{
    public class BinaryListSerializer : BinarySerializer
    {
        public override bool CanProcess(Type type)
        {
            if (type.IsGenericType)
            {
                return typeof(List<>).IsAssignableFrom(type.GetGenericTypeDefinition());
            }

            return false;
        }

        public override void Deserialize(BinaryReader reader, ref object @object, Type type)
        {
            int count = reader.ReadInt32();
            var elementType = type.GetGenericArguments()[0];
            @object = Activator.CreateInstance(type);
            var list = (IList)@object;
            for (int i = 0; i < count; i++)
            {
                object element = null;
                BinaryManager.Instance.DeserializeObject(reader, ref element, elementType);
                list.Add(element);
            }
        }

        public override void Serialize(BinaryWriter writer, object @object, Type type)
        {
            var array = (IList)@object;
            writer.Write(array.Count);
            var elementType = type.GetGenericArguments()[0];
            for (int i = 0; i < array.Count; i++)
            {
                BinaryManager.Instance.SerializeObject(writer, array[i], elementType);
            }
        }

    }
}
