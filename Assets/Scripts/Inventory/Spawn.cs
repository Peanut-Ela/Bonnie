using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    private Transform playerPos;
    public GameObject item;

    // Define the range within which the item can be spawned from the player
    public float spawnRadius;

    private void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    public void SpawnItem()
    {
        Vector2 randomOffset = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 spawnPosition = new Vector3(playerPos.position.x + randomOffset.x, playerPos.position.y + randomOffset.y, 0f);
        Instantiate(item, spawnPosition, Quaternion.identity);
    }
}
