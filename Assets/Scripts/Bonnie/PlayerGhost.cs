using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGhost : MonoBehaviour
{
    public float ghostFadeDuration = 0.5f;
    float fadeTimer;
    internal SpriteRenderer sr;
    public Gradient ghostGradient;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    public void SetSprite(Sprite sprite)
    {
        sr.sprite = sprite;
    }
    private void Update()
    {
        fadeTimer += Time.deltaTime;
        sr.color = ghostGradient.Evaluate(fadeTimer / ghostFadeDuration);

        if (fadeTimer > ghostFadeDuration)
            Destroy(gameObject);
    }
}
