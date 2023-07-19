using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject item;

    void Start()
    {
        
    }

    // Update is called once per frame
    public void SpawnDroppedItem()
    {
        Vector2 playerPos = new Vector2(Player.instance.transform.position.x, Player.instance.transform.position.x + 3);
        Instantiate(item, playerPos, Quaternion.identity);
    }
}
