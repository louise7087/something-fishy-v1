using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class ItemDatabase : ScriptableObject
{
    public List<ItemEntry> items;

    private Dictionary<string, ItemEntry> lookup;

    public void Init()
    {
        lookup = items.ToDictionary(i => i.id, i => i);
    }

    public ItemEntry Get(string id)
    {
        return lookup[id];
    }
}

[CreateAssetMenu]
public class ItemEntry : ScriptableObject
{
    public string id;
    public GameObject prefab;
}
