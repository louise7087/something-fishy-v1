using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Container : MonoBehaviour
{
    public int sizeX = -1;
    public int sizeY = -1;
    public List<ContainerSlot> slots = new List<ContainerSlot>();

    public bool isLocked;
    public bool isOpen;
    public bool canOpen = true;

    public ulong inUseBy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Open()
    {
        isOpen = true;
    }

    public void Close()
    {
        isOpen = false;
    }

    private void AddItems(ContainerSlot items)
    {
        if(items.position > (sizeX * sizeY))
        {
            // Tried to add an item which position is greater than the container allows
            Debug.Log($"Can't add {items.item.id} at position {items.position}");
            return;
        }

        foreach(ContainerSlot slot in  slots)
        {
            if(slot.position == items.position && slot.item.id == items.item.id)
            {
                // Edge case for when an item is already in a container, so just add the amount
                slot.amount += items.amount;
                items.amount = 0;
            }
            else if (slot.position == items.position)
            {
                // Edge case for when the slot is taken in the container
                Debug.Log("This slot is already taken!");
                return;
            }
        }

        if(items.amount > 0)
        {
            slots.Add(items);
        }
    }
}
