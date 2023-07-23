using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class holds the data that is loaded for stuff in unity to get reference from
/// </summary>
public class GameData : MonoBehaviour
{
    public static GameData instance;
    DataManager dataManager;
    public List<DialogueData> dialogueList = new();
    public List<EnemyProperties> enemyPropertiesList = new();
    public List<ChestProperties> chestProperties = new();
    //public List<Player> PlayerList;
    //public List<NPC> NPCList;
    //public List<Enemy> EnemiesList;
    //public List<Enemy> EnemiesMovement;
    //public List<Weapon> Weapon;
    //public List<Inventory> Inventory;
    //public List<Slot> Slot;
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
    //public static List<DialogueData> GetDialogueDataList() { return dialogueList; }
    public void RunOnce()
    {
        dataManager = GetComponent<DataManager>();
        dataManager.LoadRefData(StartGame);
    }
    public void StartGame()
    {

    }
    public static DialogueData GetDialogueData(string dialogueId) => instance.dialogueList.Find(aaaaa => aaaaa.dialogueId == dialogueId);
    //public static void SetDialogueList(List<DialogueData> alist) { dialogueList = alist; }

    //public static List<Player> GetPlayerList() { return PlayerList; }
    //public static void SetPlayerList(List<Player> alist) { PlayerList = alist; }

    //public static List<NPC> GetNPCList() { return NPCList; }
    //public static void SetNPCList(List<NPC> alist) { NPCList = alist; }

    //public static List<Enemy> EnemyList() { return EnemiesList; }
    //public static void SetEnemiesList(List<Enemy> alist) { EnemiesList = alist; }

    //public static List<Enemy> EnemyMovement() { return EnemiesMovement; }
    //public static void SetEnemiesMovement(List<Enemy> alist) { EnemiesMovement = alist; }

    //public static List <Inventory> InventoryList() { return Inventory; }
    //public static void SetInventoryList(List<Inventory> alist) { Inventory = alist; }

    //public static List<Slot> SlotList() { return Slot; }
    //public static void SetSlotList(List<Slot> alist) { Slot = alist; }

}
