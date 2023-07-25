using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    public static GameAssets instance;

    DataManager dataManager;
    public List<PlayerStats> playerStatsList;
    public List<EnemyProperties> enemyPropertiesList = new();
    public List<WeaponProperties> weaponPropertiesList = new();
    public List<ShieldProperties> shieldPropertiesList = new();
    public List<DialogueData> dialogueList = new();
    public List<NPCProperties> npcPropertiesList = new();
    public List<ItemProperties> itemPropertiesList = new();

    //public List<ChestProperties> chestPropertiesList = new();

    //list of dialogues

    public static int selectedCharacter;

    //public List<PlayerStats> playerStatsList = new List<PlayerStats>();

    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
            RunOnce();
        }
        else
            Destroy(gameObject);
    }
    public void RunOnce()
    {
        dataManager = GetComponent<DataManager>();
        dataManager.LoadRefData(StartGame);
    }

    public void StartGame()
    {
        //is called when finished loading, place UI? place ui in loading screen
    }

    //get player id 
    public int GetPlayerID()
    {
        if (selectedCharacter >= 0 && selectedCharacter < playerStatsList.Count)
        {
            return playerStatsList[selectedCharacter].playerId;
        }
        else
        {
            Debug.LogError("Invalid selectedCharacter index or playerStatsList is empty.");
            return -1; // Return an invalid value to indicate an error
        }
    }

    public static EnemyProperties GetEnemyID(string id) => GameAssets.instance.enemyPropertiesList.Find(a => a.enemyId == id);

    //get dialogue data with npc openingdialogueid


}
