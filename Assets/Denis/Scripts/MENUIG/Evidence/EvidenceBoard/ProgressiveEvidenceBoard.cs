using UnityEngine;

public class SpawnChecker : MonoBehaviour
{
    [System.Serializable]
    public class ObjectPair
    {
        public string key; // The PlayerPrefs key to check
        public GameObject objectToSpawn; // Assign in Inspector
    }

    public ObjectPair[] objectPairs; // Assign in Inspector

    void Start()
    {
        // Loop through each object pair
        foreach (ObjectPair pair in objectPairs)
        {
            // Get the PlayerPrefs value for the key
            int value = PlayerPrefs.GetInt(pair.key, 0);

            // Enable or disable objectToSpawn based on PlayerPrefs value
            if (value == 1)
            {
                if (pair.objectToSpawn != null)
                {
                    pair.objectToSpawn.SetActive(true);
                }
                else
                {
                    Debug.LogWarning("Object to spawn is not assigned.");
                }
            }
            else
            {
                if (pair.objectToSpawn != null)
                {
                    pair.objectToSpawn.SetActive(false);
                }
            }
        }
    }
}
