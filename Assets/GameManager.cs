using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject settingsPanel;
    public GameObject analyticsPanel;

    internal bool isGamePaused;

    public void PauseGame()
    {
        // Add code here to pause the game (if needed)
        isGamePaused = true;
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        // Add code here to resume the game (if needed)
        isGamePaused = false;
        Time.timeScale = 1f;
    }
}
