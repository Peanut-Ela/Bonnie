using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{
    public int health;
    public int numOfHearts;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

    private RectTransform[] heartTransforms; // Array to store the RectTransform components
    private Quaternion[] initialRotations; // Array to store the initial rotations

    void Start()
    {
        // Initialize arrays
        heartTransforms = new RectTransform[hearts.Length];
        initialRotations = new Quaternion[hearts.Length];

        for (int i = 0; i < hearts.Length; i++)
        {
            // Get the RectTransform component and initial rotation
            heartTransforms[i] = hearts[i].GetComponent<RectTransform>();
            initialRotations[i] = heartTransforms[i].rotation;
        }
    }

    void Update()
    {
        if (health > numOfHearts * 2)
        {
            health = numOfHearts * 2;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
            // Check if current heart index represents a full heart
            if (i * 2 < health - 1)
            {
                hearts[i].sprite = fullHeart;
                // Reset rotation if previously shaken
                heartTransforms[i].rotation = initialRotations[i];
            }
            // Check if current heart index represents a half heart
            else if (i * 2 == health - 1)
            {
                hearts[i].sprite = halfHeart;
                // Apply shake animation
                ShakeHeart(heartTransforms[i]);
            }
            // Current heart index represents an empty heart
            else
            {
                hearts[i].sprite = emptyHeart;
                // Reset rotation if previously shaken
                heartTransforms[i].rotation = initialRotations[i];
            }


        }
    }

    private void ShakeHeart(RectTransform heartTransform)
    {
        // Apply rotation animation
        float shakeAngle = Mathf.Sin(Time.time * 10f) * 10f; // Adjust the speed and magnitude of the shake
        heartTransform.rotation = initialRotations[0] * Quaternion.Euler(0f, 0f, shakeAngle);
    }
}
