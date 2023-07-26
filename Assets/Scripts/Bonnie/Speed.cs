using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[Serializable]
public struct SpeedProperties
{
    [Header("General Settings")]
    public string speedId;
    public string speedName;
    public string speedSpriteStr;
    public Sprite speedSprite; //use addressables
    private static string speedUIPath = "Assets/Art/Weapon/{0}.png";

    [Header("Speed Settings")]
    public float walkSpeedIncreaseAmount;
    public float runSpeedIncreaseAmount;

    public static void LoadIcon(string speedIconName, System.Action<Sprite> onLoaded)
    {
        var asyncHandle = Addressables.LoadAssetAsync<Sprite>(string.Format(speedUIPath, speedIconName));
        asyncHandle.Completed += (loadedUI) =>
        {
            onLoaded?.Invoke(loadedUI.Result);
            Addressables.Release(loadedUI);
        };
    }
}

    public class Speed : MonoBehaviour
{
    internal SpriteRenderer sr;
    internal Rigidbody2D rb;

    [Header("General Settings")]
    public string speedId;
    public string speedName;

    [Header("Speed Settings")]
    public float walkSpeedIncreaseAmount;
    public float runSpeedIncreaseAmount;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        SetProperties();
    }

    public void SetProperties()
    {
        if (GameAssets.instance != null)

        {
            SpeedProperties speedProperties = GameAssets.instance.speedPropertiesList.Find(a => a.speedId == speedId);

            // Assign weaponProperties values to the corresponding Weapon properties
            speedName = speedProperties.speedName;
            walkSpeedIncreaseAmount = speedProperties.walkSpeedIncreaseAmount;
            runSpeedIncreaseAmount = speedProperties.runSpeedIncreaseAmount;
            // Load the weapon sprite and set it to the SpriteRenderer (sr)
            SpeedProperties.LoadIcon(speedProperties.speedSpriteStr, (Sprite speedSprite) =>
            {
                sr.sprite = speedSprite;
            });
        }
    }

    // OnTriggerEnter2D is called when a collider enters the trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Get the Player component from the collided object
            Player player = collision.GetComponent<Player>();

            if (player != null)
            {
                // Equip the weapon and increase the player's damage
                player.DrinkMilk(this);
                player.IncreaseSpeed(walkSpeedIncreaseAmount, runSpeedIncreaseAmount);

                // Destroy the shield object
                Destroy(gameObject);
            }
        }
    }
}
