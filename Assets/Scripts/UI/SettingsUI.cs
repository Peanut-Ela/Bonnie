using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    public Button aiButton;

    public Button cancelButton;

        

    void Awake() //get component for buttons
    {
        aiButton.onClick.AddListener(() =>
        {
            Pause.instance.settingsPanel.gameObject.SetActive(false);
            Pause.instance.analyticsPanel.gameObject.SetActive(true);
        });

        cancelButton.onClick.AddListener(() => {
            GameManager.isPaused = false; //this unpauses game and goes back
        });
    }

}
