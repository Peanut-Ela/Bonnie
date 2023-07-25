using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]

public struct AnalyticsStats
{
    [Header("Player Analytics")]
    public int playerId;


    [Header("Item Analytics")]
    public int noOfHealthPotionsUsed;
    public int noOfWeaponsUsed;
    public int noOfShieldsUsed;
    public int noOfMilkBottlesUsed;

    [Header("Chest & Coin Analytics")]
    public int coinEarned;
    public int chestOpened;

    [Header("NPC Analytics")]
    public int npcInteractions;
    public int noOfHitsTaken;

    [Header("Quest Analytics")]
    public int questReceived;
    public int questDone;

    [Header("Enemy Analytics")]
    public int enemyDefeatCount;
    public float damageRecieved;

    [Header("Level Analytics")]
    public float timeTakenToCompleteLevel;
    public int noOfLevelsCompleted;

}

public class AnalyticsManager : MonoBehaviour
{
    public AnalyticsStats analyticsStats;

    // Initialize the analytics data
    private void Start()
    {
        analyticsStats = new AnalyticsStats();
        // You can set the playerId and other initial values here
    }

    // Call this method whenever an item is used (health potion, weapon, shield, milk bottle, etc.)
    public void OnItemUsed(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.HealthPotion:
                analyticsStats.noOfHealthPotionsUsed++;
                break;
            case ItemType.Weapon:
                analyticsStats.noOfWeaponsUsed++;
                break;
            case ItemType.Shield:
                analyticsStats.noOfShieldsUsed++;
                break;
            case ItemType.MilkBottle:
                analyticsStats.noOfMilkBottlesUsed++;
                break;
        }
    }

    // Call this method whenever a chest is opened
    public void OnChestOpened()
    {
        analyticsStats.chestOpened++;
    }

    // Call this method whenever a coin is earned
    public void OnCoinEarned(int coinAmount)
    {
        analyticsStats.coinEarned += coinAmount;
    }

    // Call this method whenever an NPC is interacted with
    public void OnNPCInteracted()
    {
        analyticsStats.npcInteractions++;
    }

    // Call this method whenever the player takes a hit
    public void OnHitTaken()
    {
        analyticsStats.noOfHitsTaken++;
    }

    // Call this method whenever a quest is received
    public void OnQuestReceived()
    {
        analyticsStats.questReceived++;
    }

    // Call this method whenever a quest is completed
    public void OnQuestDone()
    {
        analyticsStats.questDone++;
    }

    // Call this method whenever an enemy is defeated
    public void OnEnemyDefeated()
    {
        analyticsStats.enemyDefeatCount++;
    }

    // Call this method whenever the player receives damage
    public void OnDamageReceived(float damageAmount)
    {
        analyticsStats.damageRecieved += damageAmount;
    }

    // Call this method whenever a level is completed
    public void OnLevelCompleted(float timeTaken)
    {
        analyticsStats.timeTakenToCompleteLevel = timeTaken;
        analyticsStats.noOfLevelsCompleted++;
    }
}