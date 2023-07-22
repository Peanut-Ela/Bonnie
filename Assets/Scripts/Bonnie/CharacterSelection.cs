using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    public GameObject[] characters;
    public int selectedCharacter = 0;
    public Animator sceneTransitionAnimator; // Reference to the Animator component on your UI button.

    public void NextCharacter()
    {
        characters[selectedCharacter].SetActive(false);
        selectedCharacter = (selectedCharacter + 1) % characters.Length;
        characters[selectedCharacter].SetActive(true);
    }

    public void PreviousCharacter()
    {
        characters[selectedCharacter].SetActive(false);
        selectedCharacter = (selectedCharacter - 1 + characters.Length) % characters.Length;
        characters[selectedCharacter].SetActive(true);
    }

    public void StartGame()
    {
        StartCoroutine(TransitionAndLoadScene());
    }

    private IEnumerator TransitionAndLoadScene()
    {
        // Trigger the transition animation.
        sceneTransitionAnimator.SetTrigger("StartTransition");

        // Wait for the duration of the transition animation.
        yield return new WaitForSeconds(sceneTransitionAnimator.GetCurrentAnimatorStateInfo(0).length);

        // Load the next scene.
        PlayerPrefs.SetInt("selectedCharacter", selectedCharacter);
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}
