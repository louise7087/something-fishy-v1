using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] public string id;
    [SerializeField] public int spriteId;

    public Item()
    {
        id = "test";
        spriteId = 0;
    }

    public Item(string name, int spriteId)
    {
        this.id = name;
        this.spriteId = spriteId;
    }

    public Item(string name)
    {
        this.id = name;
    }
}
