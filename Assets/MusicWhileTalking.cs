using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicWhileTalking : MonoBehaviour
{
    public GameObject objectToMonitor; // Assign the GameObject you want to monitor in the Inspector.
    public AudioSource audioSource; // Assign the AudioSource component in the Inspector.

    private float originalVolume; // Store the original volume of the AudioSource.

    void Start()
    {
        // Store the original volume of the AudioSource.
        originalVolume = audioSource.volume;
    }

    void Update()
    {
        // Check if the GameObject to monitor is active.
        bool isObjectActive = objectToMonitor.activeSelf;

        // Adjust the volume of the AudioSource based on the GameObject's activity.
        if (isObjectActive)
        {
            audioSource.volume = originalVolume * 0.5f; // Lower the volume to half.
        }
        else
        {
            audioSource.volume = originalVolume; // Restore the original volume.
        }
    }
}
