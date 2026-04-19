using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class ZoneDatabase : ScriptableObject
{
    public List<ZoneEntry> items;

    private Dictionary<int, ZoneEntry> lookup;

    public void Init()
    {
        lookup = items.ToDictionary(i => i.zone, i => i);
    }

    public ZoneEntry GetZoneById(int zone)
    {
        return lookup[zone];
    }
}

[System.Serializable]
public class ZoneEntry
{
    public string name;
    public int zone;
    public int cost;
    public GameObject prefab;
}
