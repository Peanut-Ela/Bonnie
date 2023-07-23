using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boat : MonoBehaviour
{
    public GameObject objectToActivate;
    public float interactionDistance = 3f;
    public AudioClip interactionSound; // Assign the sound clip in the Inspector
    private AudioSource audioSource;
    private bool isPlayerNearby = false;

    // Add scene indices for the three buttons
    public int button1SceneIndex;
    public int button2SceneIndex;
    public int button3SceneIndex;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isPlayerNearby)
        {
            ToggleObjectActivation();
        }
    }

    void FixedUpdate()
    {
        CheckPlayerDistance();
    }

    void CheckPlayerDistance()
    {
        if (Player.instance != null)
        {
            float distanceToPlayer = Vector3.Distance(Player.instance.transform.position, transform.position);
            if (distanceToPlayer <= interactionDistance)
            {
                isPlayerNearby = true;
            }
            else
            {
                isPlayerNearby = false;
                DeactivateObject();
            }
        }
    }

    void ToggleObjectActivation()
    {
        if (objectToActivate.activeSelf)
        {
            DeactivateObject();
        }
        else
        {
            ActivateObject();
        }
    }

    void ActivateObject()
    {
        objectToActivate.SetActive(true);
        if (interactionSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(interactionSound);
        }
    }

    void DeactivateObject()
    {
        objectToActivate.SetActive(false);
    }

    // Load scenes when buttons are clicked
    public void LoadSceneButton1()
    {
        SceneManager.LoadScene(button1SceneIndex, LoadSceneMode.Single);
    }

    public void LoadSceneButton2()
    {
        SceneManager.LoadScene(button2SceneIndex, LoadSceneMode.Single);
    }

    public void LoadSceneButton3()
    {
        SceneManager.LoadScene(button3SceneIndex, LoadSceneMode.Single);
    }
}
