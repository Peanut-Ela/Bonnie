using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class YourUIController : MonoBehaviour, IAnalyticsObserver
{
    public TextMeshProUGUI itemText;
    public TextMeshProUGUI chestCoinText;
    public TextMeshProUGUI npcText;
    public TextMeshProUGUI battleText;

    

    private void OnEnable()
    {
        // Subscribe to the AnalyticsManager
        AnalyticsManager.instance.Subscribe(this);

        // Update the UI texts with initial analytics data
        UpdateAnalytics();

        // Pause the game when the UI controller is enabled
        GameManager.instance.PauseGame();
    }

    private void OnDisable()
    {
        // Unsubscribe from the AnalyticsManager
        AnalyticsManager.instance.Unsubscribe(this);
    }

    

    // Called when the analytics data changes in the AnalyticsManager
    public void UpdateAnalytics()
    {

        var ai = AnalyticsManager.instance;
        // Update the UI texts with new analytics data
        itemText.text = $"{ai.noOfItemsPickedUp}\n\n{ai.noOfItemsDropped}\n\n{ai.noOfHealthPotionsUsed}\n\n{ai.noOfWeaponsUsed}\n\n{ai.noOfShieldsUsed}\n\n{ai.noOfMilkBottlesUsed}";

        chestCoinText.text = $"{ai.coinEarned}\n\n{ai.chestOpened}";

        npcText.text = $"{ai.npcInteractions}\n\n{ai.questReceived}\n\n{ai.questDone}";

        battleText.text = $"{ai.noOfHitsTaken}\n\n{ai.enemyDefeatCount}\n\n{ai.damageRecieved}\n\n{ai.timeTakenToCompleteLevel}\n\n{ai.noOfLevelsCompleted}";


    }
}
