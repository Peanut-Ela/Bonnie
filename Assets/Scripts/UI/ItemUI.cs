using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemUI : MonoBehaviour
{

    public TextMeshProUGUI itemText;

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
        // Update the UI texts with new analytics data
        itemText.text = $"{ai.noOfItemsPickedUp}\n\n{ai.noOfItemsDropped}\n\n{ai.noOfHealthPotionsUsed}\n\n{ai.noOfWeaponsUsed}\n\n{ai.noOfShieldsUsed}\n\n{ai.noOfMilkBottlesUsed}";
    }

}
