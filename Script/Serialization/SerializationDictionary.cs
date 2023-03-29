using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializationDictionary<TKey, TValue> : ISerializationCallbackReceiver
{
    [SerializeField]
    private List<TKey> keys;
    [SerializeField]
    private List<TValue> values;

    private Dictionary<TKey, TValue> target;
    public Dictionary<TKey, TValue> ToDictionary() { return target; }

    public SerializationDictionary(Dictionary<TKey, TValue> target)
    {
        this.target = target;
    }

    public SerializationDictionary()
    {
        this.target = new Dictionary<TKey, TValue>();
    }

    public void OnBeforeSerialize()
    {
        keys = new List<TKey>(target.Keys);
        values = new List<TValue>(target.Values);
    }

    public void OnAfterDeserialize()
    {
        var count = Math.Min(keys.Count, values.Count);
        target = new Dictionary<TKey, TValue>(count);
        for (var i = 0; i < count; ++i)
        {
            target.Add(keys[i], values[i]);
        }
    }
}