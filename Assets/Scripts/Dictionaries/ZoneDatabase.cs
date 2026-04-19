using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class ZoneDatabase : ScriptableObject
{
    public List<ZoneEntry> items;

    private Dictionary<string, ZoneEntry> lookup;

    public void Init()
    {
        lookup = items.ToDictionary(i => i.id, i => i);
    }

    public ZoneEntry GetZoneByName(string zone)
    {
        return lookup[zone];
    }
}

[System.Serializable]
public class ZoneEntry
{
    public string id;
    public int cost;
    public GameObject prefab;
}
