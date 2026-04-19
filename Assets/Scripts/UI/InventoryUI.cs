using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class InventoryUI : MonoBehaviour
{

    private GameManager gameManager;

    private UIDocument uiDocument;

    private Inventory inventory;

    private Camera mainCamera;

    private List<ItemStack> stacks;

    private Image[] imageSlots;
    private Label[] textSlots;
    private Button[] buttonSlots;

    private VisualElement itemInfo;
    private Label itemName;
    private Label itemValue;
    private Label itemDifficulty;

    private bool isOpen;
    private bool showItemInfo;

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

    private void RefreshInventory()
    {
        var root = uiDocument.rootVisualElement;

        imageSlots = new Image[inventory.GetCapacity()];
        textSlots = new Label[inventory.GetCapacity()];
        buttonSlots = new Button[inventory.GetCapacity()];

        itemInfo = root.Q<VisualElement>("item-info");
        itemName = root.Q<Label>("item-text-name");
        itemValue = root.Q<Label>("item-text-value");
        itemDifficulty = root.Q<Label>("item-text-difficulty");

        for (int i = 0; i < imageSlots.Length; i++)
        {
            imageSlots[i] = root.Q<Image>($"inventory-slot-image-{i}");
            textSlots[i] = root.Q<Label>($"inventory-slot-text-{i}");
            buttonSlots[i] = root.Q<Button>($"inventory-slot-button-{i}");

            int index = i;

            buttonSlots[i].clicked += () => OnInventoryButtonClicked(index);

            buttonSlots[i].RegisterCallback<PointerEnterEvent>(evt =>
            {
                ShowItemInfo(index);
            });

            buttonSlots[i].RegisterCallback<PointerLeaveEvent>(evt =>
            {
                HideItemInfo();
            });
        }

        stacks = inventory.GetItems();

        foreach(ItemStack stack in stacks)
        {
            int position = stack.position;

            imageSlots[position].style.backgroundImage = new StyleBackground(stack.item.prefab.GetComponentInChildren<SpriteRenderer>().sprite);
            textSlots[position].text = stack.amount.ToString();
        }
    }
    
    public void Toggle()
    {
        isOpen = !isOpen;
        uiDocument.enabled = !uiDocument.enabled;

        if (isOpen)
        {
            RefreshInventory();
        }
        else
        {
            HideItemInfo();
        }
    }

    private void OnInventoryButtonClicked(int index)
    {
        inventory.EquipItemAtIndex(index);
        Toggle();
    }

    private void ShowItemInfo(int index)
    {
        itemInfo.style.opacity = 1f;
        showItemInfo = true;

        var stack = inventory.GetStackAtIndex(index);

        if (stack == null)
        {
            HideItemInfo();
            return;
        }

        itemName.text = stack.item.name;
        itemValue.text = $"${stack.item.value}";
        
        if (stack.item is RodEntry rod)
        {
            itemDifficulty.text = $"{rod.strength} strength";
        }
        else if (stack.item is FishEntry fish)
        {
            itemDifficulty.text = $"{fish.difficulty} difficulty";
        }
    }

    private void HideItemInfo()
    {
        itemInfo.style.opacity = 0f;
        showItemInfo = false;
    }
}
