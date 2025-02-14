using UnityEngine;

public class LoadLevelFromPrefs : MonoBehaviour
{
    public LevelSelectManager levelSelectManager; // Reference to the LevelSelectManager
    private string levelPrefKey = "SelectedLevel"; // Key for saving the level name to PlayerPrefs

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
                if (hit.collider != null && hit.collider.gameObject == gameObject)
                {
                    // Load the saved level name from PlayerPrefs
                    string savedLevelName = PlayerPrefs.GetString(levelPrefKey, null);

                    if (!string.IsNullOrEmpty(savedLevelName))
                    {
                        // Call the method to change the level to the saved scene
                        levelSelectManager.LoadLevelByName(savedLevelName);
                    }
                }
            }
        }
    }
}
