using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Ludiq
{
    public class BinaryDictionarySerializer : BinarySerializer
    {
        public override bool CanProcess(Type type)
        {
            if (type.IsGenericType)
            {
                return typeof(Dictionary<,>).IsAssignableFrom(type.GetGenericTypeDefinition());
            }

            return false;
        }

        public override void Deserialize(BinaryReader reader, ref object @object, Type type)
        {
            var count = reader.ReadInt32();
            var keyType = type.GetGenericArguments()[0];
            var valueType = type.GetGenericArguments()[1];

            @object = Activator.CreateInstance(type);
            var dic = (IDictionary)@object;

            for (int i =0; i < count;i++)
            {
                object key = null;
                BinaryManager.Instance.DeserializeObject(reader, ref key, keyType);

                object value = null;
                BinaryManager.Instance.DeserializeObject(reader, ref value, valueType);

                dic.Add(key, value);
            }
            
        }

        public override void Serialize(BinaryWriter writer, object @object, Type type)
        {
            var dic = (IDictionary)@object;
            writer.Write(dic.Count);
            var enumerator = dic.GetEnumerator();
            var keyType = type.GetGenericArguments()[0];
            var valueType = type.GetGenericArguments()[1];

            while (enumerator.MoveNext())
            {
                var key = enumerator.Key;
                var value = enumerator.Value;
                BinaryManager.Instance.SerializeObject(writer, key, keyType);
                BinaryManager.Instance.SerializeObject(writer, value, valueType);
            }

        }
    }
}
