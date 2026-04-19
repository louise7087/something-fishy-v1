using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
using System.Linq;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;


public class DataManager : MonoBehaviour
{
    private GameObject player;
    private Inventory inventory;

    private GameManager gameManager;
    private ZoneManager zoneManager;

    private const string INVENTORY_PATH = "/inventory.json";

    private void Awake()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        zoneManager = GameObject.FindWithTag("ZoneManager").GetComponent<ZoneManager>();
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player;
        inventory = player.GetComponent<Inventory>();
    }

    public async void Load()
    {
        var dbPath = DbPathProvider.GetDatabasePath();

        using (var db = new GameDbContext(dbPath))
        {
            db.Database.EnsureCreated();
        }

        var playerRepository = new PlayerRepository(dbPath);
        var loadService = new LoadPlayerService(playerRepository);

        var result = await loadService.LoadorCreateLocalPlayerAsync();
        var save = result.Player;

        inventory.SetMoney(save.BalanceMinorUnits);

        var stacks = new List<ItemStack>();

        foreach (var itemStack in save.InventoryItems)
        {
            var item = gameManager.GetItemManager().GetItemById(itemStack.ItemDefinitionKey);

            if (item != null)
            {
                stacks.Add(new ItemStack(item, itemStack.SlotIndex, itemStack.Quantity));
            }
            else
            {
                Debug.LogWarning($"Could not find item with id {itemStack.ItemDefinitionKey} in item manager");
                continue;
            }
        }

        inventory.SetItems(stacks);

        LoadPosition();
    }

    public async void Save()
    {
        if (player == null || inventory == null)
        {
            Debug.LogWarning("No active player to save.");
            return;
        }

        SavePosition();

        if (ActivePlayerSession.CurrentPlayerId == null)
        {
            Debug.LogError("No active player session found, cannot save");
            return;
        }

        string dbPath = DbPathProvider.GetDatabasePath();

        using (var db = new GameDbContext(dbPath))
        {
            db.Database.EnsureCreated();
        }

        var saveService = new SavePlayerProgressService(
            new PlayerRepository(dbPath),
            new InventoryRepository(dbPath),
            new WalletRepository(dbPath)
        );

        var save = new PlayerSaveModel
        {
            PlayerId = ActivePlayerSession.CurrentPlayerId.Value,
            DisplayName = "Player",
            ProgressionLevel = 0,
            CurrentAreaKey = "starter_area",
            EquippedToolKey = "basic_rod",
            BalanceMinorUnits = inventory.GetMoney(),
            InventoryItems = inventory.GetItems().Select(stack => new InventoryItemModel
            {
                ItemDefinitionKey = stack.item.id,
                Quantity = stack.amount,
                SlotIndex = stack.position,
            }).ToList()
        };

        await saveService.SavePlayerProgressAsync(save);

        PlayerPrefs.Save();
    }

    private void LoadPosition()
    {
        if (player == null)
        {
            return;
        }

        player.transform.position = new Vector2(PlayerPrefs.GetFloat("playerPosX"), PlayerPrefs.GetFloat("playerPosY"));
    }

    private void SavePosition()
    {
        if (player == null)
        {
            return;
        }

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

        foreach (ItemStack stack in stacks)
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

        if (File.Exists(Application.persistentDataPath + INVENTORY_PATH))
        {
            File.Delete(Application.persistentDataPath + INVENTORY_PATH);
        }
    }

    private void LoadUnlockedZones()
    {
        string serialized = PlayerPrefs.GetString("unlockedZones", "");

        List<int> lockedZones = new List<int>();

        if (!string.IsNullOrEmpty(serialized))
        {
            string[] parts = serialized.Split(',');

            foreach (string part in parts)
            {
                if(int.TryParse(part, out int value))
                {
                    lockedZones.Add(value);
                }
            }
        }

        zoneManager.UnlockZones(lockedZones);
    }

    private void SaveUnlockedZones()
    {
        var zones = zoneManager.GetUnlockedZoneIds();
        
        string serialized  = string.Join(",", zones);
        PlayerPrefs.SetString("unlockedZones", serialized);
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