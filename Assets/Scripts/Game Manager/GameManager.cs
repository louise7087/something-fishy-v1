using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Season currentSeason = Season.SPRING;
    private GameObject player;

    private DataManager dataManager;
    private ItemManager itemManager;

    void Awake()
    {
        dataManager = GameObject.FindWithTag("DataManager").GetComponent<DataManager>();
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

    public void SetPlayer(GameObject player)
    {
        // Player script calls this method once spawned in
        this.player = player;
        dataManager.SetPlayer(player);

        dataManager.Load();
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

    private void OnApplicationQuit()
    {
        dataManager.Save();
    }
}

public enum Season
{
    SPRING,
    SUMMER,
    AUTUMN,
    WINTER
}
