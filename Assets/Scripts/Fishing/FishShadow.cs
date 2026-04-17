using UnityEngine;

public class FishShadow : MonoBehaviour
{
    private ItemManager itemManager;
    private GameManager gameManager;

    private FishEntry fishEntry;

    private void Awake()
    {
        itemManager = GameObject.FindWithTag("ItemManager").GetComponent<ItemManager>();
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AssignFish();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void AssignFish()
    {
        // Determines what this fish will be once caught
        fishEntry = itemManager.fishDatabase.GetRandomFishFromSeason(gameManager.GetSeason());
        Debug.Log($"Assigned {fishEntry.id} to fish shadow!");
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
        ItemStack stack = new ItemStack(fishEntry);
        gameManager.GetPlayer().GetComponent<Inventory>().AddItem(stack);
        gameManager.GetEnvironmentManager().DecreaseFishCount();

        // Delete fish shadow
        Destroy(gameObject);
    }
}
