using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Container : MonoBehaviour
{
    [SerializeField] private int capacity;

    [SerializeField] protected List<ItemStack> items = new List<ItemStack>();

    private ItemDatabase database;

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
        /*foreach (var stack in items)
        {
            // Try stacking first
            if(stack.item == item.item)
            {
                // We already have this item in container, can it stack?
                if(stack.amount + item.amount < stack.item.maxStack)
                {
                    // Can be stacked so we can add item
                    items.Add(item);
                    Debug.Log($"Added {item.item.id} to container");

                }
                else
                {
                    // Can't be stacked so try adding to next available spot
                    if(items.Count < capacity)
                    {
                        // We have a free spot
                        items.Add(item);
                        Debug.Log($"Added {item.item.id} to container");
                    }
                }
            }
            else
            {
                // We don't have this item in container so add to next spot
                if(items.Count < capacity)
                {
                    items.Add(item);
                    Debug.Log($"Added {item.item.id} to container");
                }
            }
        }*/

        items.Add(item);
        Debug.Log($"Added {item.item.id} to container");
    }

    public void AddItem(string id)
    {
        ItemEntry item = GameObject.FindWithTag("ItemManager").GetComponent<ItemManager>().GetItemById(id);
        ItemStack stack = new ItemStack(item);
        AddItem(stack);
    }

    public void SetItems(List<ItemStack> items)
    {
        this.items = items;
    }

    public List<ItemStack> GetItems()
    {
        return items;
    }

    public bool ContainsItem(string id)
    {
        return items.Any(i => i.item.id == id);
    }

    public int GetCapacity()
    {
        return capacity;
    }
}

[System.Serializable]
public class ItemStack
{
    public ItemEntry item;
    public int position;
    public int amount;

    public ItemStack(ItemEntry item)
    {
        this.item = item;
        amount = 1;
    }

    public ItemStack(ItemEntry item, int position, int amount)
    {
        this.item = item;
        this.position = position;
        this.amount = amount;
    }
}
