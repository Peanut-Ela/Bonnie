using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsUI : MonoBehaviour
{
    private void OnEnable()
    {

        Pause.instance.analyticsPanel.SetActive(false);

    }

    private void OnDisable()
    {
    }
}
