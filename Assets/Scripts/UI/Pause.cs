using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject analyticsPanel;
    void OnEnable()
    {
        // Pause the game when the UI controller is enabled
        settingsPanel.SetActive(false);
        GameManager.instance.PauseGame();
    }

    void OnDisable()
    {
        GameManager.instance.ResumeGame();
    }
}
