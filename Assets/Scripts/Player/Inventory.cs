using System.Linq;
using UnityEngine;

public class Inventory : Container
{
    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;

    private ItemEntry inHand;
    private GameObject inHandObject;

    [SerializeField] private int money;

    public void EquipItemAtIndex(int index)
    {
        Unequip();

        var stack = stacks.FirstOrDefault(i => i.position == index);

        if(stack == null) return;

        inHand = stack.item;
        inHandObject = Instantiate(inHand.prefab, rightHand.transform);
    }

    public void Unequip()
    {
        if (inHand == null || inHandObject == null) return;

        inHand = null;
        Destroy(inHandObject);
        inHandObject = null;
    }

    public ItemEntry GetEquippedItem()
    {
        return inHand;
    }

    public int GetMoney()
    {
        return money;
    }

    public void SetMoney(int money)
    {
        this.money = money;
    }

    public void DeltaMoney(int delta)
    {
        money += delta;
    }

    public void SetInHandLayer(int layer)
    {
        if(inHand == null) return;

        inHandObject.GetComponentInChildren<SpriteRenderer>().sortingOrder = layer;
    }

    public void SetHand(bool right)
    {
        if (inHand == null) return;

        if (right)
        {
            inHandObject.transform.parent = rightHand.transform;
            inHandObject.transform.localPosition = Vector3.zero;
        }
        else
        {
            inHandObject.transform.parent = leftHand.transform;
            inHandObject.transform.localPosition = Vector3.zero;
        }
    }
}
