using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.AddressableAssets;
using System;
using Unity.VisualScripting;
/// <summary>
/// This is for loading data from JSON to put into GameData
/// </summary>
public class DataManager : MonoBehaviour
{
    // Start is called before the first frame update
    public void LoadRefData(Action onloaded)
    {
        StartCoroutine(LoadRefData("JSONData", onloaded));
    }

    public IEnumerator LoadRefData(string path, Action onloaded)
    {
        bool processing = true;
        string loadedText = "";

        Addressables.LoadAssetAsync<TextAsset>(path).Completed += (dataPath) =>
        {
            loadedText = dataPath.Result.text;
            processing = false;
        };

        while (processing)
            yield return null;
        RefData dataToLoad = JsonUtility.FromJson<RefData>(loadedText);
        ProcessData(dataToLoad);

        onloaded?.Invoke();
    }

    private void ProcessData(RefData data)
    {
        GameAssets.instance.playerStatsList = new();

        foreach (var refplayer in data.player)
        {
            refplayer.Parse();
            GameAssets.instance.playerStatsList.Add(refplayer);
        }

        foreach (var refenemy in data.enemies) 
        {
            refenemy.Parse(); 
            GameAssets.instance.enemyPropertiesList.Add(refenemy);
        }

        foreach (var refweapon in data.weapon)
            GameAssets.instance.weaponPropertiesList.Add(refweapon);

        foreach (var refshield in data.shield)
            GameAssets.instance.shieldPropertiesList.Add(refshield);

        foreach (var refnpc in data.NPC)
            GameAssets.instance.npcPropertiesList.Add(refnpc);

        foreach (var refdialogue in data.dialogueData) 
        {
            refdialogue.Parse();
            GameAssets.instance.dialogueList.Add(refdialogue);
        }

        foreach (var refitem in data.item)
            GameAssets.instance.itemPropertiesList.Add(refitem);

        //foreach (var refchest in data.chest)
        //    GameAssets.instance.chestPropertiesList.Add(refchest);
    }

    //private void ProcessEnemyData(EnemyData Enemy)
    //{
    //    List<EnemyData> enemyList = new List<EnemyData>();
    //    foreach (RefenemyData refenemy in Enemy.RefenemyData)
    //    {
    //        enemyData EnemyData = new EnemyData(refenemy.enemy, refenemy.detectionRange, refenemy.health, refenemy.damage, refenemy.fadeDuration, refenemy.fadeIterations, refenemy.idleDurationMin, refenemy.idleDurationMax);
    //        enemyList.Add(EnemyData);
    //    }
    //    Game.SetEnemyList(enemyList);

    //}
}
