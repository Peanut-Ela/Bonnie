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
    [SerializeField] private GameObject startTransition;

    

    // Add scene indices for the three buttons
    public int button1SceneIndex;
    public int button2SceneIndex;
    public int button3SceneIndex;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnDestroy()
    {
        objectToActivate.SetActive(false);
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

    // Function to be called when the button1 is clicked
    public void Button1Clicked()
    {
        //SavePlayerPosition();
        SceneManager.LoadScene(button1SceneIndex);
    }

    // Function to be called when the button2 is clicked
    public void Button2Clicked()
    {
        //SavePlayerPosition();
        SceneManager.LoadScene(button2SceneIndex);
    }

    // Function to be called when the button3 is clicked
    public void Button3Clicked()
    {
        //SavePlayerPosition();
        SceneManager.LoadScene(button3SceneIndex);
    }

    
}
