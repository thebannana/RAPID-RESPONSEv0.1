using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeLevelOnClick : MonoBehaviour
{
    public string sceneName; // Name of the scene to load

    private void Start()
    {
        // Assuming the button is using the UI Button component, add a listener to its onClick event
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(LoadSceneOnClick);
        }
        else
        {
            Debug.LogError("ChangeLevelOnClick script attached to a GameObject without a Button component.");
        }
    }

    private void LoadSceneOnClick()
    {
        // Load the scene when the button is clicked
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(sceneName);
    }
}

