using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class MarketplaceUI : MonoBehaviour
{

    private GameManager gameManager;

    private UIDocument uiDocument;

    private Inventory inventory;

    private ItemManager itemManager;

    private List<ItemStack> sellStacks;
    private List<ItemStack> buyStacks = new List<ItemStack>();

    private Image[] sellImageSlots;
    private Label[] sellTextSlots;
    private Button[] sellButtonSlots;

    private Image[] buyImageSlots;
    private Label[] buyTextSlots;
    private Button[] buyButtonSlots;

    private Label moneyText;
    private Label seasonText;

    private VisualElement itemInfo;
    private Label itemName;
    private Label itemValue;
    private Label itemDifficulty;
    private Label itemSeason;

    private bool isOpen;
    private bool showItemInfo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();

        uiDocument = GetComponent<UIDocument>();

        itemManager = GameObject.FindWithTag("ItemManager").GetComponent<ItemManager>();
    }

    private void Update()
    {
        if (showItemInfo)
        {
            Vector2 screenPos = Mouse.current.position.ReadValue();
            Vector2 panelPos = RuntimePanelUtils.ScreenToPanel(itemInfo.panel, screenPos);
            float flippedY = itemInfo.panel.visualTree.resolvedStyle.height - panelPos.y;

            itemInfo.style.left = panelPos.x + 16;
            itemInfo.style.top = flippedY + 16;
        }
    }

    public void Init()
    {
        inventory = gameManager.GetPlayer().GetComponent<Inventory>();

        var rodEntrys = itemManager.GetAllRods();

        for(int i = 0; i < rodEntrys.Count; i++)
        {
            buyStacks.Add(new ItemStack(rodEntrys[i], i, 1));
        }
    }

    private void SetupUI()
    {
        var root = uiDocument.rootVisualElement;

        sellImageSlots = new Image[inventory.GetCapacity()];
        sellTextSlots = new Label[inventory.GetCapacity()];
        sellButtonSlots = new Button[inventory.GetCapacity()];

        buyImageSlots = new Image[inventory.GetCapacity()];
        buyTextSlots = new Label[inventory.GetCapacity()];
        buyButtonSlots = new Button[inventory.GetCapacity()];

        moneyText = root.Q<Label>("marketplace-text-money");
        seasonText = root.Q<Label>("marketplace-text-season");

        itemInfo = root.Q<VisualElement>("item-info");
        itemName = root.Q<Label>("item-text-name");
        itemValue = root.Q<Label>("item-text-value");
        itemDifficulty = root.Q<Label>("item-text-difficulty");
        itemSeason = root.Q<Label>("item-text-season");

        for (int i = 0; i < sellImageSlots.Length; i++)
        {
            sellImageSlots[i] = root.Q<Image>($"marketplace-sell-slot-image-{i}");
            sellTextSlots[i] = root.Q<Label>($"marketplace-sell-slot-text-{i}");
            sellButtonSlots[i] = root.Q<Button>($"marketplace-sell-slot-button-{i}");

            buyImageSlots[i] = root.Q<Image>($"marketplace-buy-slot-image-{i}");
            buyTextSlots[i] = root.Q<Label>($"marketplace-buy-slot-text-{i}");
            buyButtonSlots[i] = root.Q<Button>($"marketplace-buy-slot-button-{i}");

            int index = i;

            sellButtonSlots[i].clicked += () => OnInventoryButtonClicked(index, false);
            buyButtonSlots[i].clicked += () => OnInventoryButtonClicked(index, true);

            sellButtonSlots[i].RegisterCallback<PointerEnterEvent>(_ => ShowItemInfo(index, false));
            sellButtonSlots[i].RegisterCallback<PointerLeaveEvent>(_ => HideItemInfo());

            buyButtonSlots[i].RegisterCallback<PointerEnterEvent>(_ => ShowItemInfo(index, true));
            buyButtonSlots[i].RegisterCallback<PointerLeaveEvent>(_ => HideItemInfo());
        }
    }

    private void DeregisterButtons()
    {
        var root = uiDocument.rootVisualElement;

        sellButtonSlots = new Button[inventory.GetCapacity()];

        for (int i = 0; i < sellImageSlots.Length; i++)
        {
            sellButtonSlots[i] = root.Q<Button>($"marketplace-sell-slot-button-{i}");
            buyButtonSlots[i] = root.Q<Button>($"marketplace-buy-slot-button-{i}");

            int index = i;

            sellButtonSlots[i].clicked -= () => OnInventoryButtonClicked(index, false);
            buyButtonSlots[i].clicked -= () => OnInventoryButtonClicked(index, true);
        }
    }

    private void RefreshInventory()
    {
        sellStacks = inventory.GetItems()
            .Where(i => i.item is FishEntry)
            .Select(i => new ItemStack(i.item, i.position, i.amount))
            .ToList();

        inventory.RefreshItemValues();

        moneyText.text = $"${inventory.GetMoney()}";
        seasonText.text = gameManager.GetSeason().ToString();

        for (int i = 0; i < sellImageSlots.Length; i++)
        {
            sellImageSlots[i].style.backgroundImage = StyleKeyword.None;
            sellTextSlots[i].text = "";

            buyImageSlots[i].style.backgroundImage = StyleKeyword.None;
            buyTextSlots[i].text = "";
        }

        for (int i = 0; i < sellStacks.Count; i++)
        {
            sellStacks[i].position = i;

            sellImageSlots[i].style.backgroundImage =
                new StyleBackground(sellStacks[i].item.prefab.GetComponentInChildren<SpriteRenderer>().sprite);

            sellTextSlots[i].text = sellStacks[i].amount.ToString();
        }

        for (int i = 0; i < buyStacks.Count; i++)
        {
            buyImageSlots[i].style.backgroundImage =
                new StyleBackground(buyStacks[i].item.prefab.GetComponentInChildren<SpriteRenderer>().sprite);

            buyTextSlots[i].text = $"${buyStacks[i].item.value.ToString()}";
        }
    }

    public void Toggle()
    {
        isOpen = !isOpen;

        if (isOpen)
        {
            uiDocument.enabled = true;
            SetupUI();
            RefreshInventory();
        }
        else
        {
            DeregisterButtons();
            HideItemInfo();
            uiDocument.enabled = false;
        }
    }

    public void Open()
    {
        if (isOpen) return;

        isOpen = true;
        uiDocument.enabled = true;
        SetupUI();
        RefreshInventory();
    }

    public void Close()
    {
        if (!isOpen) return;

        isOpen = false;
        DeregisterButtons();
        HideItemInfo();
        uiDocument.enabled = false;
    }

    private void OnInventoryButtonClicked(int index, bool isBuyButton)
    {
        ItemStack stack = null;

        if(isBuyButton)
        {
            stack = buyStacks.FirstOrDefault(i => i.position == index);
            if (stack == null) return;
            inventory.BuyItem(stack.item.id);
        }
        else
        {
            stack = sellStacks.FirstOrDefault(i => i.position == index);
            if (stack == null) return;
            inventory.SellItem(stack.item.id);
        }

        RefreshInventory();
    }

    private void ShowItemInfo(int index, bool isBuySlot)
    {
        itemInfo.style.opacity = 1f;
        showItemInfo = true;

        ItemStack stack = null;

        if (isBuySlot)
        {
            stack = buyStacks.FirstOrDefault(i => i.position == index);
        }
        else
        {
            stack = sellStacks.FirstOrDefault(i => i.position == index);
        }


        if (stack == null)
        {
            HideItemInfo();
            return;
        }

        itemName.text = stack.item.name;
        itemValue.text = $"${stack.item.value}";

        // Clear season text
        itemSeason.text = string.Empty;

        if (stack.item is RodEntry rod)
        {
            itemDifficulty.text = $"Strength: {rod.strength}";
            itemValue.text = string.Empty;
        }
        else if (stack.item is FishEntry fish)
        {
            itemDifficulty.text = $"Difficulty: {fish.difficulty}";
            itemSeason.text = fish.season.ToString();
        }
    }

    private void HideItemInfo()
    {
        itemInfo.style.opacity = 0f;
        showItemInfo = false;
    }
}
