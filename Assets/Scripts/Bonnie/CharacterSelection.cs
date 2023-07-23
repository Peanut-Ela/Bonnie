using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    public GameObject[] characters;
    public int selectedCharacter = 0;
    [SerializeField] private GameObject startTransition;

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
        //PlayerPrefs.SetInt("selectedCharacter", selectedCharacter);
        startTransition.SetActive(true);
        StartCoroutine(WaitAndExecute(1f));
    }

    IEnumerator WaitAndExecute(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        GameAssets.selectedCharacter = selectedCharacter;
        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }

}