using UnityEngine;
using UnityEngine.UI;

public class TutorialTimer : MonoBehaviour
{
    public Canvas tutorialCanvas; // The canvas to show after the timer
    public Button continueButton; // The button to continue the game
    public GameObject overlay; // Reference to the overlay GameObject
    private float timer = 2f; // 2-second timer
    private bool hasShownTutorial = false;

    void Start()
    {
        // Check if the tutorial has already been shown
        if (PlayerPrefs.GetInt("TutorialShown", 0) == 1)
        {
            // Disable the GameObject holding the canvas if the tutorial has been shown
            tutorialCanvas.gameObject.SetActive(false);
            overlay.SetActive(false);
            gameObject.SetActive(false);
            return;
        }

        // Initially hide the canvas and overlay
        tutorialCanvas.gameObject.SetActive(false);
        overlay.SetActive(false);

        // Assign the button click listener
        continueButton.onClick.AddListener(ResumeGame);
    }

    void Update()
    {
        if (!hasShownTutorial)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                ShowTutorial();
                hasShownTutorial = true;
            }
        }
    }

    void ShowTutorial()
    {
        // Pause the game
        Time.timeScale = 0f;
        // Show the tutorial canvas and overlay
        tutorialCanvas.gameObject.SetActive(true);
        overlay.SetActive(true);
        // Mark the tutorial as shown
        PlayerPrefs.SetInt("TutorialShown", 1);
        PlayerPrefs.Save();
    }

    public void ResumeGame()
    {
        // Hide the tutorial canvas and overlay
        tutorialCanvas.gameObject.SetActive(false);
        overlay.SetActive(false);
        // Resume the game
        Time.timeScale = 1f;
        // Optionally disable this GameObject to ensure the tut
        gameObject.SetActive(false);
    }
}