using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChestStates;
using PlayerStates;

[System.Serializable]
public struct ChestProperties
{
    [Header("General Settings")]
    public int chestId;
    public string chestType;

    [Header("Color Settings")]
    public string chestColorStr;
    public Color chestColor;

    [Header("CoinPrefab Settings")]
    public List<GameObject> dropList;

    [Header("Distance Settings")]
    public float itemSpawnDist;


    [Header("Probability Settings")]
    public int minCoinSpawnCount;
    public int maxCoinSpawnCount;

    public void Parse()
    {
        if (!string.IsNullOrEmpty(chestColorStr))
        {
            Color color;
            ColorUtility.TryParseHtmlString(chestColorStr, out color);
            chestColor = color;
        }

    }
}

public class Chest : StateMachine
{
    internal Animator anim;
    internal SpriteRenderer sr;
    public override BaseState StartState => new ClosedState(this);
    #region Animation Keys
    public static readonly int ClosedKey = Animator.StringToHash("Closed");
    public static readonly int OpeningKey = Animator.StringToHash("Opening");
    public static readonly int OpenedKey = Animator.StringToHash("Opened");
    #endregion

    [Header("General Settings")]
    public int chestId;
    public string chestType;

    [Header("CoinPrefab Settings")]
    public List<GameObject> dropList = new List<GameObject>();

    [Header("Distance Settings")]
    public float itemSpawnDist = 1.5f;

    [Header("Probability Settings")]
    public int minCoinSpawnCount = 1;
    public int maxCoinSpawnCount = 8;

    protected override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
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
            ChestProperties chestProperties = GameAssets.instance.chestPropertiesList.Find(a => a.chestId == chestId);

            // Assign chestProperties values to the corresponding chest properties
            chestType = chestProperties.chestType;
            minCoinSpawnCount = chestProperties.minCoinSpawnCount;
            maxCoinSpawnCount = chestProperties.maxCoinSpawnCount;
            itemSpawnDist = chestProperties.itemSpawnDist;
            sr.color = chestProperties.chestColor;
        }
    }

    public void SpawnRandomLoot()
    {
        int randAmt = Random.Range(minCoinSpawnCount, maxCoinSpawnCount);
        for (int i = 0; i < randAmt; i++)
        {
            SpawnItem();
        }
    }
    void SpawnItem()
    {
        if (dropList.Count > 0) // Use dropListScript.dropList instead of LootList
        {
            int randomIndex = Random.Range(0, dropList.Count); // Use dropListScript.dropList instead of LootList
            GameObject itemPrefab = dropList[randomIndex]; // Use dropListScript.dropList instead of LootList

            Vector2 randomDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            float randomDist = Random.value * itemSpawnDist;

            Instantiate(itemPrefab.gameObject, (Vector2)transform.position + randomDir * randomDist, Quaternion.identity);
        }
    }
}
namespace ChestStates
{

    public class BaseChestState : BaseState
    {
        protected Chest chest;

        public BaseChestState(Chest sm) : base(sm)
        {
            chest = sm;
        }
    }
    public class ClosedState : BaseChestState
    {
        bool playerInRange = false;
        public ClosedState(Chest sm) : base(sm) { }
        public override void OnEnter()
        {
            base.OnEnter();
            chest.anim.PlayInFixedTime(Chest.ClosedKey);
        }
        public override void Update()
        {
            base.Update();
            if (playerInRange && Input.GetKeyDown(KeyCode.E))
                chest.QueueState(new OpeningState(chest));
        }
        public override void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);
            if (collision.gameObject.CompareTag("Player"))
            {
                playerInRange = true;
            }
        }
        public override void OnTriggerExit2D(Collider2D collision)
        {
            base.OnTriggerExit2D(collision);
            if (collision.gameObject.CompareTag("Player"))
            {
                playerInRange = false;
            }
        }
    }
    public class OpeningState : BaseChestState
    {
        public OpeningState(Chest sm) : base(sm)
        {
            duration = 0.4f; // duration of opening anim
        }
        public override void OnEnter()
        {
            base.OnEnter();
            chest.anim.PlayInFixedTime(Chest.OpeningKey);

            chest.SpawnRandomLoot();
        }
        public override void OnStateExpired()
        {
            base.OnStateExpired();
            chest.QueueState(new OpenedState(chest));
        }
    }
    public class OpenedState : BaseChestState
    {
        public OpenedState(Chest sm) : base(sm) { }
        public override void OnEnter()
        {
            base.OnEnter();
            chest.anim.PlayInFixedTime(Chest.OpenedKey);
        }
    }
}
