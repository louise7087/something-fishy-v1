using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryUI : MonoBehaviour
{
    private GameManager gameManager;

    private UIDocument uiDocument;

    private Inventory inventory;

    private List<ItemStack> stacks;

    private Image[] imageSlots;

    private Label[] textSlots;

    private bool isOpen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();

        uiDocument = GetComponent<UIDocument>();
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

        for (int i = 0; i < imageSlots.Length; i++)
        {
            imageSlots[i] = root.Q<Image>($"inventory-slot-image-{i}");
            textSlots[i] = root.Q<Label>($"inventory-slot-text-{i}");
        }

        stacks = inventory.GetItems();

        foreach(ItemStack stack in stacks)
        {
            int position = stack.position;

            imageSlots[position].style.backgroundImage = new StyleBackground(stack.item.prefab.GetComponent<SpriteRenderer>().sprite);
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
    }
}
