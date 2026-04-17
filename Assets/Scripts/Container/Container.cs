using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    [SerializeField] private int capacity;

    private List<ItemStack> items = new List<ItemStack>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItem(ItemStack item)
    {
        foreach (var stack in items)
        {
            // Try stacking first
            if(stack.item == item.item)
            {
                // We already have this item in container, can it stack?
                if(stack.amount + item.amount < stack.item.maxStack)
                {
                    // Can be stacked so we can add item
                    items.Add(item);
                }
                else
                {
                    // Can't be stacked so try adding to next available spot
                    if(items.Count < capacity)
                    {
                        // We have a free spot
                        items.Add(item);
                    }
                }
            }
            else
            {
                // We don't have this item in container so add to next spot
                if(items.Count < capacity)
                {
                    items.Add(item);
                }
            }
        }
    }
}

public class ItemStack : MonoBehaviour
{
    public ItemEntry item;
    public int amount;

    public ItemStack(ItemEntry item)
    {
        this.item = item;
        amount = 1;
    }

    public ItemStack(ItemEntry item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }
}
