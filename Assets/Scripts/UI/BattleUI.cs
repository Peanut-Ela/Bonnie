using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleUI : MonoBehaviour
{
    public TextMeshProUGUI battleText;

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

        battleText.text = $"{ai.noOfHitsTaken}\n\n{ai.enemyDefeatCount}\n\n{ai.damageRecieved}\n\n{ai.timeTakenToCompleteLevel}\n\n{ai.noOfLevelsCompleted}";
    }
}
