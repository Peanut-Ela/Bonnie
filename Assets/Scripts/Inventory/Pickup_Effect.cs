using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup_Effect : MonoBehaviour
{
    public AudioClip hitSound;
    public AudioSource audioSource;
    public Animator anim;
    void Start()
    {
        audioSource.PlayOneShot(hitSound);
        Invoke("Despawn", anim.GetCurrentAnimatorClipInfo(0).Length);
    }
    void Despawn()
    {
        Destroy(gameObject);
    }
}
