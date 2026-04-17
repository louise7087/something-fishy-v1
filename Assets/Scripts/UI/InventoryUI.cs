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
        stacks = inventory.GetItems();

        var root = uiDocument.rootVisualElement;

        imageSlots = new Image[inventory.GetCapacity()];

        for (int i = 0; i < imageSlots.Length; i++)
        {
            imageSlots[i] = root.Q<Image>($"inventory-slot-image-{i}");
        }

        foreach(ItemStack stack in stacks)
        {
            int position = stack.position;

            imageSlots[position].style.backgroundImage = new StyleBackground(stack.item.prefab.GetComponent<SpriteRenderer>().sprite);
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
