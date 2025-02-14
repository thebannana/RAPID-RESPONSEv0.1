using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class LevelSelectManager : MonoBehaviour
{
    [System.Serializable]
    public class LevelCanvas
    {
        public Canvas canvas;
        public Button nextButton;
        public Button previousButton;
        public Button playBriefingButton;
        public Button stopBriefingButton;
        public AudioSource briefingAudio;
        public string[] keysToCheck; // Array of keys to check
    }

    public LevelCanvas[] levelCanvases; // Array of canvases for each level
    public float fadeDuration = 1f; // Duration for fading in/out

    private int currentIndex = 0; // Current index of the level canvas
    private string levelPrefKey = "SelectedLevel"; // Key for saving the level name to PlayerPrefs

    void Start()
    {
        // Initialize canvases and buttons
        InitializeCanvases();
        UpdateCanvasVisibility();

        // Add listeners to buttons
        foreach (LevelCanvas levelCanvas in levelCanvases)
        {
            levelCanvas.nextButton.onClick.AddListener(() => ShowNextCanvas(levelCanvas));
            levelCanvas.previousButton.onClick.AddListener(() => ShowPreviousCanvas(levelCanvas));
            levelCanvas.playBriefingButton.onClick.AddListener(() => PlayBriefingAudio(levelCanvas));
            levelCanvas.stopBriefingButton.onClick.AddListener(() => StopBriefingAudio(levelCanvas));
        }
    }

    void Update()
    {
        // Check for mouse click
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                // Check if the hit object has a collider
                if (hit.collider != null)
                {
                    // Fade in the corresponding canvas
                    for (int i = 0; i < levelCanvases.Length; i++)
                    {
                        if (hit.collider.gameObject.name == levelCanvases[i].canvas.name)
                        {
                            currentIndex = i;
                            UpdateCanvasVisibility();
                            break;
                        }
                    }
                }
            }
        }
    }

    void InitializeCanvases()
    {
        foreach (LevelCanvas levelCanvas in levelCanvases)
        {
            CanvasGroup canvasGroup = levelCanvas.canvas.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = levelCanvas.canvas.gameObject.AddComponent<CanvasGroup>();
            }
            canvasGroup.alpha = 0;
            levelCanvas.canvas.enabled = false;
        }
        if (levelCanvases.Length > 0)
        {
            levelCanvases[0].canvas.enabled = true;
            SaveLevelName(levelCanvases[0].canvas.name);
        }
    }

    void UpdateCanvasVisibility()
    {
        for (int i = 0; i < levelCanvases.Length; i++)
        {
            if (i == currentIndex)
            {
                StartCoroutine(FadeCanvas(levelCanvases[i].canvas, 1, fadeDuration));
                levelCanvases[i].canvas.enabled = true;
                SaveLevelName(levelCanvases[i].canvas.name);

                // Check if all keys in the current canvas have a value of 1
                if (AllKeysAre1(levelCanvases[i].keysToCheck))
                {
                    levelCanvases[i].nextButton.interactable = true;
                }
                else
                {
                    levelCanvases[i].nextButton.interactable = false;
                }
            }
            else
            {
                StartCoroutine(FadeCanvas(levelCanvases[i].canvas, 0, fadeDuration));
                levelCanvases[i].canvas.enabled = false;
            }
        }
    }

    IEnumerator FadeCanvas(Canvas canvas, float targetAlpha, float duration)
    {
        CanvasGroup canvasGroup = canvas.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = canvas.gameObject.AddComponent<CanvasGroup>();
        }

        float startAlpha = canvasGroup.alpha;
        float time = 0;

        while (time < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
    }

    bool AllKeysAre1(string[] keys)
    {
        foreach (string key in keys)
        {
            if (PlayerPrefs.GetInt(key, 0) != 1)
            {
                return false;
            }
        }
        return true;
    }

    void ShowNextCanvas(LevelCanvas levelCanvas)
    {
        int index = System.Array.IndexOf(levelCanvases, levelCanvas);
        if (index < levelCanvases.Length - 1)
        {
            currentIndex = index + 1;
            UpdateCanvasVisibility();
        }
    }

    void ShowPreviousCanvas(LevelCanvas levelCanvas)
    {
        int index = System.Array.IndexOf(levelCanvases, levelCanvas);
        if (index > 0)
        {
            currentIndex = index - 1;
            UpdateCanvasVisibility();
        }
    }

    void PlayBriefingAudio(LevelCanvas levelCanvas)
    {
        if (!levelCanvas.briefingAudio.isPlaying)
        {
            levelCanvas.briefingAudio.Play();
        }
    }

    void StopBriefingAudio(LevelCanvas levelCanvas)
    {
        if (levelCanvas.briefingAudio.isPlaying)
        {
            levelCanvas.briefingAudio.Stop();
        }
    }

    void SaveLevelName(string levelName)
    {
        PlayerPrefs.SetString(levelPrefKey, levelName);
        PlayerPrefs.Save();
    }

    public string LoadLevelName()
    {
        return PlayerPrefs.GetString(levelPrefKey, "DefaultLevel");
    }

    public void LoadLevelByName(string levelName)
    {
        // Find the index of the saved level name in the levelCanvases array
        for (int i = 0; i < levelCanvases.Length; i++)
        {
            if (levelCanvases[i].canvas.name == levelName)
            {
                // Set the current index to the saved level index
                currentIndex = i;
                UpdateCanvasVisibility();

                // Load the scene here using SceneManager.LoadScene or your preferred method
                SceneManager.LoadScene(levelName);
                break;
            }
        }
    }
}
