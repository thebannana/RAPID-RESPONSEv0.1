using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public LevelStatus levelStatus; // Reference to the LevelStatus script
    public Button finishLevelButton; // Reference to the Finish Level button
    public GameObject failureCanvas; // Reference to the Failure canvas
    public GameObject successCanvas; // Reference to the Success canvas

    private void Start()
    {
        // Assuming LevelStatus script is attached to a GameObject in the scene
        levelStatus = LevelStatus.Instance;

        // Disable all canvases initially
        finishLevelButton.interactable = false;
        failureCanvas.SetActive(false);
        successCanvas.SetActive(false);
    }

    private void Update()
    {
        // Check the conditions to enable the Finish Level button and canvases
        CheckFinishLevelConditions();
        CheckFailureConditions();
    }

    void CheckFinishLevelConditions()
    {
        // Check if All Suspects are either Killed or Arrested AND All Civilians are Detained
        if (AllSuspectsAreProcessed() && AllCiviliansAreDetained())
        {
            // Enable the Finish Level button
            finishLevelButton.interactable = true;

            // Show the Success canvas immediately
            successCanvas.SetActive(true);
        }
        else
        {
            // Disable the Finish Level button
            finishLevelButton.interactable = false;

            // Hide the Success canvas if conditions are no longer met
            successCanvas.SetActive(false);
        }
    }

    bool AllSuspectsAreProcessed()
    {
        // Check if all suspects are either killed or arrested
        return levelStatus.allSuspects.Count == levelStatus.killedSuspects.Count + levelStatus.arrestedSuspects.Count;
    }

    bool AllCiviliansAreDetained()
    {
        // Check if all civilians are detained
        return levelStatus.allCivilians.Count == levelStatus.detainedCivilians.Count;
    }

    void CheckFailureConditions()
    {
        // Check if Incapacitated units >= 4
        if (levelStatus.incapacitatedUnits.Count >= 4)
        {
            // Show the Failure canvas and hide the success canvas
            failureCanvas.SetActive(true);
            successCanvas.SetActive(false);
        }
        else
        {
            // Hide the Failure canvas if not met
            failureCanvas.SetActive(false);
        }
    }

    // Method to be called when the Finish Level button is pressed
    public void FinishLevel()
    {
        // Optionally, load the next level (you can modify this to load a specific scene)
        // SceneManager.LoadScene("NextLevel");
    }

}
