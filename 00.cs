using System;
using System.Collections.Generic;

public class Inventory
{
    private List<Item> items;

    public Inventory()
    {
        items = new List<Item>();
    }

    // 添加物品
    public void AddItem(Item newItem)
    {
        Item existingItem = items.Find(item => item.Name == newItem.Name);
        if (existingItem != null)
        {
            existingItem.Quantity += newItem.Quantity;
        }
        else
        {
            items.Add(newItem);
        }
        Console.WriteLine($"Added {newItem.Quantity} {newItem.Name}(s) to the inventory.");
    }

    // 移除物品
    public void RemoveItem(string itemName, int quantity)
    {
        Item existingItem = items.Find(item => item.Name == itemName);
        if (existingItem != null)
        {
            if (existingItem.Quantity >= quantity)
            {
                existingItem.Quantity -= quantity;
                if (existingItem.Quantity == 0)
                {
                    items.Remove(existingItem);
                }
                Console.WriteLine($"Removed {quantity} {itemName}(s) from the inventory.");
            }
            else
            {
                Console.WriteLine($"Not enough {itemName} in the inventory.");
            }
        }
        else
        {
            Console.WriteLine($"{itemName} not found in the inventory.");
        }
    }

    // 查看背包
    public void ViewInventory()
    {
        if (items.Count == 0)
        {
            Console.WriteLine("The inventory is empty.");
        }
        else
        {
            Console.WriteLine("Inventory Contents:");
            foreach (var item in items)
            {
                Console.WriteLine(item);
            }
        }
    }
}