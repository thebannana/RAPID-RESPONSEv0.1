using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitOnClick : MonoBehaviour
{
    void OnMouseDown()
    {
        // Quit the application
        Application.Quit();
    }
}

