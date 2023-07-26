using EnemyStates;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct EnemyProperties
{
    [Header("General Settings")]
    public string enemyId;
    public float health;
    public string enemyColorStr;
    public Color enemyColor;

    [Header("Wander Settings")]
    public float moveSpeed;
    public float idleDurationMin;
    public float idleDurationMax;

    [Header("Charge Settings")]
    public bool canCharge;
    public float chargeCooldown;
    public float detectionRange;

    [Header("Attack Settings")]
    public int damage;
    public float attackRange;
    public float attackDur;

    [Header("Defeated Settings")]
    public float fadeDuration;
    public int fadeIterations;

    public void Parse()
    {
        if (!string.IsNullOrEmpty(enemyColorStr))
        {
            Color color;
            ColorUtility.TryParseHtmlString(enemyColorStr, out color);
            enemyColor = color;
        }
    }
}
public class Enemy : StateMachine
{
    public SpriteRenderer hand;
    public SpriteRenderer outerhand;

    internal SpriteRenderer sr;
    internal Animator animator;
    internal Rigidbody2D rb;
    internal Vector2 moveDirection => (Player.instance.transform.position - transform.position).normalized;
    public bool InAttackRange => Vector2.Distance(Player.instance.transform.position, transform.position) < attackRange;
    #region Animation Keys
    public static readonly int IdleKey = Animator.StringToHash("Idle");
    public static readonly int WalkKey = Animator.StringToHash("Walk");
    public static readonly int ChargeKey = Animator.StringToHash("Charge");
    public static readonly int HurtKey = Animator.StringToHash("Hurt");
    //public static readonly int ShootKey = Animator.StringToHash("Shoot");
    #endregion
    public override BaseState StartState => new IdleState(this);
    public override BaseState DefaultState => new IdleState(this);
    public virtual BaseState AttackState => new IdleState(this);

    [Header("General Settings")]
    public string enemyId;
    public float health;

    [Header("Wander Settings")]
    public float moveSpeed;
    public float idleDurationMin;
    public float idleDurationMax;

    [Header("Charge Settings")]
    public bool canCharge;
    public float chargeCooldown;
    public float detectionRange;

    [Header("Attack Settings")]
    public int damage;
    public float attackRange;
    public float attackDur;

    [Header("Defeated Settings")]
    public float fadeDuration; // Duration of each fade
    public int fadeIterations;

    public void SetProperties()
    {
        if (GameAssets.instance != null)

        {
            EnemyProperties enemyProperties = GameAssets.instance.enemyPropertiesList.Find(a => a.enemyId == enemyId);



            // Assign enemyProperties values to the corresponding Enemy properties
            health = enemyProperties.health;
            sr.color = enemyProperties.enemyColor;
            moveSpeed = enemyProperties.moveSpeed;
            idleDurationMin = enemyProperties.idleDurationMin;
            idleDurationMax = enemyProperties.idleDurationMax;
            canCharge = enemyProperties.canCharge;
            chargeCooldown = enemyProperties.chargeCooldown;
            detectionRange = enemyProperties.detectionRange;
            damage = enemyProperties.damage;
            attackRange = enemyProperties.attackRange;
            attackDur = enemyProperties.attackDur;
            fadeDuration = enemyProperties.fadeDuration;
            fadeIterations = enemyProperties.fadeIterations;
            
        }
    }

    public float Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;

            if (health <= 0)
            {
                StartCoroutine(Defeated());
            }
        }
    }



    protected override void Awake()
    {
        base.Awake();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    protected override void Start()
    {
        base.Start();
        SetProperties();

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


    public void TakeDamage()
    {
        // Subtract the damage from the enemy's health
        if (currentState is HurtState) return;
        //Health -= damage;

        if (Health <= 0)
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
    public void Die()
    {
        StartCoroutine(Defeated());
    }
    public void Despawn()
    {
        Destroy(gameObject);
        AnalyticsManager.instance.OnEnemyDefeated();
    }
    IEnumerator Defeated()
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
            TakeDamage();
        }
    }
}
