using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PrefabSerialization : MonoBehaviour,ISerializationCallbackReceiver
{
    public string value = "_json";

    [Serializable]
    public struct SerializationData
    {
        [SerializeField]
        private string _json;

        public SerializationData(string json)
        {
            _json = json;
        }
    }

    [SerializeField]
    protected SerializationData _data;

    public void OnBeforeSerialize()
    {
        _data = new SerializationData(value);
    }

    public void OnAfterDeserialize()
    {

    }
}
