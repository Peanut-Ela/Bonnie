using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bag : MonoBehaviour
{
    bool isClosed = true; // Initialize the bag as closed initially
    public GameObject bag;

    private void Update()
    {
        // Check if the TAB key is pressed
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            OpenCloseBag();
        }
    }

    public void OpenCloseBag()
    {
        if (isClosed)
        {
            bag.SetActive(true);
            isClosed = false;
        }
        else
        {
            bag.SetActive(false);
            isClosed = true;
        }
    }
}
