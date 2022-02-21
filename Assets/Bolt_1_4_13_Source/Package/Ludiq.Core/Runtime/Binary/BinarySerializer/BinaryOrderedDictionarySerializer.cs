using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using UnityEngine;

namespace Ludiq
{
    public class BinaryOrderedDictionarySerializer : BinarySerializer
    {
        public override bool CanProcess(Type type)
        {
            return typeof(OrderedDictionary).IsAssignableFrom(type);
        }

        public override void Deserialize(BinaryReader reader, ref object @object, Type type)
        {
            var count = reader.ReadInt32();
            @object = Activator.CreateInstance(type);
            var dic = (IDictionary)@object;

            for (int i = 0; i < count; i++)
            {
                object key = null;
                BinaryManager.Instance.DeserializeObject(reader, ref key);

                object value = null;
                BinaryManager.Instance.DeserializeObject(reader, ref value);

                dic.Add(key, value);
            }

        }

        public override void Serialize(BinaryWriter writer, object @object, Type type)
        {
            var dic = (IDictionary)@object;
            writer.Write(dic.Count);
            var enumerator = dic.GetEnumerator();

            while (enumerator.MoveNext())
            {
                var key = enumerator.Key;
                var value = enumerator.Value;
                BinaryManager.Instance.SerializeObject(writer, key);
                BinaryManager.Instance.SerializeObject(writer, value);
            }

        }
    }
}
