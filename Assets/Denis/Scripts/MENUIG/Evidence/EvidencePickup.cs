using UnityEngine;

public class EvidencePickup : MonoBehaviour
{
    private EvidenceManager evidenceManager;

    void Start()
    {
        evidenceManager = FindObjectOfType<EvidenceManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PoliceOfficer"))
        {
            evidenceManager.PickUpEvidence(gameObject);
            Debug.Log($"{gameObject.name} picked up and PlayerPrefs set.");
        }
    }
}
