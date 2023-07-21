using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random; // Add this line

public class Chest : MonoBehaviour
{
    public ChestRandomDropList dropListScript;
    //public List<GameObject> LootList = new List<GameObject>(); // List of coin prefabs
    public ChestRandomDropList dropListAsset;

    public GameObject itemHolder;

    public bool isInRange;
    public KeyCode interactKey;
    public TriggerChest triggerChest;
    public GameObject chestItemPrefab;
    public List<GameObject> chestItems;

    private bool isOpen;

    private void Start()
    {
        triggerChest = GetComponent<TriggerChest>();
        dropListScript = GetComponent<ChestRandomDropList>(); // Get the ChestRandomDropList script attached to this GameObject
    }

    private void Update()
    {
        if (isInRange)
        {
            if (Input.GetKeyDown(interactKey))
            {
                if (isOpen)
                {
                    triggerChest.CloseChest();
                    HideItem();
                }
                else
                {
                    triggerChest.OpenChest();
                    ShowItem();
                }
                isOpen = !isOpen;
            }
        }
    }

    private void HideItem()
    {
        itemHolder.SetActive(false);

        foreach (Transform child in itemHolder.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void ShowItem()
    {
       if (dropListScript.dropList.Count > 0) // Use dropListScript.dropList instead of LootList
        {
            int randomIndex = Random.Range(0, dropListScript.dropList.Count); // Use dropListScript.dropList instead of LootList
            GameObject itemPrefab = dropListScript.dropList[randomIndex]; // Use dropListScript.dropList instead of LootList
            GameObject item = Instantiate(itemPrefab.gameObject, Vector3.zero, Quaternion.identity, itemHolder.transform);
            item.transform.SetParent(itemHolder.transform);
            
            //itemHolder.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = true;
            Debug.Log("Player now in range");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = false;
            Debug.Log("Player now not in range");
        }
    }
}
