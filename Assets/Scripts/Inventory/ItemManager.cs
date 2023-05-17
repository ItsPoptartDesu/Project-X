using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InventoryManager : MonoBehaviour
{
    private Dictionary<string , Item> inventory = new Dictionary<string , Item>();
    public List<Item> GetInventoryItems()
    {
        return inventory.Values.ToList();
    }

    public void AddItem(Item item)
    {
        if (inventory.ContainsKey(item.itemName))
        {
            // Item already exists in the inventory, so update the quantity
            inventory[item.itemName].quantity += item.quantity;
        }
        else
        {
            // Add the item to the inventory
            inventory.Add(item.itemName , item);
        }
    }

    public void RemoveItem(Item item)
    {
        if (inventory.ContainsKey(item.itemName))
        {
            inventory[item.itemName].quantity -= item.quantity;
            if (inventory[item.itemName].quantity <= 0)
            {
                // Remove the item from the inventory if the quantity reaches zero
                inventory.Remove(item.itemName);
            }
        }
    }

    // Implement other inventory management functions as needed
}
