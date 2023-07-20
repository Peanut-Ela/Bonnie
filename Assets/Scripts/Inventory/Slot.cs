using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour {


    public int index;

    private void Update()
    {
        //if (transform.childCount <= 0) {
        //    Player.instance.items[index] = 0;
        //}
    }

    public void Cross() {

        foreach (Transform child in transform) {
            child.GetComponent<Spawn>().SpawnItem();
            GameObject.Destroy(child.gameObject);
        }
    }

}
