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
    public bool playerIsClose => Vector2.Distance(transform.position, Player.instance.transform.position) < dialogueRange;
    public float dialogueRange = 3f;


    internal SpriteRenderer sr;
    internal Animator animator;
    internal Rigidbody2D rb;
    internal Vector2 moveDirection;


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


    [SerializeField]
    [Header("Dialogue Data")]
    public int index;
    public string openingDialogueId;

    //Transfer couroutines to dialoguemanager

    // New DialogueData struct
    [System.Serializable]
    public struct DialogueData
    {
        public int npcId;
        public string lineId;
        public string nextLineId;
        public string text;
        public string dialogueName;
        public Sprite speakerSprite;
        public List<string> choices; // List of choices for the dialogue line
        public List<string> choiceNextLineIds; // List of nextLineIds corresponding to each choice
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



    //public void StartInteraction()
    //{
    //    // Get the DialogueManager instance
    //    DialogueManager dialogueManager = DialogueManager.instance;

    //    if (dialogueManager != null)
    //    {
    //        // Retrieve the DialogueData list for this NPC based on the openingDialogueId
    //        List<DialogueData> npcDialogueList = dialogueManager.GetDialogueDataListForNPC(openingDialogueId);

    //        if (npcDialogueList != null && npcDialogueList.Count > 0)
    //        {
    //            // Assign the retrieved dialogue list to the NPC's dialogueDataList
    //            dialogueDataList = npcDialogueList;

    //            // Start the dialogue by displaying the opening dialogue
    //            index = 0;
    //            StartTyping();
    //        }
    //    }
    //}

    


    

}
