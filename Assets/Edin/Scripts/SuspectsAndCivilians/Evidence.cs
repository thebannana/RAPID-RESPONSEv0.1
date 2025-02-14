using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evidence : MonoBehaviour
{   
    public bool isCollected = false;
    private EvidenceManager evidenceManager;

    void Start()
    {
        LevelStatus.Instance.allEvidence.Add(gameObject);
        evidenceManager = FindObjectOfType<EvidenceManager>();
    }

    public void Collect()
    {
        isCollected = true;
        evidenceManager.PickUpEvidence(gameObject);
        Debug.Log($"{gameObject.name} picked up and PlayerPrefs set.");
        LevelStatus.Instance.collectedEvidence.Add(gameObject);
        gameObject.SetActive(false);
    }
}
