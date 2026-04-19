using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ZoneManager : MonoBehaviour
{
    [SerializeField] private ZoneDatabase zoneDatabase;

    private List<string> unlockedZoneIds = new List<string>();

    private Dictionary<string, GameObject> lockedZonesGameObjects = new Dictionary<string, GameObject>();

    private void Start()
    {
        zoneDatabase.Init();

        // Spawn all zones in for now
        foreach(ZoneEntry zoneEntry in zoneDatabase.items)
        {
            lockedZonesGameObjects.Add(zoneEntry.id, Instantiate(zoneEntry.prefab));
        }

        Debug.Log("Spawned all zones");
    }

    public void UnlockZone(string zoneId)
    {
        Destroy(lockedZonesGameObjects[zoneId]);
        lockedZonesGameObjects.Remove(zoneId);
        unlockedZoneIds.Add(zoneId);

        Debug.Log($"Unlocked zone {zoneId}");
    }

    public void UnlockZones(List<string> zoneIds)
    {
        foreach (string zoneId in zoneIds)
        {
            UnlockZone(zoneId);
        }
    }

    public List<string> GetUnlockedZoneIds()
    {
        return unlockedZoneIds;
    }

    public ZoneEntry GetZoneByName(string zoneId)
    {
        return zoneDatabase.GetZoneByName(zoneId);
    }
}
