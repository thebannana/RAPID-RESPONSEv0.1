using UnityEngine;

public class EvidenceManager : MonoBehaviour
{
    public GameObject[] evidences;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }


    void Start()
    {
        //ClearPlayerPrefs();
        foreach (GameObject evidence in evidences)
        {
            if (evidence == null)
            {
                Debug.LogError("Evidence GameObject is null in the evidences array.");
                continue;
            }

            string evidenceName = evidence.name;
            int pickedUp = PlayerPrefs.GetInt(evidenceName, 0);

            Debug.Log($"Initializing evidence: {evidenceName} with value: {pickedUp}");

            if (pickedUp == 1)
            {
                evidence.SetActive(true);
                Debug.Log($"{evidenceName} is picked up and set inactive.");
            }
            else
            {
                evidence.SetActive(true);
                Debug.Log($"{evidenceName} is not picked up and remains active.");
            }
        }
    }

    public void PickUpEvidence(GameObject evidence)
    {
        if (evidence == null)
        {
            Debug.LogError("Attempted to pick up a null evidence GameObject.");
            return;
        }

        string evidenceName = evidence.name;
        PlayerPrefs.SetInt(evidenceName, 1);
        PlayerPrefs.Save();
        evidence.SetActive(false);
        Debug.Log($"{evidenceName} picked up and set inactive.");
    }

    public bool IsEvidencePickedUp(string evidenceName)
    {
        return PlayerPrefs.GetInt(evidenceName, 0) == 1;
    }

    // Method to clear all PlayerPrefs
    public void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("All PlayerPrefs cleared.");
    }

    // Method to manually set PlayerPrefs for testing
    public void SetTestPlayerPrefs()
    {
        foreach (GameObject evidence in evidences)
        {
            if (evidence != null)
            {
                string evidenceName = evidence.name;
                PlayerPrefs.SetInt(evidenceName, 1);
            }
        }
        PlayerPrefs.Save();
        Debug.Log("Test PlayerPrefs set.");
    }
}
