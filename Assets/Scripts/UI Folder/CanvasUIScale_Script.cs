using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasUIScale_Script : MonoBehaviour
{
    public float Scale = 1.2f;
    // Start is called before the first frame update
    void Awake()
    {
        GetComponent<RectTransform>().sizeDelta = Camera.main.pixelRect.size * Scale;
    }

    // Update is called once per frame
    void Update()
    {

    }
}