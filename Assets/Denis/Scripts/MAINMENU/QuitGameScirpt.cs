using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGameScirpt : MonoBehaviour
{
    // Method called when the Quit button is clicked
    public void QuitGame()
    {
        // Quit the application
        Application.Quit();

        // Log a message to indicate that the application is quitting (optional)
        Debug.Log("Quitting game...");
    }
}

