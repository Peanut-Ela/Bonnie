using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
// The names in this class need to match the name of the CSV files exactly
public class RefData
{
    public List<PlayerStats> player;
    public List<EnemyProperties> enemies;
    public List<WeaponProperties> weapon;
    public List<ShieldProperties> shield;

    public List<DialogueData> dialogueData;
    public List<NPCProperties> NPC;

    public List<ItemProperties> item;
    public List<ChestProperties> chest;
}
