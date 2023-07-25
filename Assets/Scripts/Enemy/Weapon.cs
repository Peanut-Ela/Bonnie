using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

[Serializable]
public struct WeaponProperties
{
    [Header("General Settings")]
    public string weaponId;
    public string weaponName;
    public string weaponSpriteStr;
    public Sprite weaponSprite; //use addressables
    private static string weaponUIPath = "Assets/Art/Weapon/{0}.png";

    [Header("Damage Settings")]
    public float damageIncreaseAmount;

    public static void LoadIcon(string weaponIconName, System.Action<Sprite> onLoaded)
    {
        var asyncHandle = Addressables.LoadAssetAsync<Sprite>(string.Format(weaponUIPath, weaponIconName));
        asyncHandle.Completed += (loadedUI) =>
        {
            onLoaded?.Invoke(loadedUI.Result);
            Addressables.Release(loadedUI);
        };
    }
}

public class Weapon : MonoBehaviour
{
    internal SpriteRenderer sr;
    internal WeaponProperties currentWeapon;

    [Header("General Settings")]
    public string weaponId;
    public string weaponName;
    public Image weaponImage;

    [Header("Damage Settings")]
    public float damageIncreaseAmount;

    public void SetImage(string path)
    {
        WeaponProperties.LoadIcon(path, (Sprite weapon) => weaponImage.sprite = weapon);
    }

    public void SetProperties()
    {
        if (GameAssets.instance != null)

        {
            WeaponProperties weaponProperties = GameAssets.instance.weaponPropertiesList.Find(a => a.weaponId == weaponId);

            // Assign weaponProperties values to the corresponding Weapon properties
            weaponName = weaponProperties.weaponName;
            damageIncreaseAmount = weaponProperties.damageIncreaseAmount;
            SetImage(currentWeapon.weaponSpriteStr);
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
                player.EquipWeapon(this);
                player.IncreaseDamage(damageIncreaseAmount);

                // Destroy the weapon object
                Destroy(gameObject);
            }
        }
    }
}
