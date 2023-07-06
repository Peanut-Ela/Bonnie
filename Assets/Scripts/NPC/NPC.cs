using NPCStates;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : StateMachine
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI dialogueName;
    public Image speakerImage;
    public float wordSpeed; 
    public bool playerIsClose;
    public GameObject contButton;

    internal SpriteRenderer sr;
    internal Animator animator;
    internal Rigidbody2D rb;
    internal Vector2 moveDirection;



    public AudioSource typingSound; // Reference to the audio source for the typing sound effect
    public float pitchIncreaseInterval = 3; // Number of letters after which the pitch should increase
    public float pitchVariance = 0.1f; // Maximum amount by which the pitch can vary

    [SerializeField]
    [Header("Dialogue Data")]
    private List<DialogueData> dialogueDataList = new List<DialogueData>();
    private int index;
    private Coroutine typingCoroutine;

    [Header("Idle Settings")]
    public float idleDurationMin = 1f;
    public float idleDurationMax = 3f;

    [Header("Wander Settings")]
    public float moveSpeed = 5f;

    #region Animation Keys
    public static readonly int IdleKey = Animator.StringToHash("Idle");
    public static readonly int WalkKey = Animator.StringToHash("Walk");
    public static readonly int LoveKey = Animator.StringToHash("Love");

    public static readonly int NomKey = Animator.StringToHash("Nom");
    public static readonly int ChewKey = Animator.StringToHash("Chew");

    public static readonly int DownKey = Animator.StringToHash("Down");
    public static readonly int Down_IdleKey = Animator.StringToHash("Down_Idle");
    public static readonly int Down_SleepKey = Animator.StringToHash("Down_Sleep");
    public static readonly int[] IdleKeys = new int[]
    {
        Animator.StringToHash("Idle"),
        Animator.StringToHash("Nom"),
        Animator.StringToHash("Chew"),
        Animator.StringToHash("Down"),
        Animator.StringToHash("Down_Idle"),
        Animator.StringToHash("Down_Sleep")
    };
    #endregion
    public override BaseState StartState => new IdleState(this);
    public override BaseState DefaultState => new IdleState(this);
    [Serializable]
    public class DialogueData
    {
        public string text;
        public string dialogueName;
        public Sprite speakerSprite;
    }

    // Add any additional functions or variables you need here
    protected override void Awake()
    {
        base.Awake();
        visualCue.SetActive(false);
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    protected override void Update()
    {
        base.Update();

        if (playerIsClose)
        {
            visualCue.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                QueueState(new IdleState(this)); // Enter the Idle state when the dialogue ends
                visualCue.SetActive(false);
                if (dialoguePanel.activeInHierarchy)
                {
                    zeroText();
                    if (sr.flipX != playerIsClose) // Face the player if not already facing the player
                    {
                        sr.flipX = playerIsClose;
                    }
                }
                else
                {
                    dialoguePanel.SetActive(true);
                    StartTyping();
                }
            }

            if (dialogueText.text == dialogueDataList[index].text)
            {
                contButton.SetActive(true);
            }
        }
        else
        {
            visualCue.SetActive(false);
        }
    }


    public void zeroText()
    {
        dialogueText.text = "";
        dialogueName.text = "";
        index = 0;
        speakerImage.sprite = null; // Clear the speaker image when dialogue is not active
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingSound.Stop(); // Stop the typing sound
        }
        dialoguePanel.SetActive(false);
    }

    public void StartTyping()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        dialogueText.text = "";
        dialogueName.text = dialogueDataList[index].dialogueName;
        speakerImage.sprite = dialogueDataList[index].speakerSprite; // Set the speaker image for this dialogue element
        typingCoroutine = StartCoroutine(Typing());
    }

    IEnumerator Typing()
    {
        int letterCount = 0; // Count of letters typed
        foreach (char letter in dialogueDataList[index].text.ToCharArray())
        {
            dialogueText.text += letter;
            typingSound.pitch = UnityEngine.Random.Range(1f - pitchVariance, 1f + pitchVariance); // Randomize the pitch
            letterCount++;

            // Check if the letter count is a multiple of the pitch increase interval
            if (letterCount % pitchIncreaseInterval == 0)
            {
                typingSound.pitch = 1f; // Reset the pitch to normal
            }

            typingSound.Play(); // Play the typing sound effect
            //yield return new WaitForSeconds(wordSpeed);
            yield return new WaitForSecondsRealtime(wordSpeed);
        }
    }

    public void NextLine()
    {
        contButton.SetActive(false);

        if (index < dialogueDataList.Count - 1)
        {
            index++;
            StartTyping();
        }
        else
        {
            zeroText();
        }
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
        }
    }

    protected override void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
            zeroText();
        }
    }
}
