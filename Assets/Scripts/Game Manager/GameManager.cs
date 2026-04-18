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

    private Bobber bobber;

    void Awake()
    {
        dataManager = GameObject.FindWithTag("DataManager").GetComponent<DataManager>();
        environmentManager = GameObject.FindWithTag("EnvironmentManager").GetComponent<EnvironmentManager>();
        itemManager = GameObject.FindWithTag("ItemManager").GetComponent <ItemManager>();
        bobber = GameObject.FindWithTag("Bobber").GetComponent<Bobber>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFishing && !inFishingGame)
        {
            if(fails >= maxAllowedFails - currentFish.GetFishEntry().Difficulty)
            {
                currentFish.SelfDestruct();
                ResetFishCatchGame();
                return;
            }

            if(wins >= currentFish.GetFishEntry().Difficulty)
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
        Debug.Log($"{maxAllowedFails - currentFish.GetFishEntry().Difficulty} fails to lose");
        Debug.Log($"{currentFish.GetFishEntry().Difficulty} wins to win");
    }

    private void TryStartFishingAttempt()
    {
        fishingTimer = 0;

        if (Random.Range(0f, 1f) <= fishGameChance)
        {
            inFishingGame = true;
            float moveSpeed = 300f + Random.Range(-25f, 25f);
            int chances = 6 - currentFish.GetFishEntry().Difficulty;
            fishCatchMinigame.StartGame(moveSpeed, 30, 70, chances);
        }
    }

    public void SetFishCatchOutcome(bool outcome)
    {
        if (outcome)
        {
            // Game was won
            wins++;
            Debug.Log($"Wins: {wins}");
            Debug.Log($"Fails: {fails}");
        }
        else
        {
            // Game was lost
            fails++;
            Debug.Log($"Wins: {wins}");
            Debug.Log($"Fails: {fails}");
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
}

public enum Season
{
    SPRING,
    SUMMER,
    AUTUMN,
    WINTER
}
