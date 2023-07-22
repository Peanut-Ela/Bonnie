using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public static Spawn spawnInstance;
    public List<GameObject> itemPrefabs = new List<GameObject>(); // List of item prefabs
    public float spawnRadius; // Define the range within which the item can be spawned from the player

    private void Start()
    {
    }

    public void SpawnItem(int itemID)
    {
        if (itemPrefabs.Count == 0)
        {
            Debug.LogError("Item prefabs list is empty in the Spawn script.");
            return;
        }

        // Get a random item prefab from the list
        GameObject randomItemPrefab = itemPrefabs[Random.Range(0, itemPrefabs.Count)];

        if (randomItemPrefab == null)
        {
            Debug.LogError("Random item prefab is null in the Spawn script.");
            return;
        }

        Vector2 randomOffset = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 spawnPosition = new Vector3(Player.instance.transform.position.x + randomOffset.x, Player.instance.transform.position.y + randomOffset.y, 0f);

        // Instantiate the item prefab and set its itemID
        GameObject newItem = Instantiate(randomItemPrefab, spawnPosition, Quaternion.identity);
        Pickup pickupScript = newItem.GetComponent<Pickup>();
        if (pickupScript != null)
        {
            pickupScript.itemID = itemID;
        }
    }
}
