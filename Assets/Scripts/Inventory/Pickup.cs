using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{

    public GameObject itemButton;
    public Pickup_Effect effectPrefab;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Check if the 'effect' variable is assigned before using it
            if (effectPrefab != null)
            {
                // The rest of your code remains unchanged
                for (int i = 0; i < Player.instance.items.Length; i++)
                {
                    if (Player.instance.items[i] == 0)
                    { // check whether the slot is EMPTY
                        Instantiate(effectPrefab, transform.position, Quaternion.identity);
                        Player.instance.items[i] = 1; // makes sure that the slot is now considered FULL
                        Instantiate(itemButton, Player.instance.transform, false); // spawn the button so that the player can interact with it
                        Destroy(gameObject);
                        break;
                    }
                }
            }
        }
    }
}
