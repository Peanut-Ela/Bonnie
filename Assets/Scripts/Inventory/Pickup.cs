using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

[Serializable]

public struct ItemProperties
{
    [Header("ID Settings")]
    public int itemId; // Assign a unique ID for each pickup object in the Inspector
    public string itemName;

    [Header("Sprite Settings")]
    public string itemSpriteStr;
    public Sprite itemSprite; // Assign the sprite of the item to this variable in the Inspector
    private static string itemUIPath = "Assets/Art/Objects/Items/{0}.png";

    [Header("GameObject Settings")]
    public string itemCategoryStr;
    public GameObject itemCategory;
    public List<GameObject> itemPrefabs;
    private static string itemGameObjectPath = "Assets/Prefab/Inventory/{0}.prefab"; // Replace with the correct path to your prefabs

    // Load the item GameObject
    public static void LoadGameObject(string itemGameObjectName, System.Action<GameObject> onLoaded)
    {
        var asyncHandle = Addressables.LoadAssetAsync<GameObject>(string.Format(itemGameObjectPath, itemGameObjectName));
        asyncHandle.Completed += (loadedGameObject) =>
        {
            onLoaded?.Invoke(loadedGameObject.Result);
            Addressables.Release(loadedGameObject);
        };
    }

    //public GameObject itemCategory;

    public static void LoadIcon(string itemIconName, System.Action<Sprite> onLoaded)
    {
        var asyncHandle = Addressables.LoadAssetAsync<Sprite>(string.Format(itemUIPath, itemIconName));
        asyncHandle.Completed += (loadedUI) =>
        {
            onLoaded?.Invoke(loadedUI.Result);
            Addressables.Release(loadedUI);
        };
    }
}

public class Pickup : MonoBehaviour
{
    internal SpriteRenderer sr;
    internal Rigidbody2D rb;

    public delegate void OnItemPickupEvent(Sprite itemSprite, int itemID);
    public static event OnItemPickupEvent OnItemPickup;

    [Header("ID Settings")]
    public int itemId; // Assign a unique ID for each pickup object in the Inspector
    public string itemName;

    [Header("Sprite Settings")]
    public Sprite itemSprite; // Assign the sprite of the item to this variable in the Inspector
    public GameObject itemCategory;
    public Pickup_Effect effectPrefab;

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
            ItemProperties itemProperties = GameAssets.instance.itemPropertiesList.Find(a => a.itemId == itemId);

            // Assign itemProperties values to the corresponding item properties
            itemName = itemProperties.itemName;
            // Load the item sprite and set it to the SpriteRenderer (sr)
            ItemProperties.LoadIcon(itemProperties.itemSpriteStr, (Sprite itemSprite) =>
            {
                sr.sprite = itemSprite;
            });

            // Load the item GameObject and assign it directly to itemCategory
            ItemProperties.LoadGameObject(itemProperties.itemCategoryStr, (GameObject itemGameObject) =>
            {
                itemCategory = itemGameObject;
            });
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Trigger the event when an item is picked up
            if (effectPrefab != null)
            {
                // The rest of your code remains unchanged
                for (int i = 0; i < Player.instance.items.Length; i++)
                {
                    if (Player.instance.items[i] == 0)
                    { // check whether the slot is EMPTY
                        Instantiate(effectPrefab, transform.position, Quaternion.identity);
                        Player.instance.items[i] = 1; // makes sure that the slot is now considered FULL

                        // Trigger the event passing the item sprite and itemID
                        OnItemPickup?.Invoke(sr.sprite, itemId);

                        // Instantiate(itemButton, Player.instance.transform, false); // spawn the button so that the player can interact with it
                        Destroy(gameObject);
                        break;
                    }
                }
            }
        }
    }
}
