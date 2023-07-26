using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPCUI : MonoBehaviour
{
    public TextMeshProUGUI npcText;

    private void OnEnable()
    {

        // Update the UI texts with initial analytics data
        UpdateAnalytics();

    }

    private void OnDisable()
    {
    }



    // Called when the analytics data changes in the AnalyticsManager
    public void UpdateAnalytics()
    {

        var ai = AnalyticsManager.instance;

        npcText.text = $"{ai.npcInteractions}\n\n{ai.questReceived}\n\n{ai.questDone}";


    }
}
