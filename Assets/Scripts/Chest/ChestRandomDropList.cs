using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChestRandomDropList : MonoBehaviour
{
    public List<GameObject> dropList = new List<GameObject>();

    public GameObject itemHolder;

    public bool isOpen;
    public Animator animator;

    
    public Animation dropSprite;
    public string dropName;
    public string dropType;
    public int dropChance;
    
    void Start()
    {
        
    }

    void Update()
    {
        if (!isOpen)
        {
            isOpen = true;
            Debug.Log("Chest is now open");
            animator.SetBool("IsOpen", isOpen);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (IsOpen())
            {
                animator.SetTrigger("close");
                HideItem();
            }
            else
            {
                animator.SetTrigger("open");
                ShowItem();
            }
        }
    }

    bool IsOpen()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("ChestOpen");
    }

    void HideItem()
    {

        itemHolder.transform.localScale = Vector3.zero;
        itemHolder.SetActive(false);

        foreach (GameObject child in itemHolder.transform)
        {
            Destroy(child.gameObject);
        }

    }

    public ChestRandomDropList (Animation dropSprite, string dropName, string dropType, int dropChance)
    {
        this.dropSprite = dropSprite;
        this.dropName = dropName;
        this.dropChance = dropChance;

    }
    public void ShowItem()
    {
        //int randomIndex = UnityEngine.Random.Range(0, dropList.Count);
        //GameObject item = dropList[randomIndex];
        //Instantiate(item, itemHolder.transform);
        //itemHolder.SetActive(true);

    /*int totalChance = 0;
    foreach (ChestLootItem lootItem in dropList)
    {
        totalChance += lootItem.dropChance;
    }

    int randomChance = Random.Range(0, totalChance);
    foreach (ChestLootItem lootItem in dropList)
    {
        if (randomChance < lootItem.dropChance)
        {
            GameObject item = Instantiate(lootItem.lootPrefab, itemHolder.transform);
            itemHolder.SetActive(true);
            return;
        }
        randomChance -= lootItem.dropChance;
    }*/
}
}
