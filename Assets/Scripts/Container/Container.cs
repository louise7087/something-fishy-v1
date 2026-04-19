using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Container : MonoBehaviour
{
    [SerializeField] private int capacity;

    [SerializeField] protected List<ItemStack> stacks = new List<ItemStack>();

    private ItemDatabase database;

    private bool[] positionTaken;

    private void Awake()
    {
        positionTaken = new bool[capacity];
    }

    public void AddStackAtBestPosition(ItemStack stack)
    {
        if(stacks.Count >= capacity)
        {
            // Container is full
            Debug.Log("Inventory Full");
            return;
        }

        foreach(ItemStack stackToAddTo in stacks.Where(i => i.item.id == stack.item.id && i.amount < i.item.maxStack))
        {
            int amountToAdd = Mathf.Min(stackToAddTo.item.maxStack - stackToAddTo.amount, stack.amount);

            stackToAddTo.amount += amountToAdd;
            stack.amount -= amountToAdd;

            if(stack.amount <= 0)
            {
                return;
            }
        }

        for(int i = 0; i < capacity; i++)
        {
            if (positionTaken[i]) continue;

            int amountToAdd = Mathf.Min(stack.amount, stack.item.maxStack);

            ItemStack newStack = new ItemStack(stack.item, i, amountToAdd);

            stacks.Add(newStack);
            positionTaken[i] = true;

            stack.amount -= amountToAdd;

            if (stack.amount <= 0) return;
        }
    }

    public void AddItem(string id, int amount = 0)
    {
        ItemEntry item = GameObject.FindWithTag("ItemManager").GetComponent<ItemManager>().GetItemById(id);
        ItemStack stack = new ItemStack(item);
        AddStackAtBestPosition(stack);
    }

    public void SetItems(List<ItemStack> stacks)
    {
        this.stacks = stacks;

        foreach(ItemStack stack in stacks)
        {
            positionTaken[stack.position] = true;
        }
    }

    public List<ItemStack> GetItems()
    {
        return stacks;
    }

    public ItemStack GetStackAtIndex(int index)
    {
        return stacks.FirstOrDefault(s => s.position == index);
    }

    public bool ContainsItem(string id)
    {
        return stacks.Any(i => i.item.id == id);
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
    public int position = -1;
    public int amount = 1;

    public ItemStack(ItemEntry item)
    {
        this.item = item;
    }

    public ItemStack(ItemEntry item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }

    public ItemStack(ItemEntry item, int position, int amount)
    {
        this.item = item;
        this.position = position;
        this.amount = amount;
    }
}
