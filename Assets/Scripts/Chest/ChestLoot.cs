using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
 public class ChestLoot: ScriptableObject
{
    public GameObject lootPrefab;
    public string lootName;
    public int dropChance;

    public ChestLoot(GameObject lootPrefab, string lootName, int dropChance)
    {
        this.lootPrefab = lootPrefab;
        this.lootName = lootName;
        this.dropChance = dropChance;
    }
}

/*public class ChestLoot 
{
    [SerializeField] private Chest loot;
    public GameObject chestItemPrefab;
    public List<Chest> chestList = new List<Chest>();
    // Start is called before the first frame update

    Chest GetChest()
    {
        int randomNumber = Random.Range()
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}*/



