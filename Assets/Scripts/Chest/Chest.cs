using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public ChestRandomDropList<Transform> dropList;
    public Transform itemHolder;

    public bool isOpen;
    public Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isOpen)
        {
            isOpen = true;
            Debug.Log("Chest is now open");
            animator.SetBool("IsOpen", isOpen);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (IsOpen())
            {
                animator.SetTrigger("close");
                //HideItem();
            }
            else
            {
                animator.SetTrigger("open");
                //ShowItem();
            }
        }
    }

    bool IsOpen()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("ChestOpen");
    }

    /*void HideItem()
    {
        itemHolder.localScale = Vector3.zero;
        itemHolder.gameObject.SetActive(false);

        foreach (Transform t in itemHolder)
        {
            Destroy(t.gameObject);
        }
    }

    void ShowItem()
    {
        Transform item = dropList.GetRandom();
        Instantiate(item, itemHolder);
        itemHolder.gameObject.SetActive(true);
    }*/
}
