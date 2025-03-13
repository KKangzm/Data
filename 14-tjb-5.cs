using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    private List<string> items = new List<string>();

    void Awake()
    {
        Instance = this;
    }

    public void AddItem(string itemName)
    {
        items.Add(itemName);
        Debug.Log("Picked up: " + itemName);
    }

    public bool HasItem(string itemName)
    {
        return items.Contains(itemName);
    }
}
