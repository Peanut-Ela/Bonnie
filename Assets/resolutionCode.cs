using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resolutionCode : MonoBehaviour
{
    private void Awake()
    {
        Screen.SetResolution(1920, 1080, false);
    }
}
