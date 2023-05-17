using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public string itemName;
    public string description;
    public Sprite sprite;
    public ItemType itemType;
    public int quantity;
    // Add any additional properties as needed for your game
}

public enum ItemType
{
    Potion,
    Pokeball,
    // Add more item types as needed
}
