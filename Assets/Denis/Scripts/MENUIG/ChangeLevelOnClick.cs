using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneOnClick : MonoBehaviour
{
    public string levelName; // Name of the level to load

    void OnMouseDown()
    {
        // Load the specified level
        SceneManager.LoadScene(levelName);
    }
}

