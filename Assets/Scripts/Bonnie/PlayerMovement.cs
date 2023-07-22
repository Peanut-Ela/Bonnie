using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed;
    public float runSpeed;
    public Rigidbody2D rb;



    private Vector2 moveDirection;
    private Animator animator;

    private bool playAttackAnim = false;
    public float attackDuration = 0.5f;
    private float attackTimer;
    private bool playedAttackAnim = false;

    public Attack attack;

    #region Animation Keys
    public static readonly int HorizontalParameterKey = Animator.StringToHash("horizontal");
    public static readonly int VerticalParameterKey = Animator.StringToHash("vertical");
    public static readonly int IdleKey = Animator.StringToHash("Idle");
    public static readonly int WalkKey = Animator.StringToHash("Walk");
    public static readonly int RunKey = Animator.StringToHash("Run");
    public static readonly int AttackKey = Animator.StringToHash("Attack");
    #endregion

    private enum PlayerState
    {
        Idle,
        Walking,
        Running,
        Attacking
    }

    private PlayerState currentState;
    private Vector2 previousMoveDirection;

    [Header("Dash Settings")]
    public float dashSpeed;
    public float dashDuration;
    public float dashCoolDown;
    public bool isDashing;
    public bool canDash = true;

    [Header("Ghost Settings")]
    [Header("Ghost Settings")]
    public GameObject[] ghostPrefabs; // Array to hold the ghost prefabs (0: Left, 1: Right, 2: Up, 3: Down)
    public int maxGhostCount = 5;
    public float ghostFadeDuration = 0.5f;
    public float ghostSpawnInterval = 0.1f; // Time between spawning each ghost
    private List<GameObject> ghostInstances = new List<GameObject>();

    public CoinManager coinManager;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        currentState = PlayerState.Idle;
        previousMoveDirection = Vector2.down;
        canDash = true;
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInputs();
        UpdateAnimationState();
        if (playAttackAnim)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer > attackDuration)
            {
                attackTimer = 0;
                playAttackAnim = false;
            }
        }
    }

    void FixedUpdate()
    {
        if (isDashing) return;
        Move();
    }

    void ProcessInputs()
    {
        if (isDashing || playAttackAnim) return;

        Vector2 inputDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveDirection = inputDirection.normalized;

        if (inputDirection != Vector2.zero)
            previousMoveDirection = moveDirection;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentState = PlayerState.Attacking;
            playAttackAnim = true;
            playedAttackAnim = false;
            return;
        }

        if (Input.GetKeyDown(KeyCode.Q) && canDash)
        {
            StartCoroutine(Dash());
        }

        if (moveDirection == Vector2.zero)
            currentState = PlayerState.Idle;
        else
            currentState = Input.GetKey(KeyCode.LeftShift) ? PlayerState.Running : PlayerState.Walking;
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;
        rb.velocity = moveDirection * dashSpeed;

        StartCoroutine(SpawnGhostInstances());

        yield return new WaitForSeconds(dashDuration);

        rb.velocity = Vector2.zero;
        isDashing = false;
        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;
    }

    private IEnumerator SpawnGhostInstances()
    {
        while (isDashing)
        {
            CreateGhostInstance();
            yield return new WaitForSeconds(ghostSpawnInterval);
        }
    }

    private void CreateGhostInstance()
    {
        if (ghostInstances.Count >= maxGhostCount)
        {
            Destroy(ghostInstances[0]);
            ghostInstances.RemoveAt(0);
        }

        GameObject ghostPrefabToInstantiate;
        if (previousMoveDirection == Vector2.left)
            ghostPrefabToInstantiate = ghostPrefabs[0]; // Left
        else if (previousMoveDirection == Vector2.right)
            ghostPrefabToInstantiate = ghostPrefabs[1]; // Right
        else if (previousMoveDirection == Vector2.up)
            ghostPrefabToInstantiate = ghostPrefabs[2]; // Up
        else
            ghostPrefabToInstantiate = ghostPrefabs[3]; // Down

        GameObject ghost = Instantiate(ghostPrefabToInstantiate, transform.position, Quaternion.identity);
        ghost.transform.SetParent(transform.Find("GhostContainer"));
        ghostInstances.Add(ghost);

        StartCoroutine(FadeOutGhost(ghost));
    }

    private IEnumerator FadeOutGhost(GameObject ghost)
    {
        SpriteRenderer ghostRenderer = ghost.GetComponent<SpriteRenderer>();
        Color originalColor = ghostRenderer.color;
        float startTime = Time.time;

        while (Time.time - startTime < ghostFadeDuration)
        {
            float normalizedTime = (Time.time - startTime) / ghostFadeDuration;
            ghostRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f - normalizedTime);
            yield return null;
        }

        // Ensure the ghost is fully faded out
        ghostRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

        // Remove the ghost from the list and destroy it
        ghostInstances.Remove(ghost);
        Destroy(ghost);
    }

    void Move()
    {
        if (currentState != PlayerState.Attacking)
        {
            float speed = currentState == PlayerState.Running ? runSpeed : walkSpeed;
            Vector2 movement = moveDirection * speed;
            rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);
        }
        else
        {
            rb.velocity = Vector2.zero; // Stop player movement during attack
        }
    }

    void UpdateAnimationState()
    {
        animator.SetFloat(HorizontalParameterKey, previousMoveDirection.x);
        animator.SetFloat(VerticalParameterKey, previousMoveDirection.y);

        switch (currentState)
        {
            case PlayerState.Idle:
                animator.PlayInFixedTime(IdleKey);
                break;
            case PlayerState.Walking:
                animator.PlayInFixedTime(WalkKey);
                break;
            case PlayerState.Running:
                animator.PlayInFixedTime(RunKey);
                break;
            case PlayerState.Attacking:
                if (!playedAttackAnim)
                {
                    // Used to play attack animation once only instead of in every frame
                    animator.PlayInFixedTime(AttackKey);
                    playedAttackAnim = true;
                }
                break;
        }
    }

    public void SwordAttack()
    {
        rb.velocity = Vector2.zero;

        if (previousMoveDirection == Vector2.left)
        {
            attack.AttackLeft();
        }
        else if (previousMoveDirection == Vector2.right)
        {
            attack.AttackRight();
        }
        else if (previousMoveDirection == Vector2.up)
        {
            attack.AttackUp();
        }
        else if (previousMoveDirection == Vector2.down)
        {
            attack.AttackDown();
        }
    }

    public void EndSwordAttack()
    {
        attack.StopAttack();
    }


}
