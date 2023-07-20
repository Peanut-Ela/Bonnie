using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random; // Add this line

public class Chest : MonoBehaviour
{
    public List<GameObject> LootList = new List<GameObject>(); // List of coin prefabs

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
        if (LootList.Count > 0)
        {
            int randomIndex = Random.Range(0, LootList.Count);
            GameObject itemPrefab = LootList[randomIndex];
            GameObject item = Instantiate(itemPrefab, itemHolder.transform);
            itemHolder.SetActive(true);
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
