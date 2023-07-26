using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCUI : MonoBehaviour
{
    public TextMeshProUGUI npcText;
    public Button cancelButton;

    private void Awake()
    {
        cancelButton.onClick.AddListener(() => {
            GameManager.isPaused = false; //this unpauses game and goes back
        });
    }

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
