using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{

    public int loadScene;
    [SerializeField] private GameObject startTransition;

    public void LoadScene()
    {
        startTransition.SetActive(true);
        StartCoroutine(WaitAndExecute(1f));
    }

    IEnumerator WaitAndExecute(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        SceneManager.LoadScene(loadScene, LoadSceneMode.Single);
    }
}
