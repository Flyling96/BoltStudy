using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Ludiq
{
    public class BinaryArraySerializer : BinarySerializer
    {
        public override bool CanProcess(Type type)
        {
            return type.IsArray;
        }

        public override void Deserialize(BinaryReader reader, object @object, Type type)
        {
            int count = reader.ReadInt32();
            var elementType = type.GetElementType();
            var list = new ArrayList();
            for(int i = 0;i < count;i++)
            {
                object element = null;
                BinaryManager.Instance.DeserializeObject(reader,element, elementType);
                list.Add(element);
            }
            @object = list.ToArray(elementType);

        }

        public override void Serialize(BinaryWriter writer, object @object, Type type)
        {
            var array = (IList)@object;
            writer.Write(array.Count);
            var elementType = type.GetElementType();
            for (int i =0; i < array.Count;i++)
            {
                BinaryManager.Instance.SerializeObject(writer, array[i], elementType);
            }
        }
    }
}
