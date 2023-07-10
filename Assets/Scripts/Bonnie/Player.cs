using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerStates;
using Bonnie.CharacterStats;

public class Player : StateMachine
{
    public static Player instance;
    internal Rigidbody2D rb;
    internal SpriteRenderer sr;
    internal Animator animator;
    public GameObject weaponHitbox;
    internal Vector2 moveDirection;
    internal Vector2 lastAnimDir; // Locked to 4 direction
    public float walkSpeed;
    public float runSpeed;
    [Header("Attack Settings")]
    public float attackDuration = 0.5f;
    public float attackWindupDuration = 0.3f;
    public float damage;
    public CharacterStats Strength;
    public bool InputRun => Input.GetKey(KeyCode.LeftShift);
    [Header("Dash Settings")]
    public float dashSpeed;
    public float dashDuration;
    public float dashCoolDown;
    internal float currentDashCooldown;
    [Header("Ghost Settings")]
    public float ghostSpawnInterval = 0.1f; // Time between spawning each ghost
    public PlayerGhost ghostPrefab;
    public System.Action OnTakeDamage;

    public int currentHealth;
    public int maxHealth;
    #region Animation Keys
    public static readonly int HorizontalParameterKey = Animator.StringToHash("horizontal");
    public static readonly int VerticalParameterKey = Animator.StringToHash("vertical");
    public static readonly int IdleKey = Animator.StringToHash("Idle");
    public static readonly int WalkKey = Animator.StringToHash("Walk");
    public static readonly int RunKey = Animator.StringToHash("Run");
    public static readonly int AttackKey = Animator.StringToHash("Attack");
    #endregion
    public override BaseState StartState => new IdleState(this);
    public override BaseState DefaultState => new IdleState(this);
    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        instance = this;
    }
    protected override void Start()
    {
        base.Start();
        currentHealth = maxHealth;
    }
    public IEnumerator SpawnGhost()
    {
        while (true)
        {
            yield return new WaitForSeconds(ghostSpawnInterval);
            var ghost = Instantiate(ghostPrefab, transform.position, transform.rotation);
            ghost.SetSprite(sr.sprite);
        }
    }

    public void ApplyKnockback(Vector3 direction)
    {
        rb.AddForce(direction * 5f, ForceMode2D.Impulse);
    }

    public void TakeDamage(int damage)
    {
        OnTakeDamage?.Invoke(); // ? is used for if event is null
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            // Player is dead, handle game over or respawn logic here
        }
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);

        if (other.CompareTag("Enemy"))
        {
            // Get the enemy component from the collided object
            Enemy enemy = other.GetComponent<Enemy>();

            if (enemy != null)
            {
                // Deal damage to the enemy
                Vector3 knockbackDirection = (Player.instance.transform.position - enemy.transform.position).normalized;
                Player.instance.ApplyKnockback(knockbackDirection);
                TakeDamage(enemy.damage);
            }
        }
    }
}
