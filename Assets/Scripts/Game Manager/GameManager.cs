using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Season currentSeason = Season.SPRING;
    [SerializeField] private GameObject playerPrefab;

    [Header("Fishing Settings")]
    [SerializeField] FishCatch fishCatchMinigame;
    [SerializeField] private float fishGameChance;
    [SerializeField] private float minTimeBetweenAttempts;
    [SerializeField] private int maxAllowedFails;

    private int wins;
    private int fails;
    private bool isFishing;
    private bool inFishingGame;
    private float fishingTimer;
    private FishShadow currentFish;

    private GameObject player;

    private DataManager dataManager;
    private ItemManager itemManager;
    private EnvironmentManager environmentManager;

    void Awake()
    {
        dataManager = GameObject.FindWithTag("DataManager").GetComponent<DataManager>();
        environmentManager = GameObject.FindWithTag("EnvironmentManager").GetComponent<EnvironmentManager>();
        itemManager = GameObject.FindWithTag("ItemManager").GetComponent <ItemManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFishing && !inFishingGame)
        {
            if(fails >= maxAllowedFails - currentFish.GetFishEntry().Difficulty)
            {
                Debug.Log("Fishing Result = Lost");
                currentFish.SelfDestruct();
                isFishing = false;
                fishingTimer = 0;
                wins = 0;
                fails = 0;
                return;
            }

            if(wins >= currentFish.GetFishEntry().Difficulty)
            {
                Debug.Log("Fishing result = caught");
                currentFish.Catch();
                isFishing = false;
                fishingTimer = 0;
                wins = 0;
                fails = 0;
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
        Debug.Log("Starting Fishing!");
        currentFish = fish;
        isFishing = true;
    }

    private void TryStartFishingAttempt()
    {
        if(Random.Range(0f, 1f) <= fishGameChance)
        {
            inFishingGame = true;
            float moveSpeed = 300f + Random.Range(-25f, 25f);
            int chances = 6 - currentFish.GetComponent<FishShadow>().GetFishEntry().Difficulty;
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
}

public enum Season
{
    SPRING,
    SUMMER,
    AUTUMN,
    WINTER
}
