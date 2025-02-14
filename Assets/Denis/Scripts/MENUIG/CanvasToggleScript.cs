using UnityEngine;

public class CanvasToggle : MonoBehaviour
{
    public Canvas targetCanvas;
    public Collider modelCollider;

    private void Start()
    {
        if (targetCanvas == null)
        {
            Debug.LogError("Target canvas is not assigned.");
        }

        if (modelCollider == null)
        {
            Debug.LogError("Model collider is not assigned.");
        }

        // Ensure the canvas is initially disabled
        targetCanvas.enabled = false;
    }

    private void Update()
    {
        // Check for ESC key press to disable the canvas
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            targetCanvas.enabled = false;
        }
    }

    private void OnMouseDown()
    {
        // Check if the mouse is over the model's collider
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (modelCollider.Raycast(ray, out hit, Mathf.Infinity))
        {
            // Toggle the canvas visibility
            targetCanvas.enabled = true;
        }
    }
}
