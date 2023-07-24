using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
// The names in this class need to match the name of the CSV files exactly
public class RefData
{
    //public List<DialogueData> Refdata;
    //public List<RefenemyData> RefenemyData;
    //public List<enemyMovementData> RefenemyMovement;
    //public List<RefnpcData> RefnpcData;
    public List<DialogueData> dialogueData;
    public List<EnemyProperties> enemies;
    public List<ChestProperties> chestProperties;

    //public List<Player> PlayerList;
    //public List<NPC> NPCList;
    //public List<Enemy> EnemiesList;
    //public List<Enemy> EnemiesMovement;
    //public List<Weapon> Weapon;
    //public List<Inventory> Inventory;
    //public List<Slot> Slot;
}
