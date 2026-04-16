using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class FishDatabase : ScriptableObject
{
    public List<FishEntry> items;

    private Dictionary<string, FishEntry> lookup;

    public void Init()
    {
        lookup = items.ToDictionary(i => i.id, i => i);
    }

    public FishEntry Get(string id)
    {
        return lookup[id];
    }
}

[System.Serializable]
public class FishEntry
{
    public string id;
    public int value = 10;
    public SpawnSeason season;
    public GameObject prefab;
}

public enum SpawnSeason
{
    ALL,
    SPRING,
    SUMMER,
    AUTUMN,
    WINTER
}
