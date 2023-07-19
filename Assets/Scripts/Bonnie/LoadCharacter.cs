using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadCharacter : MonoBehaviour
{
    public GameObject[] characterPrefabs;
    public Transform spawnPoint;
    public TMP_Text label;

    // Reference to the existing Player object
    public GameObject playerObject;

    // Start is called before the first frame update
    void Start()
    {
        int selectedCharacter = PlayerPrefs.GetInt("selectedCharacter");
        GameObject prefab = characterPrefabs[selectedCharacter];

        // Destroy the existing player object
        Destroy(playerObject);

        // Transfer the selected character to the playerObject
        playerObject = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        playerObject.name = "Player";

        label.text = prefab.name;
    }
}
