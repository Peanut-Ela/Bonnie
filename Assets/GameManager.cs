using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // always use instance = this;

    public GameObject deathScreen;

    public Button restartButton;

    private bool isGamePaused;

    public static bool isPaused
    {
        get => instance.isGamePaused;
        set
        {
            instance.isGamePaused = value;
            Time.timeScale = value ? 0 : 1;
            Pause.instance.gameObject.SetActive(isPaused);
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
            //items = new int[playerStats.items.Length];
        }
        else
        {
            Destroy(gameObject); // Destroy the duplicate player if it exists
            return;
        }

        restartButton.onClick.AddListener(() => {
            SceneManager.LoadScene(0, LoadSceneMode.Single);
            Time.timeScale = 1;
        });

    }

    private void Start()
    {
        Player.instance.OnDeath += EndGame;
    }

    void EndGame()
    {
        deathScreen.SetActive(true);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        { 
            isPaused = !isPaused;
        }
    }
}
