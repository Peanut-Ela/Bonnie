using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]

public struct AnalyticsStats
{
    [Header("Player Analytics")]
    public int playerId;


    [Header("Item Analytics")]
    public int noOfItemsPickedUp;
    public int noOfItemsDropped;
    public int noOfHealthPotionsUsed;
    public int noOfWeaponsUsed;
    public int noOfShieldsUsed;
    public int noOfMilkBottlesUsed;

    [Header("Chest & Coin Analytics")]
    public int coinEarned;
    public int chestOpened;

    [Header("NPC Analytics")]
    public int npcInteractions;
    public int questReceived;
    public int questDone;

    [Header("Battle Analytics")]
    public int noOfHitsTaken;
    public int enemyDefeatCount;
    public float damageRecieved;
    public float timeTakenToCompleteLevel;
    public int noOfLevelsCompleted;

}

public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager instance;
    //public AnalyticsStats analyticsStats;

    public TextMeshProUGUI itemText;
    public TextMeshProUGUI chestCoinText;
    public TextMeshProUGUI npcText;
    public TextMeshProUGUI battleText;

    [Header("Player Analytics")]
    public int playerId;


    [Header("Item Analytics")]
    public int noOfItemsPickedUp;
    public int noOfItemsDropped;
    public int noOfHealthPotionsUsed;
    public int noOfWeaponsUsed;
    public int noOfShieldsUsed;
    public int noOfMilkBottlesUsed;

    [Header("Chest & Coin Analytics")]
    public int coinEarned;
    public int chestOpened;

    [Header("NPC Analytics")]
    public int npcInteractions;
    public int questReceived;
    public int questDone;

    [Header("Battle Analytics")]
    public int noOfHitsTaken;
    public int enemyDefeatCount;
    public float damageRecieved;
    public float timeTakenToCompleteLevel;
    public int noOfLevelsCompleted;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
            SetProperties();
            //items = new int[playerStats.items.Length];
        }
        else
        {
            Destroy(gameObject); // Destroy the duplicate manager if it exists
            return;
        }
    }

    public void SetProperties()
    {
        if (GameAssets.instance != null)

        {
            AnalyticsStats statsProperties = GameAssets.instance.analyticsStatsList.Find(a => a.playerId == playerId);

            noOfItemsPickedUp = statsProperties.noOfItemsPickedUp;
            noOfItemsDropped = statsProperties.noOfItemsDropped;
            noOfHealthPotionsUsed = statsProperties.noOfHealthPotionsUsed;
            noOfWeaponsUsed = statsProperties.noOfWeaponsUsed;
            noOfShieldsUsed = statsProperties.noOfShieldsUsed;
            noOfMilkBottlesUsed = statsProperties.noOfMilkBottlesUsed;

            coinEarned = statsProperties.coinEarned;
            chestOpened = statsProperties.chestOpened;

            npcInteractions = statsProperties.npcInteractions;
            questReceived = statsProperties.questReceived;
            questDone = statsProperties.questDone;

            noOfHitsTaken = statsProperties.noOfHitsTaken;
            enemyDefeatCount = statsProperties.enemyDefeatCount;
            damageRecieved = statsProperties.damageRecieved;
            timeTakenToCompleteLevel = statsProperties.timeTakenToCompleteLevel;
            noOfLevelsCompleted = statsProperties.noOfLevelsCompleted;
        }
    }
    // Initialize the analytics data
    private void Update()
    {
        itemText.text = noOfItemsPickedUp.ToString() + "\n\n" + noOfItemsDropped.ToString() + "\n\n" + noOfHealthPotionsUsed.ToString() + "\n\n" + noOfWeaponsUsed.ToString() + "\n\n" + noOfShieldsUsed.ToString() + "\n\n" + noOfMilkBottlesUsed.ToString();

        chestCoinText.text = coinEarned.ToString() + "\n\n" + chestOpened.ToString();

        npcText.text = npcInteractions.ToString() + "\n\n" + questReceived.ToString() + "\n\n" + questDone.ToString();

        battleText.text = noOfHitsTaken.ToString() + "\n\n" + enemyDefeatCount.ToString() + "\n\n" + damageRecieved.ToString() + "\n\n" + timeTakenToCompleteLevel.ToString() + "\n\n" + noOfLevelsCompleted.ToString();

    }

    public void OnItemPickUp()
    {
        noOfItemsPickedUp++;
    }

    public void OnItemsDropped()
    {
        noOfItemsDropped++;
    }

    // Call this method whenever an item is used (health potion, weapon, shield, milk bottle, etc.)
    public void OnItemUsed(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.HealthPotion:
                noOfHealthPotionsUsed++;
                break;
            case ItemType.Weapon:
                noOfWeaponsUsed++;
                break;
            case ItemType.Shield:
                noOfShieldsUsed++;
                break;
            case ItemType.MilkBottle:
                noOfMilkBottlesUsed++;
                break;
        }
    }

    // Call this method whenever a chest is opened
    public void OnChestOpened()
    {
        chestOpened++;
    }

    // Call this method whenever a coin is earned
    public void OnCoinEarned(int coinAmount)
    {
        coinEarned += coinAmount;
    }

    // Call this method whenever an NPC is interacted with
    public void OnNPCInteracted()
    {
        npcInteractions++;
    }

    // Call this method whenever the player takes a hit
    public void OnHitTaken()
    {
        noOfHitsTaken++;
    }

    // Call this method whenever a quest is received
    public void OnQuestReceived()
    {
        questReceived++;
    }

    // Call this method whenever a quest is completed
    public void OnQuestDone()
    {
        questDone++;
    }

    // Call this method whenever an enemy is defeated
    public void OnEnemyDefeated()
    {
        enemyDefeatCount++;
    }

    // Call this method whenever the player receives damage
    public void OnDamageReceived(float damageAmount)
    {
        damageRecieved += damageAmount;
    }

    // Call this method whenever a level is completed
    public void OnLevelCompleted(float timeTaken)
    {
        timeTakenToCompleteLevel = timeTaken;
        noOfLevelsCompleted++;
    }
}