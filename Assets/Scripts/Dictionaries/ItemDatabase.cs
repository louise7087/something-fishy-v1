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

[System.Serializable]
public class ItemEntry
{
    public string name;
    public string id;
    public int value = 10;
    public int maxStack = 1;
    public GameObject prefab;
}
