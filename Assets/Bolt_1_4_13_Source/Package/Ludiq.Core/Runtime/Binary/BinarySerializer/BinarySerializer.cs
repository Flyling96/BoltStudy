using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Ludiq
{
    public abstract class BinarySerializer
    {
        public abstract bool CanProcess(Type type);

        public abstract void Serialize(BinaryWriter writer,object @object, Type type);

        public abstract void Deserialize(BinaryReader reader,ref object @object, Type type);
    }
}
