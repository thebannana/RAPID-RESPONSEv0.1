using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public TutorialStep[] tutorialSteps; // Array of tutorial steps
    private int currentStepIndex = -1; // Index of the current tutorial step
    private bool isPaused = false;

    void Start()
    {
        foreach (var step in tutorialSteps)
        {
            if (step.tutorialCanvas != null)
            {
                step.tutorialCanvas.gameObject.SetActive(false); // Hide all canvases initially
            }
        }
    }

    public void TriggerTutorialStep(TutorialStep step)
    {
        int stepIndex = System.Array.IndexOf(tutorialSteps, step);
        if (stepIndex != -1 && currentStepIndex != stepIndex)
        {
            if (currentStepIndex >= 0 && tutorialSteps[currentStepIndex].tutorialCanvas != null)
            {
                tutorialSteps[currentStepIndex].tutorialCanvas.gameObject.SetActive(false); // Hide the previous canvas
            }

            currentStepIndex = stepIndex;
            PauseGame();

            if (tutorialSteps[stepIndex].tutorialCanvas != null)
            {
                Debug.Log($"Showing tutorial canvas for step {stepIndex}");
                tutorialSteps[stepIndex].tutorialCanvas.gameObject.SetActive(true); // Show the canvas for this step
            }
        }
    }

    void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // Pause the game
        Debug.Log("Game paused.");
    }

    public void ResumeGame()
    {
        if (isPaused && currentStepIndex >= 0)
        {
            isPaused = false;
            Time.timeScale = 1f; // Resume the game
            Debug.Log("Game resumed.");

            if (tutorialSteps[currentStepIndex].tutorialCanvas != null)
            {
                Debug.Log($"Hiding tutorial canvas for step {currentStepIndex}");
                tutorialSteps[currentStepIndex].tutorialCanvas.gameObject.SetActive(false); // Hide the canvas for the current step
                tutorialSteps[currentStepIndex].DeactivateStep(); // Deactivate the step's GameObject
            }

            currentStepIndex = -1; // Reset the step index
        }
    }
}
