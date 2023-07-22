using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject startTransition;
    [SerializeField] private GameObject endTransition;

    public bool playEnd = false;

    public AudioClip playSound;
    public AudioSource audioSource;



    private void Start()
    {
        if (playEnd == true)
        {
            endTransition.SetActive(true);
        }
    }

    public void PlayGame()
    {
        startTransition.SetActive(true);
        audioSource.PlayOneShot(playSound);
        //black.SetActive(true);
        StartCoroutine(WaitAndExecute(1f));
        //video.SetActive(false);
    }

    public void PlaySound()
    {
        audioSource.PlayOneShot(playSound);
    }

    IEnumerator WaitAndExecute(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
