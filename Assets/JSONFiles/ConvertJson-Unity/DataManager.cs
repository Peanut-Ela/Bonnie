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
        foreach (var refdialogue in data.dialogueData)
            GameData.instance.dialogueList.Add(refdialogue);

        foreach (var item in data.enemies)
            GameData.instance.enemyPropertiesList.Add(item);

        foreach (var item in data.chestProperties)
            GameData.instance.chestProperties.Add(item);
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