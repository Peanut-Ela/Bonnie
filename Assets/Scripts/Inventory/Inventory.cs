using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public Slot slotPrefab;
    public GridLayoutGroup slotGroup;
    public List<Slot> slots = new List<Slot>();

    void Start()
    {
        SetTotalSlots();
        Pickup.OnItemPickup += OnItemPickedUp;
    }

    private void OnDestroy()
    {
        Pickup.OnItemPickup -= OnItemPickedUp;
    }

    public void SetTotalSlots()
    {
        for (int i = 0; i < Player.instance.inventorySlots; i++)
        {
            var slot = Instantiate(slotPrefab, slotGroup.transform);
            slot.index = i;
            slots.Add(slot);
        }
    }

    // Event handler for when an item is picked up
    private void OnItemPickedUp(Sprite itemSprite, int itemID)
    {
        // Find the first empty slot
        Slot emptySlot = slots.Find(slot => slot.IsEmpty());

        if (emptySlot != null)
        {
            emptySlot.SetItemIcon(itemSprite, itemID); // Pass the itemID along with the item sprite
        }
        else
        {
            Debug.LogWarning("Inventory is full. Cannot add the item.");
        }
    }
}
