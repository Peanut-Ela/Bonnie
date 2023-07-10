using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TriggerChest : MonoBehaviour
{
    //private string ChestOpenAnimatorParaName = "ChestOpen";

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
