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
    [SerializeField] public GameObject visualCue;

    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI dialogueName;
    public Image speakerImage;
    public float wordSpeed;
    public bool playerIsClose => Vector2.Distance(transform.position, Player.instance.transform.position) < dialogueRange;
    public float dialogueRange = 3f;
    public GameObject contButton;
    public GameObject choiceBox; // New choice box object

    internal SpriteRenderer sr;
    internal Animator animator;
    internal Rigidbody2D rb;
    internal Vector2 moveDirection;

    public AudioSource typingSound;
    public float pitchIncreaseInterval = 3;
    public float pitchVariance = 0.1f;

    [SerializeField]
    [Header("Dialogue Data")]
    public int index;
    public List<DialogueData> dialogueDataList = new List<DialogueData>();
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
        public string id;
        public string nextLineid;
        public string text;
        public string dialogueName;
        public Sprite speakerSprite;
        public List<string> choices; // List of choices for the dialogue line
        public List<string> choiceNextLineIds; // List of nextLineids corresponding to each choice
        public List<Color> choiceColors; // List of colors for each choice
    }

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
    }

    public void zeroText()
    {
        dialogueText.text = "";
        dialogueName.text = "";
        index = 0;
        speakerImage.sprite = null;
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingSound.Stop();
        }
        dialoguePanel.SetActive(false);
        HideChoiceBox(); // Hide the choice box when resetting the dialogue

        QueueState(DefaultState);
        StopAllCoroutines();
    }

    public void StartTyping()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        dialogueText.text = "";
        dialogueName.text = dialogueDataList[index].dialogueName;
        speakerImage.sprite = dialogueDataList[index].speakerSprite;
        typingCoroutine = StartCoroutine(Typing());
    }

    IEnumerator Typing()
    {
        int letterCount = 0;
        bool skip = false;
        foreach (char letter in dialogueDataList[index].text.ToCharArray())
        {
            if (letter == '<')
                skip = true;
            if (letter == '>')
                skip = false;

            dialogueText.text += letter;

            if (skip)
            {
                continue;
            }
            else
            {
                typingSound.pitch = UnityEngine.Random.Range(1f - pitchVariance, 1f + pitchVariance);
                letterCount++;
                if (letterCount % pitchIncreaseInterval == 0)
                {
                    typingSound.pitch = 1f;
                }

                typingSound.Play();
                // Wait for wordSpeed seconds, but check for spacebar input to skip
                float elapsedTime = 0f;
                while (elapsedTime < wordSpeed)
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        // Skip the typing animation by instantly showing the entire text
                        dialogueText.text = dialogueDataList[index].text;
                        break;
                    }
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
            }
            //Skip when it starts at < until >
        }

        // Check if the current dialogue line has choices
        if (dialogueDataList[index].choices != null && dialogueDataList[index].choices.Count > 0)
        {
            ShowChoiceBox();
        }
        else
        {
            contButton.SetActive(true);
        }

        yield return null;
    }


    public void NextLine()
    {
        contButton.SetActive(false);

        if (index >= 0 && index < dialogueDataList.Count)
        {
            DialogueData currentDialogue = dialogueDataList[index];
            string nextLineid = currentDialogue.nextLineid;

            if (!string.IsNullOrEmpty(nextLineid))
            {
                index = GetIndexFromNextLineId(nextLineid);
            }
            else
            {
                index++;
            }

            if (index >= 0 && index < dialogueDataList.Count)
            {
                StartTyping();
            }
            else
            {
                zeroText();
            }
        }
        else
        {
            dialoguePanel.SetActive(false);
        }
    }


    private int GetIndexFromNextLineId(string nextLineid)
    {
        for (int i = 0; i < dialogueDataList.Count; i++)
        {
            if (dialogueDataList[i].id == nextLineid)
            {
                return i;
            }
        }
        return -1; // Invalid nextLineid
    }

    private void ShowChoiceBox()
    {
        if (choiceBox != null)
        {
            choiceBox.SetActive(true);
            // Set choice button texts and callbacks
            ChoiceBox choiceBoxComponent = choiceBox.GetComponent<ChoiceBox>();
            if (choiceBoxComponent != null)
            {
                choiceBoxComponent.SetChoices(dialogueDataList[index].choices, OnChoiceSelected);
            }
        }
    }

    private void HideChoiceBox()
    {
        if (choiceBox != null)
        {
            choiceBox.SetActive(false);
        }
    }

    private void OnChoiceSelected(int choiceIndex)
    {
        Debug.Log("Choice selected. Choice index: " + choiceIndex);
        HideChoiceBox();

        // Get the selected dialogue data
        DialogueData currentDialogue = dialogueDataList[index];

        // Check if the selected choice index is within the valid range
        if (choiceIndex >= 0 && choiceIndex < currentDialogue.choices.Count)
        {
            // Check if there is a corresponding nextLineid for the selected choice
            if (choiceIndex < currentDialogue.choiceNextLineIds.Count)
            {
                string nextLineid = currentDialogue.choiceNextLineIds[choiceIndex];

                if (!string.IsNullOrEmpty(nextLineid))
                {
                    index = GetIndexFromNextLineId(nextLineid);

                    if (index == -1)
                    {
                        dialoguePanel.SetActive(false);
                        return;
                    }

                    // Use the selected choice in the next line
                    currentDialogue = dialogueDataList[index];
                }
            }
        }

        // Continue with the next line after processing the choice
        StartTyping();
    }

}
