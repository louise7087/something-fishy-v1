using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class RodDatabase : ScriptableObject
{
    public List<RodEntry> items;

    private Dictionary<string, RodEntry> lookup;

    public void Init()
    {
        lookup = items.ToDictionary(i => i.id, i => i);
    }

    public RodEntry Get(string id)
    {
        return lookup[id];
    }
}

[System.Serializable]
public class RodEntry : ItemEntry
{
    public int strength;
}