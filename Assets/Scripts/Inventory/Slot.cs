using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public int index;
    public Image itemImage;

    // Check if the slot is empty (has no item)
    public bool IsEmpty()
    {
        return itemImage.sprite == null;
    }

    // Set the icon of the item in this slot
    public void SetItemIcon(Sprite itemSprite)
    {
        itemImage.sprite = itemSprite;
    }

    // Clear the icon of the item in this slot
    public void ClearItemIcon()
    {
        itemImage.sprite = null;
    }

    private void Update()
    {
        //if (transform.childCount <= 0) {
        //    Player.instance.items[index] = 0;
        //}
    }

    public void Cross()
    {
        ClearItemIcon();
        Player.instance.DropItem(index);
        //foreach (Transform child in transform)
        //{
        //    var spawnComponent = child.GetComponent<Spawn>();
        //    if (spawnComponent != null)
        //    {
        //        // Spawn the item near the player
        //        spawnComponent.SpawnItem();
        //        // Destroy the item image in the slot
        //        // Destroy the item GameObject in the slot
        //        Destroy(child.gameObject);
        //    }
        //    else
        //    {
        //        Debug.Log("Null item in the slot.");
        //    }
        //}
    }
}
