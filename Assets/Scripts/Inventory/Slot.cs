using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Spawn spawnComponent;
    public int index;
    public Image itemImage;
    public bool isItemCollected = false;
    public int itemID; // Store the itemID from the Pickup script

    private void Start()
    {
        // Disable the item image sprite at the start
        itemImage.enabled = false;
    }

    // Check if the slot is empty (has no item)
    public bool IsEmpty()
    {
        return !isItemCollected;
    }

    // Set the icon of the item in this slot and store the itemID
    public void SetItemIcon(Sprite itemSprite, int itemID)
    {
        this.itemID = itemID; // Store the itemID from the Pickup script
        // Enable the item image sprite when an item is collected
        itemImage.sprite = itemSprite;
        itemImage.enabled = true;
        isItemCollected = true;
    }

    // Clear the icon of the item in this slot
    public void ClearItemIcon()
    {
        // Disable the item image sprite when the item is removed
        itemImage.sprite = null;
        itemImage.enabled = false;
        isItemCollected = false;
        itemID = 0;
    }

    private void Update()
    {
        // You can add any additional logic here if needed for updates
    }

    public void Cross()
    {
        if (!IsEmpty())
        {
            
            Player.instance.DropItem(index); // Pass the itemID along with the index to use the item
            Debug.Log("ItemID is" + itemID);
            spawnComponent.SpawnItem(itemID);
            ClearItemIcon();

        }
    }
}
