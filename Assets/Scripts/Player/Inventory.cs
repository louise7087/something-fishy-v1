using System.Linq;
using UnityEngine;

public class Inventory : Container
{
    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;

    private ItemEntry inHand;

    private int money;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EquipItem(string id)
    {
        if(items.Any(i => i.item.id == id))
        {
            // We have this item in inventory so lets equip it
            inHand = items.First(i => i.item.id == id).item;
            Instantiate(inHand.prefab, rightHand.transform);
        }
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
}
