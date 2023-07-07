using EnemyStates;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : StateMachine
{
    public float detectionRange = 10f;
    public float health;
    public int damage;
    public float fadeDuration = 1f; // Duration of each fade
    public int fadeIterations = 3; // Number of fade iterations
    public SpriteRenderer hand;
    public SpriteRenderer outerhand;

    internal SpriteRenderer sr;
    internal Animator animator;
    internal Rigidbody2D rb;
    internal Vector2 moveDirection => (Player.instance.transform.position - transform.position).normalized;

    [Header("Idle Settings")]
    public float idleDurationMin = 1f;
    public float idleDurationMax = 3f;

    [Header("Wander Settings")]
    public float moveSpeed = 5f;

    [Header("Charge Settings")]
    public float chargeCooldown = 2f; // Cooldown duration before charging again
    private bool canCharge = true; // Flag to determine if charging is allowed

    public float attackRange;
    public float attackDur = 0.5f;
    public bool InAttackRange => Vector2.Distance(Player.instance.transform.position, transform.position) < attackRange;
    #region Animation Keys
    public static readonly int IdleKey = Animator.StringToHash("Idle");
    public static readonly int WalkKey = Animator.StringToHash("Walk");
    public static readonly int ChargeKey = Animator.StringToHash("Charge");
    public static readonly int HurtKey = Animator.StringToHash("Hurt");
    #endregion
    public override BaseState StartState => new IdleState(this);
    public override BaseState DefaultState => new IdleState(this);

    public float Health
    {
        set
        {
            health = value;

            if (health <= 0)
            {
                StartCoroutine(Defeated());
            }
        }
        get
        {
            return health;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        sr = GetComponent<SpriteRenderer>();
        animator= GetComponent<Animator>();
        rb= GetComponent<Rigidbody2D>();
    }

    protected override void Update()
    {
        base.Update();

        
            // Check if player is in range
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                Vector2 playerPosition = player.transform.position;
                float distanceToPlayer = Vector2.Distance(transform.position, playerPosition);

                if (distanceToPlayer <= detectionRange && canCharge)
                {
                    // Play ChargeState if player is in range
                    QueueState(new ChargeWindupState(this));

                // Disable charging until cooldown is over
                canCharge = false;
                StartCoroutine(ChargeCooldown());
            }
            }
    }
    private IEnumerator ChargeCooldown()
    {
        yield return new WaitForSeconds(chargeCooldown);

        // Enable charging after cooldown
        canCharge = true;
    }

    //public void TakeDamage(float damage)
    //{
    //    Health -= damage;
    //}

    public void TakeDamage(float damage)
    {
        // Subtract the damage from the enemy's health
        health -= damage;

        if (health <= 0)
        {
            // Enemy is dead, transition to death state
            QueueState(new DeathState(this));
        }
        else
        {
            // Enemy is hurt, transition to hurt state
            QueueState(new HurtState(this));
        }
    }

    public IEnumerator Defeated()
    {
        // Perform fading effect
        for (int i = 0; i < fadeIterations; i++)
        {
            yield return StartCoroutine(FadeOut());
            yield return StartCoroutine(FadeIn());
        }

        // Destroy the enemy object
        Destroy(gameObject);
    }

    public IEnumerator FadeOut()
    {
        float startTime = Time.time;
        Color originalColor = sr.color;
        Color targetColor = new Color(originalColor.r, 0f, 0f, 0f); // Change to red hue
        targetColor = Color.Lerp(Color.black, targetColor, 0.5f); // Increase saturation

        while (Time.time - startTime < fadeDuration)
        {
            float normalizedTime = (Time.time - startTime) / fadeDuration;
            sr.color = Color.Lerp(originalColor, targetColor, normalizedTime);
            yield return null;
        }
    }

    public IEnumerator FadeIn()
    {
        float startTime = Time.time;
        Color originalColor = sr.color;
        Color targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);

        while (Time.time - startTime < fadeDuration)
        {
            float normalizedTime = (Time.time - startTime) / fadeDuration;
            sr.color = Color.Lerp(originalColor, targetColor, normalizedTime);
            yield return null;
        }
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if (other.CompareTag("PlayerAttack"))
        {
            // do check if enemy is not alreadyy in damaged state
            // take damage and change to damaged state
        }
    }
}
