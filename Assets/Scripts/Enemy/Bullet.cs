using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float delay;
    public float distance;

    public float bulletSpeed = 5f;
    public float rotationSpeed = 200f;
    public float maxDistance = 10f;
    public Transform target;
    public AudioSource audioSource;
    public AudioClip fireSound;
    public AudioClip hitSound;
    public GameObject explosionPrefab;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private CircleCollider2D col;
    private TrailRenderer tr;
    private float travelledDistance = 0f;
    private bool exploded = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<CircleCollider2D>();
        tr = GetComponent<TrailRenderer>();
    }

    private void Start()
    {
        if (audioSource != null && fireSound != null)
        {
            audioSource.PlayOneShot(fireSound);
        }
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            float rotateAmount = Vector3.Cross(direction, transform.up).z;
            rb.angularVelocity = -rotateAmount * rotationSpeed;

            rb.velocity = transform.up * bulletSpeed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }

        travelledDistance += bulletSpeed * Time.fixedDeltaTime;
        if (travelledDistance > maxDistance)
        {
            DestroyBullet();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Handle collision with the player
            if (audioSource != null && hitSound != null)
            {
                Debug.Log("Playing hit sound");
                audioSource.PlayOneShot(hitSound);
            }
            if (!exploded)
            {
                Explode();
                exploded = true;
            }
            DestroyBullet();
        }
    }

    private void Explode()
    {
        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(explosion, explosion.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        }
    }

    private void DestroyBullet()
    {
        sr.enabled = false;
        col.enabled = false;
        tr.enabled = false;
        target = null;
        Destroy(gameObject);
    }
}
