using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public static Pause instance;

    public SettingsUI settingsPanel;
    public AnalyticsUI analyticsPanel;
    public ItemUI itemPanel;
    public ChestCoinUI chestPanel;
    public NPCUI npcPanel;
    public BattleUI battlePanel;

    private float originalPitch; // To store the original pitch value of the audio

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }
    void OnEnable()
    {
        // Pause the game when the UI controller is enabled
        settingsPanel.gameObject.SetActive(true);
        analyticsPanel.gameObject.SetActive(false);
        itemPanel.gameObject.SetActive(false);
        chestPanel.gameObject.SetActive(false);
        npcPanel.gameObject.SetActive(false);
        battlePanel.gameObject.SetActive(false);


        // Decrease the pitch of the audio
        originalPitch = AudioListener.volume;
        AudioListener.volume = originalPitch * 0.5f;
    }

    void OnDisable()
    {

        // Restore the original pitch of the audio
        AudioListener.volume = originalPitch;
    }
}
