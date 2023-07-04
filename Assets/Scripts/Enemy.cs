using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health;
    public float fadeDuration = 1f; // Duration of each fade
    public int fadeIterations = 3; // Number of fade iterations
    private SpriteRenderer spriteRenderer;

    public float Health
    {
        set
        {
            health = value;

            if (health <= 0)
            {
                StartCoroutine(Defeated());
            }
        }
        get
        {
            return health;
        }
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
    }

    private IEnumerator Defeated()
    {
        // Perform fading effect
        for (int i = 0; i < fadeIterations; i++)
        {
            yield return StartCoroutine(FadeOut());
            yield return StartCoroutine(FadeIn());
        }

        // Destroy the enemy object
        Destroy(gameObject);
    }

    private IEnumerator FadeOut()
    {
        float startTime = Time.time;
        Color originalColor = spriteRenderer.color;
        Color targetColor = new Color(originalColor.r, 0f, 0f, 0f); // Change to red hue
        targetColor = Color.Lerp(Color.black, targetColor, 0.5f); // Increase saturation

        while (Time.time - startTime < fadeDuration)
        {
            float normalizedTime = (Time.time - startTime) / fadeDuration;
            spriteRenderer.color = Color.Lerp(originalColor, targetColor, normalizedTime);
            yield return null;
        }
    }

    private IEnumerator FadeIn()
    {
        float startTime = Time.time;
        Color originalColor = spriteRenderer.color;
        Color targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);

        while (Time.time - startTime < fadeDuration)
        {
            float normalizedTime = (Time.time - startTime) / fadeDuration;
            spriteRenderer.color = Color.Lerp(originalColor, targetColor, normalizedTime);
            yield return null;
        }
    }
}
