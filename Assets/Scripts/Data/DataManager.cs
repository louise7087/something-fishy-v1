using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
using System.Linq;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;


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

        var marketPriceRepository = new MarketPriceRepository(dbPath);
        await marketPriceRepository.SeedDefaultsAsync();

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

    public void WipeSave()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        if (File.Exists(Application.persistentDataPath + INVENTORY_PATH))
        {
            File.Delete(Application.persistentDataPath + INVENTORY_PATH);
        }
    }

    public async Task<int> GetItemPrice(string itemId)
    {
        var dbPath = DbPathProvider.GetDatabasePath();

        var marketRepo = new MarketPriceRepository(dbPath);

        var sellFishService = new SellFishToMarketService(marketRepo);

        var price = await sellFishService.loadFishPrice(itemId);

        return price;
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