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

    private Camera mainCamera;

    private List<ItemStack> stacks;

    private Image[] imageSlots;
    private Label[] textSlots;
    private Button[] buttonSlots;

    private Label moneyText;
    private Label seasonText;

    private VisualElement itemInfo;
    private Label itemName;
    private Label itemValue;
    private Label itemDifficulty;
    private Label itemSeason;

    private bool isOpen;
    private bool showItemInfo;

    private bool hasSetupUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();

        mainCamera = Camera.main;

        uiDocument = GetComponent<UIDocument>();
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
    }

    private void SetupUI()
    {
        var root = uiDocument.rootVisualElement;

        imageSlots = new Image[inventory.GetCapacity()];
        textSlots = new Label[inventory.GetCapacity()];
        buttonSlots = new Button[inventory.GetCapacity()];

        moneyText = root.Q<Label>("inventory-text-money");
        seasonText = root.Q<Label>("inventory-text-season");

        itemInfo = root.Q<VisualElement>("item-info");
        itemName = root.Q<Label>("item-text-name");
        itemValue = root.Q<Label>("item-text-value");
        itemDifficulty = root.Q<Label>("item-text-difficulty");
        itemSeason = root.Q<Label>("item-text-season");

        for (int i = 0; i < imageSlots.Length; i++)
        {
            imageSlots[i] = root.Q<Image>($"inventory-slot-image-{i}");
            textSlots[i] = root.Q<Label>($"inventory-slot-text-{i}");
            buttonSlots[i] = root.Q<Button>($"inventory-slot-button-{i}");

            int index = i;

            buttonSlots[i].clicked += () => OnInventoryButtonClicked(index);

            buttonSlots[i].RegisterCallback<PointerEnterEvent>(_ => ShowItemInfo(index));
            buttonSlots[i].RegisterCallback<PointerLeaveEvent>(_ => HideItemInfo());
        }
    }

    private void DeregisterButtons()
    {
        var root = uiDocument.rootVisualElement;

        buttonSlots = new Button[inventory.GetCapacity()];

        for (int i = 0; i < imageSlots.Length; i++)
        {
            buttonSlots[i] = root.Q<Button>($"inventory-slot-button-{i}");

            int index = i;

            buttonSlots[i].clicked -= () => OnInventoryButtonClicked(index);
        }
    }

    private void RefreshInventory()
    {
        stacks = inventory.GetItems()
            .Where(i => i.item is FishEntry)
            .Select(i => new ItemStack(i.item, i.position, i.amount))
            .ToList();

        inventory.RefreshItemValues();

        moneyText.text = $"${inventory.GetMoney()}";
        seasonText.text = gameManager.GetSeason().ToString();

        for (int i = 0; i < imageSlots.Length; i++)
        {
            imageSlots[i].style.backgroundImage = StyleKeyword.None;
            textSlots[i].text = "";
        }

        for (int i = 0; i < stacks.Count; i++)
        {
            stacks[i].position = i;

            imageSlots[i].style.backgroundImage =
                new StyleBackground(stacks[i].item.prefab.GetComponentInChildren<SpriteRenderer>().sprite);

            textSlots[i].text = stacks[i].amount.ToString();
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

    private void OnInventoryButtonClicked(int index)
    {
        var stack = stacks.FirstOrDefault(i => i.position == index);

        if (stack == null) return;

        inventory.SellItem(stack.item.id);

        RefreshInventory();
    }

    private void ShowItemInfo(int index)
    {
        itemInfo.style.opacity = 1f;
        showItemInfo = true;

        var stack = stacks.FirstOrDefault(i => i.position == index);

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
