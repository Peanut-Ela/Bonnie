using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public static Pause instance;

    public GameObject settingsPanel;
    public GameObject analyticsPanel;
    public GameObject itemPanel;
    public GameObject chestPanel;
    public GameObject npcPanel;
    public GameObject battlePanel;

    private float originalPitch; // To store the original pitch value of the audio
    void OnEnable()
    {
        // Pause the game when the UI controller is enabled
        settingsPanel.SetActive(true);
        analyticsPanel.SetActive(false);
        itemPanel.SetActive(false);
        chestPanel.SetActive(false);
        npcPanel.SetActive(false);
        battlePanel.SetActive(false);
        GameManager.instance.PauseGame();

        // Decrease the pitch of the audio
        originalPitch = AudioListener.volume;
        AudioListener.volume = originalPitch * 0.5f;
    }

    void OnDisable()
    {
        GameManager.instance.ResumeGame();

        // Restore the original pitch of the audio
        AudioListener.volume = originalPitch;
    }
}
