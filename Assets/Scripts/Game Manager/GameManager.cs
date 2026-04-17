using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Season currentSeason = Season.SPRING;
    [SerializeField] private GameObject playerPrefab;

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
}

public enum Season
{
    SPRING,
    SUMMER,
    AUTUMN,
    WINTER
}
