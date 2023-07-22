using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    public static GameAssets instance;

    public static int selectedCharacter;

    public List<PlayerStats> playerStatsList = new List<PlayerStats>();

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);

        }
        else
            Destroy(instance);
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


}
