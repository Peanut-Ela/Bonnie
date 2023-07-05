using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerStates;

public class Player : StateMachine
{
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
    public bool InputRun => Input.GetKey(KeyCode.LeftShift);
    [Header("Dash Settings")]
    public float dashSpeed;
    public float dashDuration;
    public float dashCoolDown;
    internal float currentDashCooldown;
    [Header("Ghost Settings")]
    public float ghostSpawnInterval = 0.1f; // Time between spawning each ghost
    public PlayerGhost ghostPrefab;
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
}
