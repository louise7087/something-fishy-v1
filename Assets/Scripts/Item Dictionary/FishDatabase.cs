using NUnit.Framework;
using System;
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

    public FishEntry GetRandomFishFromSeason(Season season)
    {
        var possibleFish = items.Where(i => (int)i.season == (int)season || i.season == SpawnSeason.ALL).ToList(); // Gets all fish where seasons match

        int randomIndex = UnityEngine.Random.Range(0, possibleFish.Count());

        return possibleFish[randomIndex];
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
    SPRING,
    SUMMER,
    AUTUMN,
    WINTER,
    ALL
}
