using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsOpener : MonoBehaviour
{
    public GameObject ControlsPanel;

    public bool IsControlsOpen = false;

    public void ControlsOpen()
    {
        IsControlsOpen = !IsControlsOpen;
        ControlsPanel.SetActive(IsControlsOpen);
    }
    public void ControlsExit()
    {
        IsControlsOpen = !IsControlsOpen;
        ControlsPanel.SetActive(IsControlsOpen);
    }
    void Update()
    {
        if (ControlsPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            ControlsExit();
        }
    }
}
