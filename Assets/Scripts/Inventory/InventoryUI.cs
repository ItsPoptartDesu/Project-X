using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;
    public GameObject inventoryItemPrefab;

    [SerializeField]
    private InventoryManager inventoryManager;

    private void Start()
    {
        UpdateInventoryUI();
    }

    private void UpdateInventoryUI()
    {
        foreach (Transform child in itemsParent)
        {
            Destroy(child.gameObject);
        }
        if (inventoryItemPrefab == null)
            return;
        foreach (Item item in inventoryManager.GetInventoryItems())
        {
            GameObject itemUI = Instantiate(inventoryItemPrefab , itemsParent);
            // Set the UI elements' text and images based on the item data
            itemUI.GetComponentInChildren<Text>().text = item.itemName;
            itemUI.GetComponentInChildren<Image>().sprite = item.sprite;
        }
    }
}

