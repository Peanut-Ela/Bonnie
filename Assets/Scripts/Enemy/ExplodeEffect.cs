using UnityEngine;

public class ExplodeEffect : MonoBehaviour
{
    public float explosionForce = 10f;
    public float explosionRadius = 5f;
    public Animator animator;
    

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private CircleCollider2D col;

    public static readonly int ExplodeKey = Animator.StringToHash("Explode");

    private void Start()
    {
        Explode();
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<CircleCollider2D>();
    }

    public void Explode()
    {
        //enemy.animator.PlayInFixedTime(Enemy.HurtKey);

        // Play explosion sound if available
        

        // Apply explosion force to nearby objects
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D collider in colliders)
        {
            Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 direction = (collider.transform.position - transform.position).normalized;
                rb.AddForce(direction * explosionForce, ForceMode2D.Impulse);
            }
        }


        // Destroy the explosion effect game object after the animation finishes
        // Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
    }
}
