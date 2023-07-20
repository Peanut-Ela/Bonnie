using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public int index;
    public Image itemImage;
    private bool isItemCollected = false;

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

    // Set the icon of the item in this slot
    public void SetItemIcon(Sprite itemSprite)
    {
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
    }

    private void Update()
    {
        // You can add any additional logic here if needed for updates
    }

    public void Cross()
    {
        ClearItemIcon();
        Player.instance.DropItem(index);
        // Additional logic for Cross() method
    }
}
