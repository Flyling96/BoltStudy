using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Ludiq
{
    public class BinaryManager
    {
        private static BinaryManager m_Instance = null;

        public static BinaryManager Instance
        {
            get
            {
                if(m_Instance == null)
                {
                    m_Instance = new BinaryManager();
                }

                return m_Instance;
            }
        }

        private const string m_ObjectNullStr = "Null";
        public List<UnityEngine.Object> m_UnityObjectReferences = null;

        public void SerializeObject(BinaryWriter writer,object @object,List<UnityEngine.Object> references = null)
        {
            if(@object == null)
            {
                writer.Write(m_ObjectNullStr);
                return;
            }
            Type type = @object.GetType();
            string typeString = RuntimeCodebase.SerializeType(type);
            writer.Write(typeString);
            if(references != null)
            {
                m_UnityObjectReferences = references;
            }
            SerializeObject(writer, @object, type);

            if (references != null)
            {
                m_UnityObjectReferences = null;
            }
        }

        public void DeserializeObject(BinaryReader reader, ref object @object, List<UnityEngine.Object> references = null)
        {
            string typeString = reader.ReadString();
            if(typeString == m_ObjectNullStr)
            {
                return;
            }

            if(RuntimeCodebase.TryDeserializeType(typeString,out var type))
            {
                if (references != null)
                {
                    m_UnityObjectReferences = references;
                }

                DeserializeObject(reader, ref @object,type);

                if (references != null)
                {
                    m_UnityObjectReferences = null;
                }

            }
            else
            {
                Debug.LogError($"BinaryManager DeserializeType Error typeString : {typeString}");
            }
        }


        public void SerializeObject(BinaryWriter writer, object @object, Type type)
        {
            var serializer = GetSerializer(type);
            if(serializer != null)
            {
                serializer.Serialize(writer, @object, type);
            }
            else
            {
                Debug.LogError($"BinaryManager can't find seriilaizer type {type}");
            }
        }

        public void DeserializeObject(BinaryReader reader, ref object @object,Type type)
        {
            var serializer = GetSerializer(type);
            if (serializer != null)
            {
                serializer.Deserialize(reader, ref @object, type);
            }
        }

        private BinarySerializer GetSerializer(Type type)
        {
            foreach(var serializer in m_BinarySerializers)
            {
                if(serializer.CanProcess(type))
                {
                    return serializer;
                }
            }

            return null;
        }

        private BinarySerializer[] m_BinarySerializers = new BinarySerializer[]
        {
            new BinaryPrimitiveSerializer(),
            new BinaryEnumSerializer(),
            new BinaryTypeSerializer(),
            new BinaryArraySerializer(),
            new BinaryArrayListSerializer(),
            new BinaryListSerializer(),
            new BinaryOrderedDictionarySerializer(),
            new BinaryDictionarySerializer(),
            new BinaryCustomTypeSerializer(),
            new BinaryUnityObjectSerializer(),
        };

    }
}
