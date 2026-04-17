using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class DataManager : MonoBehaviour
{
    private GameObject player;
    private Inventory inventory;

    private GameManager gameManager;

     private const string INVENTORY_PATH = "/inventory.json";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player;
        inventory = player.GetComponent<Inventory>();
    }

    public void Load()
    {
        Debug.Log("Loading Data");
        LoadPosition();
        LoadInventory();
        LoadMoney();
    }

    public void Save()
    {
        SavePosition(); 
        SaveInventory();
        SaveMoney();
        PlayerPrefs.Save();
    }

    private void LoadPosition()
    {
        player.transform.position = new Vector2(PlayerPrefs.GetFloat("playerPosX"), PlayerPrefs.GetFloat("playerPosY"));
    }

    private void SavePosition()
    {
        PlayerPrefs.SetFloat("playerPosX", player.transform.position.x);
        PlayerPrefs.SetFloat("playerPosY", player.transform.position.y);
    }

    private void LoadInventory()
    {
        string path = Application.persistentDataPath + INVENTORY_PATH;

        if (!File.Exists(path))
        {
            // Inventory json file does not exist.
            Debug.Log("No saved inventory found");
            return;
        }

        string json = File.ReadAllText(path);
        List<ItemStackWrapper> storedWrappedStacks = JsonConvert.DeserializeObject<List<ItemStackWrapper>>(json);
        List<ItemStack> unwrappedStacks = new List<ItemStack>();

        foreach (var wrappedStack in storedWrappedStacks)
        {
            Debug.Log($"Found {wrappedStack.id} in saved inventory");
            ItemStack stack = new ItemStack(gameManager.GetItemManager().GetItemById(wrappedStack.id), wrappedStack.position, wrappedStack.amount);
            unwrappedStacks.Add(stack);
        }

        inventory.SetItems(unwrappedStacks);
    }

    private void SaveInventory()
    {
        List<ItemStack> stacks = inventory.GetItems();
        List<ItemStackWrapper> wrappedStacks = new List<ItemStackWrapper>();

        foreach(ItemStack stack in stacks)
        {
            ItemStackWrapper wrappedStack = new ItemStackWrapper(stack.item.id, stack.position, stack.amount);
            wrappedStacks.Add(wrappedStack);
        }

        string json = JsonConvert.SerializeObject(wrappedStacks, Formatting.Indented);
        File.WriteAllText(Application.persistentDataPath + INVENTORY_PATH, json);
    }

    private void LoadMoney()
    {
        inventory.SetMoney(PlayerPrefs.GetInt("money"));
    }

    private void SaveMoney()
    {
        PlayerPrefs.SetInt("money", inventory.GetMoney());
    }

    public void WipeSave()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        if(File.Exists(Application.persistentDataPath + INVENTORY_PATH))
        {
            File.Delete(Application.persistentDataPath + INVENTORY_PATH);
        }
    }
}

[System.Serializable]
public class ItemStackWrapper
{
    public string id;
    public int position;
    public int amount;

    public ItemStackWrapper(string id, int position, int amount)
    {
        this.id = id;
        this.position = position;
        this.amount = amount;
    }
}
