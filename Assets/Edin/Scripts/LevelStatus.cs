using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStatus : MonoBehaviour
{
    public static LevelStatus Instance { get; set; }
    
    public List<GameObject> allUnits = new List<GameObject>();
    public List<GameObject> incapacitatedUnits = new List<GameObject>();
    public List<GameObject> allSuspects = new List<GameObject>();
    public List<GameObject> inactiveSuspects = new List<GameObject>();
    public List<GameObject> killedSuspects = new List<GameObject>();
    public List<GameObject> arrestedSuspects = new List<GameObject>();
    public List<GameObject> allCivilians = new List<GameObject>();
    public List<GameObject> detainedCivilians = new List<GameObject>();
    public List<GameObject> allEvidence = new List<GameObject>();
    public List<GameObject> collectedEvidence = new List<GameObject>();

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
}
