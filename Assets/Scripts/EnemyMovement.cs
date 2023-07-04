using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public float moveSpeed = 2f;
    public float chargeSpeed = 5f;

    private float idleTimer;
    public float idleDurationMin = 1f;
    public float idleDurationMax = 3f;

    private float moveTimer;
    public float moveDurationMin = 1f;
    public float moveDurationMax = 3f;

    public float detectionRange = 2f; // Range within which the NPC stops moving

    public float chargeStoppingDistance = 4f; // Distance at which the NPC stops charging

    private Vector2 moveDirection;
    public bool isMoving;
    public bool isCharging; // Flag to check if the NPC is currently charging

    private bool isPlayerInRange; // Flag to check if the player is within range
    private bool isPlayerInFront; // Flag to check if the player is directly in front of the NPC

    public string[] animationKeys;

    private GameObject player; // Reference to the player object
    private Rigidbody2D rb;


    private void Start()
    {
        isCharging = false;

        rb = GetComponent<Rigidbody2D>();
        SetRandomAnimation();
        idleTimer = Random.Range(idleDurationMin, idleDurationMax);

        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (!isPlayerInRange) // Only move if the player is not within range
        {
            if (isMoving)
            {
                Move();
                moveTimer -= Time.deltaTime;
                if (moveTimer <= 0f)
                {
                    isMoving = false;
                    SetRandomAnimation();
                    idleTimer = Random.Range(idleDurationMin, idleDurationMax);
                }
            }
            else
            {
                if (!isPlayerInFront) // Check if the player is not in front
                {
                    idleTimer -= Time.deltaTime;
                    if (idleTimer <= 0f)
                    {
                        isMoving = true;
                        SetRandomMoveDirection();
                        SetWalkAnimation();
                        moveTimer = Random.Range(moveDurationMin, moveDurationMax);
                    }
                }
                else // Player is in front, stop and become idle
                {
                    isMoving = false;
                    SetRandomAnimation();
                    idleTimer = Random.Range(idleDurationMin, idleDurationMax);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        // Update the isPlayerInFront flag continuously
        if (isPlayerInRange && player != null)
        {
            Vector2 playerDirection = player.transform.position - transform.position;
            float angle = Vector2.Angle(transform.up, playerDirection);
            isPlayerInFront = Mathf.Abs(angle) <= 45f; // Check if the player is within a 90-degree cone in front of the NPC

            //Change to distance range - owen

            // Check if the player is within the charge stopping distance
            float distanceToPlayer = playerDirection.magnitude;
            if (isPlayerInFront && distanceToPlayer <= chargeStoppingDistance)
            {
                if (!isCharging)
                {
                    // Player is in front and within the charge stopping distance, start charging
                    isCharging = true;
                    SetChargeAnimation();
                }
                else
                {
                    // NPC is already charging, check if it has reached the player's position
                    float stoppingDistance = chargeStoppingDistance - 0.5f; // Add a buffer distance to stop slightly before reaching the player's position
                    if (distanceToPlayer <= stoppingDistance)
                    {
                        // NPC has reached the player's position, stop charging and become idle
                        isCharging = false;
                        rb.velocity = Vector2.zero;
                        SetRandomAnimation();
                        idleTimer = Random.Range(idleDurationMin, idleDurationMax);
                    }
                }
            }
            else
            {
                if (isCharging)
                {
                    // Player is no longer within the charge stopping distance, stop charging and become idle
                    isCharging = false;
                    rb.velocity = Vector2.zero;
                    SetRandomAnimation();
                    idleTimer = Random.Range(idleDurationMin, idleDurationMax);
                }
            }
        }
    }

    private void Move()
    {
        // Calculate the new position based on the moveDirection
        Vector3 newPosition = transform.position + (Vector3)(moveDirection * moveSpeed * Time.deltaTime);

        // Check if there is a collision with the tilemap at the new position
        Collider2D[] colliders = Physics2D.OverlapCircleAll(newPosition, 0.2f, LayerMask.GetMask("Tilemap"));
        if (colliders.Length > 0)
        {
            // If there is a collision, adjust the moveDirection to prevent going beyond the tilemap collider
            moveDirection = Vector2.zero;
        }
        else
        {
            // If there is no collision, move the NPC to the new position
            transform.position = newPosition;
        }

        rb.velocity = moveDirection.normalized * moveSpeed * Time.deltaTime;

        if (moveDirection.magnitude > 0)
        {
            //transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
            if (moveDirection.x < 0)
            {
                spriteRenderer.flipX = true;
            }
            else if (moveDirection.x > 0)
            {
                spriteRenderer.flipX = false;
            }
        }
    }

    private void SetRandomAnimation()
    {
        int randomIndex = Random.Range(0, animationKeys.Length);
        string randomKey = animationKeys[randomIndex];
        PlayAnimation(randomKey);
    }

    private void SetWalkAnimation()
    {
        string walkKey = animationKeys[0];
        if (moveDirection.magnitude > 0)
        {
            PlayAnimation(walkKey);
        }
        else
        {
            StopAnimation(walkKey);
        }
    }

    private void SetChargeAnimation()
    {
        string chargeKey = animationKeys[2];
        PlayAnimation(chargeKey);
    }

    private void StopAnimation(string animationName)
    {
        animator.Play(animationName, -1, 0f);
        animator.speed = 0f;
    }

    private void PlayAnimation(string animationName)
    {
        animator.Play(animationName, -1, 0f);
    }

    private void SetRandomMoveDirection()
    {
        moveDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            isPlayerInRange = false;
        }
    }
}
