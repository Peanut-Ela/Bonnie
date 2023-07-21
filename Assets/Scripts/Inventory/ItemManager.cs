using System;
using System.Collections.Generic;
using UnityEngine;
using static NPC;

public class ItemManager : MonoBehaviour
{
    public List<Item> dialogueDataList = new List<Item>();
    public static ItemManager instance;
    [Serializable]
    public class Item
    {
        public string itemId;
        public GameObject itemPrefabs; // Array to store item prefabs, where itemPrefabs[0] corresponds to item ID 0, itemPrefabs[1] corresponds to item ID 1, and so on.
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("More than one ItemManager instance found.");
            Destroy(gameObject);
        }
    }
}
