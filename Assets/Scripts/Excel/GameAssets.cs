using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    public static GameAssets instance;

    DataManager dataManager;
    public List<DialogueData> dialogueList = new();
    public List<EnemyProperties> enemyPropertiesList = new();
    public List<ChestProperties> chestPropertiesList = new();
    public List<PlayerStats> playerStatsList;

    //public List<Player> PlayerList;
    //public List<NPC> NPCList;
    //public List<Enemy> EnemiesList;
    //public List<Enemy> EnemiesMovement;
    //public List<Weapon> Weapon;
    //public List<Inventory> Inventory;
    //public List<Slot> Slot;

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
            return playerStatsList[selectedCharacter].playerID;
        }
        else
        {
            Debug.LogError("Invalid selectedCharacter index or playerStatsList is empty.");
            return -1; // Return an invalid value to indicate an error
        }
    }

    //get dialogue data with npc openingdialogueid


}
