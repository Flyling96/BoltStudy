using Ludiq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;

public class BinaryUnityObjectSerializer : BinarySerializer
{
    public override bool CanProcess(Type type)
    {
        return typeof(Object).IsAssignableFrom(type);
    }

    public override void Deserialize(BinaryReader reader, ref object @object, Type type)
    {
        if (BinaryManager.Instance.m_UnityObjectReferences != null)
        {
            int index = reader.ReadInt32();
            if (index > -1 && index < BinaryManager.Instance.m_UnityObjectReferences.Count)
            {
                @object = BinaryManager.Instance.m_UnityObjectReferences[index];
            }
        }
        else
        {
            @object = null;
        }


    }

    public override void Serialize(BinaryWriter writer, object @object, Type type)
    {
        if(BinaryManager.Instance.m_UnityObjectReferences != null)
        {
            writer.Write(BinaryManager.Instance.m_UnityObjectReferences.Count);
            BinaryManager.Instance.m_UnityObjectReferences.Add(@object as Object);
        }
    }
}
