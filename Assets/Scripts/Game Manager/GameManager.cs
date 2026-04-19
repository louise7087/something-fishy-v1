using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    

    [Header("Fishing Settings")]
    [SerializeField] FishCatch fishCatchMinigame;
    [SerializeField] private float fishGameChance;
    [SerializeField] private float minTimeBetweenAttempts;
    [SerializeField] private int maxAllowedFails;

    [SerializeField] private float seasonLengthSeconds = 20;
    [SerializeField] private Season currentSeason = Season.Spring;
    [SerializeField] private GameObject playerPrefab;
    private float currentSeasonTime;

    private int wins;
    private int fails;
    private bool isFishing;
    private bool inFishingGame;
    private float fishingTimer;
    private FishShadow currentFish;

    private GameObject player;
    private Inventory inventory;

    private DataManager dataManager;
    private ItemManager itemManager;
    private EnvironmentManager environmentManager;
    private ZoneManager zoneManager;

    private Bobber bobber;

    void Awake()
    {
        dataManager = GameObject.FindWithTag("DataManager").GetComponent<DataManager>();
        environmentManager = GameObject.FindWithTag("EnvironmentManager").GetComponent<EnvironmentManager>();
        itemManager = GameObject.FindWithTag("ItemManager").GetComponent <ItemManager>();
        bobber = GameObject.FindWithTag("Bobber").GetComponent<Bobber>();
        zoneManager = GameObject.FindWithTag("ZoneManager").GetComponent<ZoneManager>();
    }

    // Update is called once per frame
    void Update()
    {
        currentSeasonTime += Time.deltaTime;

        if(currentSeasonTime >= seasonLengthSeconds)
        {
            NextSeason();
            currentSeasonTime = 0;
        }

        if (isFishing && !inFishingGame)
        {
            if(fails >= maxAllowedFails - currentFish.GetFishEntry().difficulty)
            {
                currentFish.SelfDestruct();
                ResetFishCatchGame();
                return;
            }

            if(wins >= currentFish.GetFishEntry().difficulty)
            {
                currentFish.Catch();
                ResetFishCatchGame();
                return;
            }

            fishingTimer += Time.deltaTime;

            if(fishingTimer >= minTimeBetweenAttempts)
            {
                TryStartFishingAttempt();
            }
        }
    }

    public Season GetSeason()
    {
        return currentSeason;
    }

    public GameObject GetPlayer()
    {
        return player;
    }

    public DataManager GetDataManager()
    {
        return dataManager;
    }

    public ItemManager GetItemManager()
    {
        return itemManager;
    }

    public EnvironmentManager GetEnvironmentManager()
    {
        return environmentManager;
    }

    private void OnApplicationQuit()
    {
        dataManager.Save();
    }

    public void StartGame()
    {
        // Called when new game / load game button is pressed
        player = Instantiate(playerPrefab);
        inventory = player.GetComponent<Inventory>();
        dataManager.SetPlayer(player);
        dataManager.Load();
    }

    public void NewGame()
    {
        dataManager.WipeSave();
        StartGame();
    }

    public void StartFishCatch(FishShadow fish)
    {
        if (isFishing) return;

        fish.SetBeingFished(true);
        currentFish = fish;
        isFishing = true;

        Debug.Log("Starting Fishing!");
    }

    private void TryStartFishingAttempt()
    {
        fishingTimer = 0;

        if (Random.Range(0f, 1f) <= fishGameChance)
        {
            inFishingGame = true;
            float moveSpeed = 300f + Random.Range(-25f, 100f);
            int chances = 6 - currentFish.GetFishEntry().difficulty;
            fishCatchMinigame.StartGame(moveSpeed, 30, 70, chances);
        }
    }

    public void SetFishCatchOutcome(bool outcome)
    {
        if (outcome)
        {
            // Game was won
            wins++;
        }
        else
        {
            // Game was lost
            fails++;
        }

        inFishingGame = false;
        fishingTimer = 0;
    }

    private void ResetFishCatchGame()
    {
        isFishing = false;
        fishingTimer = 0;
        wins = 0;
        fails = 0;
        player.GetComponent<PlayerControls>().FinishedFishing();
    }

    private void NextSeason()
    {
        int count = System.Enum.GetValues(typeof(Season)).Length;
        SetCurrentSeason((Season)(((int)currentSeason + 1) % count));
    }

    public void SetCurrentSeasonTime(float time)
    {
        currentSeasonTime = time;
    }

    public void SetCurrentSeason(Season season)
    {
        currentSeason = season;
    }

    public float GetCurrentSeasonTime()
    {
        return currentSeasonTime;
    }

    public Season GetCurrentSeason()
    {
        return currentSeason;
    }

    public void UnlockZone(string zoneId)
    {
        var zone = zoneManager.GetZoneByName(zoneId);

        // Can player afford?
        if (inventory.GetMoney() >= zone.cost)
        {
            // They can
            inventory.DeltaMoney(-zone.cost);
            zoneManager.UnlockZone(zoneId);
        }
        else
        {
            Debug.Log("Can't afford zone");
        }
    }
}

public enum Season
{
    Spring,
    Summer,
    Autumn,
    Winter
}
