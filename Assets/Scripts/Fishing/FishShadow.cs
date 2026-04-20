using UnityEngine;

public class FishShadow : MonoBehaviour
{
    private ItemManager itemManager;
    private GameManager gameManager;

    private FishEntry fishEntry;

    private float timeToLive = 15f;
    private float deviation = 4f;

    private float timeAlive;
    private bool isBeingFished;

    private void Awake()
    {
        itemManager = GameObject.FindWithTag("ItemManager").GetComponent<ItemManager>();
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AssignFish();

        timeToLive = timeToLive + Random.Range(-deviation, deviation);
    }

    private void Update()
    {
        if (!isBeingFished)
        {
            timeAlive += Time.deltaTime;

            if(timeAlive >= timeToLive)
            {
                SelfDestruct();
            }
        }
    }

    private void AssignFish()
    {
        // Determines what this fish will be once caught
        fishEntry = itemManager.fishDatabase.GetRandomFishFromSeason(gameManager.GetSeason());
    }

    public void Catch()
    {
        // Change this later but for now:
        Caught();
    }

    private void Caught()
    {
        // Method will be called when fish has been caught
        Debug.Log($"Caught {fishEntry.id}");
        gameManager.GetPlayer().GetComponent<Inventory>().AddItem(fishEntry.id);

        SelfDestruct();
    }

    public void SelfDestruct()
    {
        gameManager.GetEnvironmentManager().DecreaseFishCount();
        Destroy(gameObject);
    }

    public FishEntry GetFishEntry()
    {
        return fishEntry;
    }

    public void SetBeingFished(bool isBeingFished)
    {
        this.isBeingFished = isBeingFished;
    }
}
