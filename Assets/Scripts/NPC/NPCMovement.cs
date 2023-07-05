using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{

    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public float moveSpeed = 2f;

    private float idleTimer;
    public float idleDurationMin = 1f;
    public float idleDurationMax = 3f;

    private float moveTimer;
    public float moveDurationMin = 1f;
    public float moveDurationMax = 3f;

    public float detectionRange = 2f; // Range within which the NPC stops moving

    private Vector2 moveDirection;
    private bool isMoving;
    private bool isPlayerInRange; // Flag to check if the player is within range
    private bool isPlayerInFront; // Flag to check if the player is directly in front of the NPC

    public string[] animationKeys;
    //"PinkCow_Stand_Idle", "PinkCow_Walk", "PinkCow_Nom", "PinkCow_Down_Sleep", "PinkCow_Chew", "PinkCow_Down_Idle" };

    private GameObject player; // Reference to the player object
    private Rigidbody2D rb;

    private void Start()
    {
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

        //transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        rb.velocity = moveDirection.normalized * moveSpeed * Time.deltaTime;
        if (moveDirection.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (moveDirection.x > 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    private void SetRandomAnimation()
    {
        int randomIndex = Random.Range(0, animationKeys.Length);
        string randomKey = animationKeys[randomIndex];
        PlayAnimation(randomKey);

        //foreach (string key in animationKeys)
        //{
        //    if (key != randomKey)
        //    {
        //        animator.SetBool(key, false);
        //    }
        //}
    }

    private void SetWalkAnimation()
    {
        if (moveDirection.magnitude > 0)
        {
            PlayAnimation("PinkCow_Walk");
        }
        else
        {
            StopAnimation("PinkCow_Walk");
        }
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