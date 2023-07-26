using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[Serializable]
public struct ShieldProperties
{
    [Header("General Settings")]
    public string shieldId;
    public string shieldName;
    public string shieldSpriteStr;
    public Sprite shieldSprite; //use addressables
    private static string shieldUIPath = "Assets/Art/Weapon/{0}.png";

    [Header("Defense Settings")]
    public float defenseIncreaseAmount;

    public static void LoadIcon(string shieldIconName, System.Action<Sprite> onLoaded)
    {
        var asyncHandle = Addressables.LoadAssetAsync<Sprite>(string.Format(shieldUIPath, shieldIconName));
        asyncHandle.Completed += (loadedUI) =>
        {
            onLoaded?.Invoke(loadedUI.Result);
            Addressables.Release(loadedUI);
        };
    }
}

public class Shield : StateMachine
{

    internal SpriteRenderer sr;
    internal Animator animator;
    internal Rigidbody2D rb;
    internal ShieldProperties currentshield;

    [Header("General Settings")]
    public string shieldId;
    public string shieldName;

    [Header("Defense Settings")]
    public float defenseIncreaseAmount;


    protected override void Awake()
    {
        base.Awake();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    protected override void Start()
    {
        base.Start();
        SetProperties();
    }

    public void SetProperties()
    {
        if (GameAssets.instance != null)

        {
            ShieldProperties shieldProperties = GameAssets.instance.shieldPropertiesList.Find(a => a.shieldId == shieldId);

            // Assign shieldProperties values to the corresponding shield properties
            shieldName = shieldProperties.shieldName;
            defenseIncreaseAmount = shieldProperties.defenseIncreaseAmount;
            // Load the shield sprite and set it to the SpriteRenderer (sr)
            ShieldProperties.LoadIcon(shieldProperties.shieldSpriteStr, (Sprite shieldSprite) =>
            {
                sr.sprite = shieldSprite;
            });
        }
    }

    // OnTriggerEnter2D is called when a collider enters the trigger
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Get the Player component from the collided object
            Player player = collision.GetComponent<Player>();

            if (player != null)
            {
                // Equip the shield and increase the player's defense
                player.EquipShield(this);
                player.IncreaseDefense(defenseIncreaseAmount);

                // Destroy the shield object
                Destroy(gameObject);
            }
        }
    }
}
