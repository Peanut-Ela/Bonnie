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

public interface IAnalyticsObserver
{
    void UpdateAnalytics();
}

public class AnalyticsManager : MonoBehaviour
{
    private List<IAnalyticsObserver> observers = new List<IAnalyticsObserver>();
    public static AnalyticsManager instance;
    //public AnalyticsStats analyticsStats;

   

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

    public void Subscribe(IAnalyticsObserver observer)
    {
        if (!observers.Contains(observer))
            observers.Add(observer);
    }

    // Unsubscribe an Observer
    public void Unsubscribe(IAnalyticsObserver observer)
    {
        observers.Remove(observer);
    }

    // Notify all Observers when the analytics data changes
    private void NotifyObservers()
    {
        foreach (var observer in observers)
        {
            observer.UpdateAnalytics();
        }
    }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
            SetProperties();
            //items = new int[playerStats.items.Length];

            // Subscribe the UI controllers to the AnalyticsManager
            YourUIController[] uiControllers = FindObjectsOfType<YourUIController>();
            foreach (var uiController in uiControllers)
            {
                Subscribe(uiController);
            }
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

    public class YourUIController : MonoBehaviour, IAnalyticsObserver
    {
        public TextMeshProUGUI itemText;
        public TextMeshProUGUI chestCoinText;
        public TextMeshProUGUI npcText;
        public TextMeshProUGUI battleText;

        // Called when the analytics data changes in the AnalyticsManager
        public void UpdateAnalytics()
        {
            // Update the UI texts with new analytics data
            itemText.text = instance.noOfItemsPickedUp.ToString() + "\n\n" + instance.noOfItemsDropped.ToString() + "\n\n" + instance.noOfHealthPotionsUsed.ToString() + "\n\n" + instance.noOfWeaponsUsed.ToString() + "\n\n" + instance.noOfShieldsUsed.ToString() + "\n\n" + instance.noOfMilkBottlesUsed.ToString();

            chestCoinText.text = instance.coinEarned.ToString() + "\n\n" + instance.chestOpened.ToString();

            npcText.text = instance.npcInteractions.ToString() + "\n\n" + instance.questReceived.ToString() + "\n\n" + instance.questDone.ToString();

            battleText.text = instance.noOfHitsTaken.ToString() + "\n\n" + instance.enemyDefeatCount.ToString() + "\n\n" + instance.damageRecieved.ToString() + "\n\n" + instance.timeTakenToCompleteLevel.ToString() + "\n\n" + instance.noOfLevelsCompleted.ToString();
            
        }

        // Other code...
    }
    private void Update()
    {

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