using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnvironmentManager : MonoBehaviour
{
    [SerializeField] private float fishSpawnChance = 0.5f;
    [SerializeField] private float secondsBetweenFishSpawn = 5f;
    [SerializeField] private int maxFish = 10;
    [SerializeField] private GameObject fishShadowPrefab;

    [SerializeField] private Tilemap waterTilemap;

    private float timer;

    private int fishCount;

    private List<Vector3Int> validTiles = new List<Vector3Int>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CollectTiles();
    }

    // Update is called once per frame
    void Update()
    {
        if(timer >= secondsBetweenFishSpawn && fishCount < maxFish)
        {
            StartFishSpawn();
            timer = 0;
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    private void CollectTiles()
    {
        // Collects all tiles in waterTileMap and puts them in a list
        foreach(Vector3Int pos in waterTilemap.cellBounds.allPositionsWithin)
        {
            if (waterTilemap.HasTile(pos))
            {
                validTiles.Add(pos);
            }
        }
    }

    private void StartFishSpawn()
    {
        // Will spawn a fish if random number is lower than fishSpawnChance
        if(Random.Range(0f, 1f) < fishSpawnChance)
        {
            // Spawn fish
            Vector3Int randomTile = validTiles[Random.Range(0, validTiles.Count)];
            Instantiate(fishShadowPrefab, waterTilemap.GetCellCenterWorld(randomTile), Quaternion.identity);
            fishCount++;
        }
    }

    public void DecreaseFishCount()
    {
        fishCount--;
    }
}
