using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerStates;
using static UnityEditor.Progress;
//using Bonnie.CharacterStats;

// Define a struct to store player stats
[System.Serializable]
//public struct PlayerStats
//{
//    [Header("Speed Settings")]
//    public float walkSpeed;
//    public float runSpeed;

//    [Header("Defense Settings")]
//    public float defense;

//    [Header("Attack Settings")]
//    public float attackDuration;
//    public float attackWindupDuration;
//    public float damage;

//    [Header("Dash Settings")]
//    public float dashSpeed;
//    public float dashDuration;
//    public float dashCoolDown;

//    [Header("Ghost Settings")]
//    public float ghostSpawnInterval;
//}

public class Player : StateMachine
{
    public static Player instance;
    internal Rigidbody2D rb;
    internal SpriteRenderer sr;
    internal Animator animator;
    public GameObject weaponHitbox;
    //public GameObject mouseSprite;
    internal Vector2 moveDirection;
    internal Vector2 lastAnimDir; // Locked to 4 direction

    //public PlayerStats playerStats; // Store player stats using the defined struct


    [Header("Inventory Settings")]
    public Spawn spawnComponent;
    public int inventorySlots;
    public int[] items;

    [Header("Speed Settings")]
    public float walkSpeed;
    public float runSpeed;

    [Header("Defense Settings")]
    public float defense;

    [Header("Attack Settings")]
    public float attackDuration = 0.5f;
    public float attackWindupDuration = 0.3f;
    public float damage;
    //public CharacterStats Strength;
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

    public Shield equippedShield;
    public Weapon equippedWeapon;
    public Speed milkDrank;
    //private float baseDamage;

    [Header("Health Settings")]
    public int currentHealth;
    public int maxHealth;

    public CoinManager coinManager;

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

        items = new int[inventorySlots];
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

    public int CalculateDamageReceived(int damage)
    {
        float damageReduction = defense / 100f; // Convert defense level to a decimal between 0 and 1
        int damageReceived = Mathf.RoundToInt(damage * (1f - damageReduction)); // Calculate the damage after applying defense reduction
        return damageReceived;
    }

    public void TakeDamage(int damage)
    {
        OnTakeDamage?.Invoke();
        int damageReceived = CalculateDamageReceived(damage);
        currentHealth -= damageReceived;

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

        else
        {
            if (other.gameObject.CompareTag("Coin"))
            {
                Destroy(other.gameObject);
                coinManager.coinCount++;
            }
        }

    }


    public void EquipWeapon(Weapon weapon)
    {
        if (equippedWeapon != null)
        {
            // Unequip the previously equipped weapon and remove its damage amount
            UnequipWeapon();
        }

        equippedWeapon = weapon;
        weapon.transform.SetParent(transform);
        weapon.transform.localPosition = Vector3.zero;
        damage = weapon.damageIncreaseAmount;
    }

    public void UnequipWeapon()
    {
        if (equippedWeapon != null)
        {
            equippedWeapon.transform.SetParent(null);
            equippedWeapon = null;
        }
    }

    public void IncreaseDamage(float amount)
    {
        damage = amount;
    }

    public void EquipShield(Shield shield)
    {
        if (equippedShield != null)
        {
            // Unequip the previously equipped weapon and remove its damage amount
            UnequipShield();
        }

        equippedShield = shield;
        shield.transform.SetParent(transform);
        shield.transform.localPosition = Vector3.zero;
        defense = shield.defenseIncreaseAmount;
    }

    public void UnequipShield()
    {
        if (equippedShield != null)
        {
            equippedShield.transform.SetParent(null);
            equippedShield = null;
        }
    }

    public void IncreaseDefense(float amount)
    {
        defense = amount;
    }

    public void DrinkMilk(Speed speed)
    {
        if (milkDrank != null)
        {
            // Unequip the previously equipped milk and remove its speed increase
            UnequipMilk();
        }

        milkDrank = speed;
        speed.transform.SetParent(transform);
        speed.transform.localPosition = Vector3.zero;
        IncreaseSpeed(speed.walkSpeedIncreaseAmount, speed.runSpeedIncreaseAmount);
    }

    public void IncreaseSpeed(float walkAmount, float runAmount)
    {
        walkSpeed += walkAmount;
        runSpeed += runAmount;
    }


    public void UnequipMilk()
    {
        if (milkDrank != null)
        {
            milkDrank.transform.SetParent(null);
            milkDrank = null;
            //// Reset to default speed values or apply any desired logic
            //walkSpeed = defaultWalkSpeed;
            //runSpeed = defaultRunSpeed;
        }
    }


    // New property name to avoid naming conflict
    public float TotalDamage
    {
        get { return damage + (equippedWeapon != null ? equippedWeapon.damageIncreaseAmount : 0f); }
    }
    public void DropItem(int index)
    {
        items[index] = 0;
        
    }

    //public static Item GetItem(int index) => instance.itemPrefabs[index];

    //private GameObject GetItemPrefab(int index)
    //{
    //    if (index >= 0 && index < ItemManager.instance.itemPrefabs.Length)
    //    {
    //        return ItemManager.instance.itemPrefabs[index];
    //    }
    //    else
    //    {
    //        Debug.LogError("Item prefab for index " + index + " is not defined.");
    //        return null;
    //    }
    //}


}
