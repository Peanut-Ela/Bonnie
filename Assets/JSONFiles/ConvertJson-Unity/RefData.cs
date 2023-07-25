using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
// The names in this class need to match the name of the CSV files exactly
public class RefData
{
    public List<DialogueData> dialogueData;
    public List<EnemyProperties> enemies;
    public List<ChestProperties> chest;
    public List<PlayerStats> player;
    public List<WeaponProperties> weapon;
    public List<ShieldProperties> shield;
    public List<NPCProperties> NPC;
    public List<ItemProperties> item;
}
