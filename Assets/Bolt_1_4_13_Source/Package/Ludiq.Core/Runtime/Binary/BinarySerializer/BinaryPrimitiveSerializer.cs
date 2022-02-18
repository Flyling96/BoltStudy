using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Ludiq
{
    public class BinaryPrimitiveSerializer : BinarySerializer
    {
        public override bool CanProcess(Type type)
        {
            return type.IsPrimitive || type == typeof(string) || type == typeof(Vector2) || type == typeof(Vector3);
        }

        public override void Deserialize(BinaryReader reader, ref object @object, Type type)
        {
            if (type == typeof(Int16))
            {
                @object = reader.ReadInt16();
            }
            else if (type == typeof(Int32))
            {
                @object = reader.ReadInt32();
            }
            else if (type == typeof(Int64))
            {
                @object = reader.ReadInt64();
            }
            else if (type == typeof(UInt16))
            {
                @object = reader.ReadUInt16();
            }
            else if (type == typeof(UInt32))
            {
                @object = reader.ReadUInt32();
            }
            else if (type == typeof(UInt64))
            {
                @object = reader.ReadUInt64();
            }
            else if (type == typeof(Single))
            {
                @object = reader.ReadSingle();
            }
            else if (type == typeof(Double))
            {
                @object = reader.ReadDouble();
            }
            else if (type == typeof(String))
            {
                @object = reader.ReadString();
            }
            else if (type == typeof(Boolean))
            {
                @object = reader.ReadBoolean();
            }
            else if (type == typeof(Vector3))
            {
                @object = reader.ReadVector3();
            }
            else if (type == typeof(Vector2))
            {
                @object = reader.ReadVector2();
            }
            else
            {
                Debug.LogError($"BinaryPrimitiveSerializer Deserialize type error {type}");
            }
        }

        public override void Serialize(BinaryWriter writer, object @object, Type type)
        {
            if(@object == null)
            {
                Debug.LogError("BinaryPrimitiveSerializer @object is null");
                return;
            }

            if (type == typeof(Int16))
            {
                writer.Write((Int16)@object);
            }
            else if (type == typeof(Int32))
            {
                writer.Write((Int32)@object);
            }
            else if (type == typeof(Int64))
            {
                writer.Write((Int64)@object);
            }
            else if (type == typeof(UInt16))
            {
                writer.Write((UInt16)@object);
            }
            else if (type == typeof(UInt32))
            {
                writer.Write((UInt32)@object);
            }
            else if (type == typeof(UInt64))
            {
                writer.Write((UInt64)@object);
            }
            else if (type == typeof(Single))
            {
                writer.Write((Single)@object);
            }
            else if (type == typeof(Double))
            {
                writer.Write((Double)@object);
            }
            else if (type == typeof(String))
            {
                writer.Write((String)@object);
            }
            else if (type == typeof(Boolean))
            {
                writer.Write((Boolean)@object);
            }
            else if (type == typeof(Vector3))
            {
                writer.Write((Vector3)@object);
            }
            else if (type == typeof(Vector2))
            {
                writer.Write((Vector2)@object);
            }
            else
            {
                Debug.LogError($"BinaryPrimitiveSerializer Serialize type error {type}");
            }


        }
    }
}
