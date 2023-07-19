using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public float damageIncreaseAmount;

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
