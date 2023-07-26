using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPCUI : MonoBehaviour, IAnalyticsObserver
{
    public TextMeshProUGUI npcText;

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

        npcText.text = $"{ai.npcInteractions}\n\n{ai.questReceived}\n\n{ai.questDone}";


    }
}
