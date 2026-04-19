using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ZoneManager : MonoBehaviour
{
    [SerializeField] private ZoneDatabase zoneDatabase;

    private List<int> unlockedZoneIds = new List<int>();

    private Dictionary<int, GameObject> lockedZonesGameObjects = new Dictionary<int, GameObject>();

    private void Start()
    {
        zoneDatabase.Init();

        // Spawn all zones in for now
        foreach(ZoneEntry zoneEntry in zoneDatabase.items)
        {
            lockedZonesGameObjects.Add(zoneEntry.zone, Instantiate(zoneEntry.prefab));
        }

        Debug.Log("Spawned all zones");
    }

    public void UnlockZone(int zoneId)
    {
        Destroy(lockedZonesGameObjects[zoneId]);
        lockedZonesGameObjects.Remove(zoneId);
        unlockedZoneIds.Add(zoneId);

        Debug.Log($"Unlocked zone {zoneId}");
    }

    public void UnlockZones(List<int> zoneIds)
    {
        foreach (int zoneId in zoneIds)
        {
            UnlockZone(zoneId);
        }
    }

    public List<int> GetUnlockedZoneIds()
    {
        return unlockedZoneIds;
    }

    public ZoneEntry GetZoneById(int zoneId)
    {
        return zoneDatabase.GetZoneById(zoneId);
    }
}
