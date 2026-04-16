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

[CreateAssetMenu]
public class FishEntry : ScriptableObject
{
    public string id;
    public Season season;
    public GameObject prefab;
}

public enum Season
{
    SPRING,
    SUMMER,
    AUTUMN,
    WINTER
}
