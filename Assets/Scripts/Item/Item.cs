using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] public string id;

    public Item()
    {
        id = "id.base";
    }

    public Item(string name)
    {
        this.id = name;
    }
}
