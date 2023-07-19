using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Explode : MonoBehaviour
{
    public AudioClip hitSound;
    public AudioSource audioSource;
    public Animator anim;

    public int damage;
    public float radius;
    // Start is called before the first frame update
    void Start()
    {
        audioSource.PlayOneShot(hitSound);
        Invoke("Despawn", anim.GetCurrentAnimatorClipInfo(0).Length);
        //Invoke("Despawn", hitSound.length);

        // Check if the enemy has collided with the player during the attack sequence
        Collider2D collision = Physics2D.OverlapCircle(transform.position, radius, LayerMask.GetMask("Player"));
        if (collision != null)
        {
            // Perform explode effect on collision with the player
            Player.instance.ApplyKnockback((Player.instance.transform.position - transform.position).normalized);
            Player.instance.TakeDamage(damage);
        }

        // Deal damage to the player



    }
    void Despawn()
    {
        Destroy(gameObject);
    }
}
