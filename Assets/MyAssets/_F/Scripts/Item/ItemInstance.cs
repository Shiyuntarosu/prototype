using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ItemInstance
{
    [SerializeField] public ItemData data;

    [SerializeField] public List<ItemFeature> features = new();

    public ItemInstance(ItemData _data)
    {
        data = _data;
    }

    public T GetFeature<T>() where T : ItemFeature
    {
        return features.OfType<T>().FirstOrDefault();
    }

    public bool TryGetFeature<T>(out T feature) where T : ItemFeature
    {
        feature = features.OfType<T>().FirstOrDefault();
        return feature != null;
    }

    // HasFeature<T>()

    public bool CanStackWith(ItemInstance other)
    {
        if (other == null) return false;
        return data == other.data;
    }
}
