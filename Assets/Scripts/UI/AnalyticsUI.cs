using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnalyticsUI : MonoBehaviour
{
    public Button itemButton;
    public Button chestButton;
    public Button npcButton;
    public Button battleButton;
    public Button cancelButton;
    // Start is called before the first frame update
    void Awake() //get component for buttons
    {
        itemButton.onClick.AddListener(() => { 
            Pause.instance.analyticsPanel.gameObject.SetActive(false);
            Pause.instance.itemPanel.gameObject.SetActive(true);
        });

        chestButton.onClick.AddListener(() => {
            Pause.instance.analyticsPanel.gameObject.SetActive(false);
            Pause.instance.chestPanel.gameObject.SetActive(true);
        });

        npcButton.onClick.AddListener(() => {
            Pause.instance.analyticsPanel.gameObject.SetActive(false);
            Pause.instance.npcPanel.gameObject.SetActive(true);
        });

        battleButton.onClick.AddListener(() => {
            Pause.instance.analyticsPanel.gameObject.SetActive(false);
            Pause.instance.battlePanel.gameObject.SetActive(true);
        });

        cancelButton.onClick.AddListener(() => {
            GameManager.isPaused = false; //this unpauses game and goes back
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
