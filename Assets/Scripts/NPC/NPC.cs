using NPCStates;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

[Serializable]

public struct NPCProperties
{
    [Header("Dialogue Settings")]
    public string npcId;
    public float dialogueRange;
    public string visualCueStr;
    public Sprite visualCue;
    private static string visualUIPath = "Assets/Art/UI/Dialogue/{0}.png";


    [Header("Idle Settings")]
    public float idleDurationMin;
    public float idleDurationMax;

    [Header("Wander Settings")]
    public float moveSpeed;

    public static void LoadIcon(string npcIconName, System.Action<Sprite> onLoaded)
    {
        var asyncHandle = Addressables.LoadAssetAsync<Sprite>(string.Format(visualUIPath, npcIconName));
        asyncHandle.Completed += (loadedUI) =>
        {
            onLoaded?.Invoke(loadedUI.Result);
            Addressables.Release(loadedUI);
        };
    }
}

public class NPC : StateMachine
{
    internal SpriteRenderer visualSr;
    internal SpriteRenderer sr;
    internal Animator animator;
    internal Rigidbody2D rb;
    internal Vector2 moveDirection;
    public bool playerIsClose => Vector2.Distance(transform.position, Player.instance.transform.position) < dialogueRange;

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

    [Header("Dialogue Settings")]
    public string openingDialogueId;
    public float dialogueRange;
    public GameObject visualCueIcon;


    [Header("Idle Settings")]
    public float idleDurationMin;
    public float idleDurationMax;

    [Header("Wander Settings")]
    public float moveSpeed;


    protected override void Awake()
    {
        base.Awake();
        visualCueIcon.SetActive(false);
        sr = GetComponent<SpriteRenderer>();
        visualSr = visualCueIcon.GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void Start()
    {
        base.Start();
        SetProperties();
    }

    public void SetProperties()
    {
        if (GameAssets.instance != null)

        {
            NPCProperties npcProperties = GameAssets.instance.npcPropertiesList.Find(a => a.npcId == openingDialogueId);

            // Assign npcProperties values to the corresponding npc properties
            dialogueRange = npcProperties.dialogueRange;
            idleDurationMin = npcProperties.idleDurationMin;
            idleDurationMax = npcProperties.idleDurationMax;
            moveSpeed = npcProperties.moveSpeed;

            // Load the npc sprite and set it to the SpriteRenderer (sr)
            NPCProperties.LoadIcon(npcProperties.visualCueStr, (Sprite visualCue) =>
            {
                sr.sprite = visualCue;
            });
        }
    }

}
