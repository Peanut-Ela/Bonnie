using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChestCoinUI : MonoBehaviour
{
    public TextMeshProUGUI chestCoinText;

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


        chestCoinText.text = $"{ai.coinEarned}\n\n{ai.chestOpened}";
    }
}
