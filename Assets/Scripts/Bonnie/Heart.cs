using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{
    public Image heartPrefab;
    public GridLayoutGroup heartGroup;
    public List<Image> hearts;
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

    int currentHealthDisplayed;
    public float lowHealthPercentage = 0.375f;
    void Start()
    {
        Player.instance.OnTakeDamage += UpdateHearts;
        SetTotalHearts();

    }
    public void SetTotalHearts()
    {
        currentHealthDisplayed = Player.instance.maxHealth;
        int heartCount = Mathf.CeilToInt(Player.instance.maxHealth * 0.5f);
        for (int i = 0; i < heartCount; i++)
        {
            var heart = Instantiate(heartPrefab, heartGroup.transform);
            hearts.Add(heart);
            heart.sprite = fullHeart;

            if (Player.instance.maxHealth % 2 != 0)
            {
                if (i == heartCount - 1)
                    heart.sprite = halfHeart;
            }
        }
    }
    private void OnDestroy()
    {
        Player.instance.OnTakeDamage -= UpdateHearts;
    }
    void UpdateHearts()
    {
        int targetHealth = Player.instance.currentHealth;
        // hehe update heart sprites here

        for (int i = 0; i < hearts.Count; i++)
        {
            if ((i + 1) < targetHealth * 0.5f)
            {
                hearts[i].sprite = fullHeart;
            }
            else if ((i + 1) > targetHealth * 0.5f)
            {
                hearts[i].sprite = emptyHeart;
            }
            if (targetHealth % 2 != 0)
            {
                int halfHeartIndex = (int)(targetHealth * 0.5f - 0.5f);
                if (i == halfHeartIndex)
                    hearts[i].sprite = halfHeart;
            }
        }

        //}
        //for (int i = 0; i < hearts.Count; i++)
        //{
        //    if (i < numOfHearts)
        //    {
        //        hearts[i].enabled = true;
        //    }
        //    else
        //    {
        //        hearts[i].enabled = false;
        //    }
        //    // Check if current heart index represents a full heart
        //    if (i * 2 < health - 1)
        //    {
        //        hearts[i].sprite = fullHeart;
        //    }
        //    // Check if current heart index represents a half heart
        //    else if (i * 2 == health - 1)
        //    {
        //        hearts[i].sprite = halfHeart;
        //    }
        //    // Current heart index represents an empty heart
        //    else
        //    {
        //        hearts[i].sprite = emptyHeart;
        //        // Reset rotation if previously shaken
        //    }
        //}

        // loop from currentdisplayed to targethearts
        currentHealthDisplayed = targetHealth;

        bool isLowHealth = (float)currentHealthDisplayed / Player.instance.maxHealth <= lowHealthPercentage;
        if (isLowHealth)
            StartCoroutine(ShakeHearts());
        else
        {
            StopCoroutine(ShakeHearts());
            // Reset heart rotations
            foreach (var heart in hearts)
            {
                heart.transform.rotation = Quaternion.identity;
            }
        }
    }

    IEnumerator ShakeHearts()
    {
        while (true)
        {
            // constantly shake all currently red hearts
            for (int i = 0; i < hearts.Count; i++)
            {
                if (i < Mathf.CeilToInt(currentHealthDisplayed * 0.5f))
                    ShakeHeart(hearts[i].rectTransform);
                else
                    hearts[i].transform.rotation = Quaternion.identity;
            }
            yield return null;
        }
    }
    private void ShakeHeart(RectTransform heartTransform)
    {
        // Apply rotation animation
        float shakeAngle = Mathf.Sin(Time.time * 10f) * 10f; // Adjust the speed and magnitude of the shake
        heartTransform.rotation = Quaternion.Euler(0f, 0f, shakeAngle);
    }
}
