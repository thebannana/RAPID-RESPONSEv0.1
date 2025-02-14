using UnityEngine;

public class CanvasToggleScript : MonoBehaviour
{
    public Canvas targetCanvas;

    private bool isPaused = false;

    private void Start()
    {
        if (targetCanvas == null)
        {
            Debug.LogError("Target canvas is not assigned.");
        }
        else
        {
            targetCanvas.enabled = false; // Ensure the canvas is initially disabled
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (targetCanvas != null)
            {
                isPaused = !isPaused;
                targetCanvas.enabled = isPaused;
                Time.timeScale = isPaused ? 0 : 1;
            }
        }
    }
}
