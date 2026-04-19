using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] public ItemDatabase itemDatabase;
    [SerializeField] public FishDatabase fishDatabase;
    [SerializeField] public RodDatabase rodDatabase;

    private void Awake()
    {
        itemDatabase.Init();
        fishDatabase.Init();
        rodDatabase.Init();
    }

    public ItemEntry GetItemById(string id)
    {
        if(itemDatabase.items.Any(i => i.id == id))
        {
            return itemDatabase.Get(id);
        }

        if (fishDatabase.items.Any(i => i.id == id))
        {
            return fishDatabase.Get(id);
        }

        if (rodDatabase.items.Any(i => i.id == id))
        {
            return rodDatabase.Get(id);
        }

        return null;
    }

    public List<RodEntry> GetAllRods()
    {
        return rodDatabase.items;
    }
}
