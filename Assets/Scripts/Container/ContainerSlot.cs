using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerSlot : MonoBehaviour
{
    public Item item;
    public int position;
    public int amount;

    public ContainerSlot()
    {
        this.item = new Item();
    }

    public ContainerSlot(Item item, int position, int amount)
    {
        this.item = item;
        this.position = position;
        this.amount = amount;
    }
}
