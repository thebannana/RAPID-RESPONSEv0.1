using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasSwitcher : MonoBehaviour
{
    public GameObject canvasToDisable; // Reference to the canvas to disable
    public GameObject canvasToEnable; // Reference to the canvas to enable

    public void SwitchCanvas()
    {
        // Disable the current canvas
        canvasToDisable.SetActive(false);

        // Enable the new canvas
        canvasToEnable.SetActive(true);
    }
}
