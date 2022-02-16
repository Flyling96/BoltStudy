using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Ludiq
{
    public static class BinaryExtension
    {
        public static Vector3 ReadVector3(this BinaryReader reader)
        {
            Vector3 res;
            res.x = reader.ReadSingle();
            res.y = reader.ReadSingle();
            res.z = reader.ReadSingle();
            return res;
        }

        public static Vector2 ReadVector2(this BinaryReader reader)
        {
            Vector2 res;
            res.x = reader.ReadSingle();
            res.y = reader.ReadSingle();
            return res;
        }

        public static void Write(this BinaryWriter writer, Vector3 target)
        {
            writer.Write(target.x);
            writer.Write(target.y);
            writer.Write(target.z);
        }

        public static void Write(this BinaryWriter writer, Vector2 target)
        {
            writer.Write(target.x);
            writer.Write(target.y);
        }
    }
}
