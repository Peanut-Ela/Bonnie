using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TriggerChest : MonoBehaviour
{
    //private string ChestOpenAnimatorParaName = "ChestOpen";

    public List<GameObject> droplist = new List< GameObject > ();
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void OpenChest()
    {
        animator.SetBool("ChestOpen",true);
    }

    public void CloseChest()
    {
        animator.SetBool("ChestOpen", false);
    }
    
}
