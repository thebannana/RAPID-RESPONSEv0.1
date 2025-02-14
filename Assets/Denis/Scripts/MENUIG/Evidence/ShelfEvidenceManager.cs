using UnityEngine;

public class ShelfEvidenceManager : MonoBehaviour
{
    public GameObject[] shelfEvidences;

    void Start()
    {
        InitializeShelfEvidences();
    }

    private void InitializeShelfEvidences()
    {
        foreach (GameObject evidence in shelfEvidences)
        {
            if (evidence == null)
            {
                Debug.LogError("Shelf evidence GameObject is null in the shelfEvidences array.");
                continue;
            }

            string evidenceName = evidence.name;
            int pickedUp = PlayerPrefs.GetInt(evidenceName, 0);

            Debug.Log($"Checking shelf evidence: {evidenceName} with value: {pickedUp}");

            if (pickedUp == 1)
            {
                evidence.SetActive(true);
                Debug.Log($"{evidenceName} was picked up and is now visible on the shelf.");
            }
            else
            {
                evidence.SetActive(false);
                Debug.Log($"{evidenceName} was not picked up and remains hidden.");
            }
        }
    }
}
