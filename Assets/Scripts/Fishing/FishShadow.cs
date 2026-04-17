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

    public void Caught()
    {
        // Method will be called when fish has been caught
        ItemStack stack = new ItemStack(fishEntry);
        gameManager.GetPlayer().GetComponent<Container>().AddItem(stack);
    }
}
