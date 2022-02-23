using System;
using UnityEngine;

public static class ComponentExtensions
{
    public static T GetOrAddComponent<T>(this Component tr)where T : Component
    {
        return tr.GetComponentOnly<T>() ?? tr.gameObject.AddComponent<T>();
    }

    public static T GetComponentOnly<T>(this Component t) where T : Component
    {
        T result = t.GetComponent<T>();
        if (result == null) return default(T);
        return result;
    }
}
