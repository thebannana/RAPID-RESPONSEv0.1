using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    public enum OfficerState
    {
        Incapacitated,
        Idle,
        Walking,
        WalkingToDoor,
        OpeningClosingDoor,
        WalkingToDetain,
        Detaining,
        WalkingToCollectEvidence,
        CollectingEvidence,
        WalkingToArrest,
        Arresting,
        AutonomousAttack,
        WalkingToAttack,
        OrderedAttack,
        Throwing
    }

    public enum Throwables
    {
        Stinger,
        Flashbang
    }


    // Health
    public bool isIncapacitated = false;
    public float unitHealth;
    public float unitMaxHealth = 100f;
    private bool hasArmor = true;
    public bool isHealthyRightArm = true;
    public bool isHealthyLeftArm = true;
    public bool isHealthyRightLeg = true;
    public bool isHealthyLeftLeg = true;
    public HealthTracker healthTracker;


    //Combat (default settings are for 5.56 rifles)
    public int weaponID = 1;
    private bool isLethal = true;
    private float damage = 33.4f;
    public float rateOfFire = 2.0f;
    private float range = 50.0f;
    private float precision = 90.0f;
    public Throwables equipedThrowable = Throwables.Stinger;
    private float throwingForce = 15f;
    public GameObject stingerPrefab;
    public GameObject flashbangPrefab;
    public int throwablesAmount = 3;
    public List<GameObject> orderedTarget = new List<GameObject>();
    public List<GameObject> autoTarget = new List<GameObject>();
    public List<GameObject> suspectsInCollider = new List<GameObject>();
    public List<GameObject> visibleSuspects = new List<GameObject>();

    //Interaction & movement
    NavMeshAgent agent;
    private float interactionRange = 1.5f;
    public List<GameObject> civilianToDetain = new List<GameObject>();
    public List<GameObject> evidenceToCollect = new List<GameObject>();
    public List<GameObject> suspectToArrest = new List<GameObject>();


    //Other
    public OfficerState currentState = OfficerState.Idle;
    private float timer = 0f;
    public LayerMask suspectLayer;
    public LayerMask wallLayer;
    public LayerMask coverLayer;
    private float viewRange = 11.0f;
    private List<GameObject> doorToOpenClose = new List<GameObject>();
    private float shotTimer = 0f;


    void Start()
    {
        UnitSelectionManager.Instance.allUnitsList.Add(gameObject);
        LevelStatus.Instance.allUnits.Add(gameObject);
        LoadoutReader.Instance.allUnitsList.Add(gameObject);
        currentState = OfficerState.Idle;
        agent = GetComponent<NavMeshAgent>();

        unitHealth = unitMaxHealth;
        UpdateHealthUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Suspect"))
        {
            GameObject suspectGameObject = other.gameObject;
            if (!suspectsInCollider.Contains(suspectGameObject))
            {
                suspectsInCollider.Add(suspectGameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Suspect"))
        {
            GameObject suspectGameObject = other.gameObject;
            suspectsInCollider.Remove(suspectGameObject);
        }
    }

    void Update()
    {
        if(unitHealth <= 0 && isIncapacitated != true)
        {
            isIncapacitated = true;
            this.gameObject.transform.GetChild(4).gameObject.GetComponent<AudioSource>().Play();
            currentState = OfficerState.Incapacitated;
            LevelStatus.Instance.incapacitatedUnits.Add(gameObject);
            this.gameObject.transform.GetChild(1).gameObject.SetActive(true);
        }

        if(currentState != OfficerState.Incapacitated)
        {
            DetectSuspects();
        }

        switch(currentState)
        {
            case OfficerState.Incapacitated:
                RunningAudio(false);
                agent.isStopped = true;
                break;

            case OfficerState.Walking:
                RunningAudio(true);
                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && !agent.hasPath)
                {
                    currentState = OfficerState.Idle;
                }
                break;

            case OfficerState.WalkingToDoor:
                RunningAudio(true);
                if (doorToOpenClose.Count <= 0)
                {
                    doorToOpenClose.Clear();
                    currentState = OfficerState.Idle;
                }
                float distanceFromOpenCloseDoor = Vector3.Distance(this.gameObject.transform.position, doorToOpenClose[0].transform.position);
                if(distanceFromOpenCloseDoor <= 3.5)
                {
                    currentState = OfficerState.OpeningClosingDoor;
                }
                else
                {
                    agent.destination = doorToOpenClose[0].transform.position;
                }
                break;

            case OfficerState.OpeningClosingDoor:
                RunningAudio(false);
                timer += Time.deltaTime;

                LookAt(doorToOpenClose[0].transform.GetChild(0).transform.GetChild(0).gameObject);
                if(timer >= 1) // 1 is how much time it takes to detain
                {
                    agent.destination = this.gameObject.transform.position;
                    doorToOpenClose[0].GetComponent<Door>().InteractWithDoor();
                    doorToOpenClose.Clear();
                    timer = 0;  
                    currentState = OfficerState.Idle;
                }
                break;

            case OfficerState.WalkingToDetain:
                RunningAudio(true);
                if (civilianToDetain.Count <= 0 || civilianToDetain[0].GetComponent<Civilian>().isDetained == true)
                {
                    civilianToDetain.Clear();
                    currentState = OfficerState.Idle;
                }
                float distanceFromCivilian = Vector3.Distance(this.gameObject.transform.position, civilianToDetain[0].transform.position);
                if(distanceFromCivilian <= interactionRange)
                {
                    agent.destination = this.gameObject.transform.position;
                    currentState = OfficerState.Detaining;
                }
                else
                {
                    agent.destination = civilianToDetain[0].transform.position;
                }
                break;

            case OfficerState.Detaining:
                RunningAudio(false);
                timer += Time.deltaTime;

                LookAt(civilianToDetain[0]);
                if(timer >= 3) // 3 is how much time it takes to detain
                {
                    civilianToDetain[0].GetComponent<Civilian>().Detain();
                    civilianToDetain.Clear();
                    timer = 0;  
                    currentState = OfficerState.Idle;
                }
                break;

            case OfficerState.WalkingToArrest:
                RunningAudio(true);
                if (suspectToArrest.Count <= 0 || suspectToArrest[0].GetComponent<Suspect>().isInactive == true || suspectToArrest[0].GetComponent<Suspect>().isSurrendered == false)
                {
                    suspectToArrest.Clear();
                    currentState = OfficerState.Idle;
                }
                float distanceFromSuspect = Vector3.Distance(this.gameObject.transform.position, suspectToArrest[0].transform.position);
                if(distanceFromSuspect <= interactionRange)
                {
                    agent.destination = this.gameObject.transform.position;
                    currentState = OfficerState.Arresting;
                }
                else
                {
                    agent.destination = suspectToArrest[0].transform.position;
                }
                break;

            case OfficerState.Arresting:
                RunningAudio(false);
                timer += Time.deltaTime;

                LookAt(suspectToArrest[0]);
                if(timer >= 3) // 3 is how much time it takes to arrest
                {
                    suspectToArrest[0].GetComponent<Suspect>().Arrest();
                    suspectToArrest.Clear();
                    timer = 0;  
                    currentState = OfficerState.Idle;
                }
                break;

            case OfficerState.WalkingToCollectEvidence:
                RunningAudio(true);
                if (evidenceToCollect.Count <= 0 || evidenceToCollect[0].GetComponent<Evidence>().isCollected == true)
                {
                    evidenceToCollect.Clear();
                    currentState = OfficerState.Idle;
                }
                float distanceFromEvidence = Vector3.Distance(this.gameObject.transform.position, evidenceToCollect[0].transform.position);
                if(distanceFromEvidence <= interactionRange)
                {
                    agent.destination = this.gameObject.transform.position;
                    currentState = OfficerState.CollectingEvidence;
                }
                else
                {
                    agent.destination = evidenceToCollect[0].transform.position;
                }
                break;

            case OfficerState.CollectingEvidence:
                RunningAudio(false);
                timer += Time.deltaTime;

                LookAt(evidenceToCollect[0]);
                if(timer >= 2) // 2 is how much time it takes to collect
                {
                    evidenceToCollect[0].GetComponent<Evidence>().Collect();
                    evidenceToCollect.Clear();
                    timer = 0;  
                    currentState = OfficerState.Idle;
                }
                break;

            case OfficerState.WalkingToAttack:
                RunningAudio(true);
                if (orderedTarget.Count <= 0 || orderedTarget[0].GetComponent<Suspect>().isInactive == true || orderedTarget[0].GetComponent<Suspect>().isSurrendered == true)
                {
                    orderedTarget.Clear();
                    currentState = OfficerState.Idle;
                }

                float distanceFromOrderedTarget = Vector3.Distance(this.gameObject.transform.position, orderedTarget[0].transform.position);
                if (visibleSuspects.Contains(orderedTarget[0]) && distanceFromOrderedTarget < range)
                {
                    agent.destination = this.gameObject.transform.position;
                    currentState = OfficerState.OrderedAttack;
                }
                else
                {
                    agent.destination = orderedTarget[0].transform.position;
                }
                
                break;

            case OfficerState.OrderedAttack:
                RunningAudio(false);
                OrdAttack(orderedTarget[0]);
                break;

            case OfficerState.AutonomousAttack:
                RunningAudio(false);
                AutoAttack(autoTarget[0]);
                break;

            case OfficerState.Throwing:
                RunningAudio(false);
                timer += Time.deltaTime;

                if(timer >= 1)
                {
                    Throw();
                    timer = 0;
                    currentState = OfficerState.Idle;
                }
                break;

            case OfficerState.Idle:
                RunningAudio(false);
                timer = 0;
                if(agent.remainingDistance > agent.stoppingDistance && agent.hasPath)
                {
                    currentState = OfficerState.Walking;
                }

                if(visibleSuspects.Count > 0)
                {
                    foreach(var visibleSuspect in visibleSuspects)
                    {
                        if(visibleSuspect.GetComponent<Suspect>().isInactive == false && visibleSuspect.GetComponent<Suspect>().isSurrendered == false)
                        {
                            autoTarget.Clear();
                            autoTarget.Add(visibleSuspect);
                            timer = 0;
                            currentState = OfficerState.AutonomousAttack;
                        }
                    }
                }
                break;
        }
    }

    private void RunningAudio(bool trigger)
    {
        if (trigger && !this.gameObject.transform.GetChild(2).gameObject.GetComponent<AudioSource>().isPlaying) 
        {
            this.gameObject.transform.GetChild(2).gameObject.GetComponent<AudioSource>().Play();
        }
    }

    private void OnDestroy()
    {
        UnitSelectionManager.Instance.allUnitsList.Remove(gameObject);
    }

    public void ResetState()
    {
        timer = 0;
        currentState = OfficerState.Idle;
    }

    public void TakeDamage(GameObject suspect, float damage, float precision, float distance)
    {
        float hitChance = 100;

        Vector3 directionToSuspect = suspect.transform.position - transform.position;

        Ray ray = new Ray(transform.position, directionToSuspect);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, distance, coverLayer))
        {
            if(hit.distance < 1.5) // If the cover is very close to the suspect, it means that it shouldn't behave like its cover for the unit
            {
                hitChance -= 20; 
            }
        }

        hitChance -= 100 - precision;
        //Debug.Log("hitChance " + hitChance);

        int roll = UnityEngine.Random.Range(1, 101);

        if((float)roll < hitChance)
        {
            int rollBodyPart = UnityEngine.Random.Range(1, 11);

            if(rollBodyPart == 1)
            {
                float damageToLimb = (damage / 3) * 2; // If a limb is hit, the total damage impact is 66%
                unitHealth -= damageToLimb;

                isHealthyLeftArm = false;
            }
            else if(rollBodyPart == 2)
            {
                float damageToLimb = (damage / 3) * 2;
                unitHealth -= damageToLimb;

                isHealthyLeftLeg = false;
            }
            else if(rollBodyPart == 3)
            {
                float damageToLimb = (damage / 3) * 2;
                unitHealth -= damageToLimb;

                isHealthyRightArm = false;
            }
            else if(rollBodyPart == 4)
            {
                float damageToLimb = (damage / 3) * 2;
                unitHealth -= damageToLimb;

                isHealthyRightLeg = false;
            }
            else
            {
                if(hasArmor == true)
                {
                    int chanceToBrakeArmor = UnityEngine.Random.Range(1, 11);
                    if(chanceToBrakeArmor <= 3 && damage > 20)
                    {
                        hasArmor = false;
                    }

                    float damageTroughArmor = (damage / 4) * 3; // Reduces damage taken by 25%
                    unitHealth -= damageTroughArmor;
                }
                else
                {
                    unitHealth -= damage;
                }
            }

            UpdateHealthUI();

        }

    }

    private void OrdAttack(GameObject suspect)
    {
        if(suspect.GetComponent<Suspect>().isInactive == true || suspect.GetComponent<Suspect>().isSurrendered == true)
        {
            autoTarget.Clear();
            currentState = OfficerState.Idle;
        }

        if(!visibleSuspects.Contains(suspect))
        {
            currentState = OfficerState.WalkingToAttack;
        }

        if(agent.hasPath)
        {
            autoTarget.Clear();
            currentState = OfficerState.Walking;
        }

        RotateTowardSuspect(suspect);
        float distanceFromTarget = Vector3.Distance(this.gameObject.transform.position, suspect.transform.position);
        if(distanceFromTarget <= range)
        {
                
            if(timer >= rateOfFire)
            {
                timer = 0;
                suspect.GetComponent<Suspect>().TakeDamage(this.gameObject, damage, precision, isLethal, distanceFromTarget);
                StartCoroutine(ShootEffect());
                this.gameObject.transform.GetChild(3).gameObject.GetComponent<AudioSource>().Play();
            } 

            timer += Time.deltaTime;
        }
        else
        {
            currentState = OfficerState.WalkingToAttack;
        }

    }

    private void AutoAttack(GameObject suspect)
    {
        if(suspect.GetComponent<Suspect>().isInactive == true || suspect.GetComponent<Suspect>().isSurrendered == true || suspect.GetComponent<Suspect>().isConcussed == true)
        {
            autoTarget.Clear();
            currentState = OfficerState.Idle;
        }

        if(!visibleSuspects.Contains(suspect))
        {
            autoTarget.Clear();
            currentState = OfficerState.Idle;
        }

        if(agent.hasPath)
        {
            autoTarget.Clear();
            currentState = OfficerState.Walking;
        }

        RotateTowardSuspect(suspect);
        float distanceFromTarget = Vector3.Distance(this.gameObject.transform.position, suspect.transform.position);
        if(distanceFromTarget <= range)
        {
                
            if(timer >= rateOfFire)
            {
                timer = 0;
                suspect.GetComponent<Suspect>().TakeDamage(this.gameObject, damage, precision, isLethal, distanceFromTarget);
                StartCoroutine(ShootEffect());
                this.gameObject.transform.GetChild(3).gameObject.GetComponent<AudioSource>().Play();
            } 

            timer += Time.deltaTime;
        }

    }

    private IEnumerator ShootEffect()
    {
        // Enable the fifth child
        this.gameObject.transform.GetChild(5).gameObject.SetActive(true);
        // Wait for a short duration (e.g., 0.1 seconds)
        yield return new WaitForSeconds(0.1f);
        // Disable the fifth child
        this.gameObject.transform.GetChild(5).gameObject.SetActive(false);
    }


    private void DetectSuspects()
    {

        foreach(var suspect in suspectsInCollider)
        {
            Vector3 directionToSuspect = suspect.transform.position - transform.position;

            Ray ray = new Ray(transform.position, directionToSuspect);
            RaycastHit hit;

            // Perform the raycast with the layer mask
            if(Physics.Raycast(ray, out hit, viewRange, suspectLayer | wallLayer))
            {
                // Check if the hit object is the suspect
                if(hit.collider.CompareTag("Suspect") && hit.collider.gameObject == suspect.gameObject)
                {
                    // The ray hit a suspect
                    if(!visibleSuspects.Contains(suspect))
                    {
                        visibleSuspects.Add(suspect);
                    }
                    //Debug.Log("Suspect is visible!");
                }

                if(hit.collider.CompareTag("Wall"))
                {
                    // The ray hit a wall
                    if(visibleSuspects.Contains(suspect))
                    {
                        visibleSuspects.Remove(suspect);
                    }
                    //Debug.Log("Suspect is mot visible!");
                }
            }

        }

    }

    public void ThrowThrowable()
    {
        ClearAllLists();
        currentState = OfficerState.Throwing;
    }

    private void Throw()
    {
        if(throwablesAmount > 0)
        {
            if(equipedThrowable == Throwables.Stinger)
            {
                GameObject sting = Instantiate(stingerPrefab, transform.position, transform.rotation);
                Rigidbody rb = sting.GetComponent<Rigidbody>();
                rb.AddForce(transform.forward * throwingForce, ForceMode.VelocityChange);
            }
            else if(equipedThrowable == Throwables.Flashbang)
            {
                GameObject flash = Instantiate(flashbangPrefab, transform.position, transform.rotation);
                Rigidbody rb = flash.GetComponent<Rigidbody>();
                rb.AddForce(transform.forward * throwingForce, ForceMode.VelocityChange);                
            }

            throwablesAmount--;
        }

    }

    public void OrderToAttackSuspect(GameObject suspect)
    {
        ClearAllLists();

        orderedTarget.Add(suspect);
        currentState = OfficerState.WalkingToAttack;
    }

    public void OrderToArrestSuspect(GameObject suspectObj)
    {
        ClearAllLists();

        suspectToArrest.Add(suspectObj);
        currentState = OfficerState.WalkingToArrest;
    }

    public void OrderToDetainCivilian(GameObject civilianObj)
    {
        ClearAllLists();

        civilianToDetain.Add(civilianObj);
        currentState = OfficerState.WalkingToDetain;
    }

    public void CollectEvidence(GameObject evidenceObj)
    {
        ClearAllLists();

        evidenceToCollect.Add(evidenceObj);
        currentState = OfficerState.WalkingToCollectEvidence;
    }

    public void OpenCloseDoor(GameObject doorObj)
    {
        ClearAllLists();

        doorToOpenClose.Add(doorObj);
        currentState = OfficerState.WalkingToDoor;
    }

    private void LookAt(GameObject obj)
    {
        Vector3 direction = obj.transform.position - agent.transform.position;
        agent.transform.rotation = Quaternion.LookRotation(direction);

        var yRotation = agent.transform.eulerAngles.y;
        agent.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    private void RotateTowardSuspect(GameObject obj)
    {
        Vector3 direction = obj.transform.position - agent.transform.position;
        agent.transform.rotation = Quaternion.LookRotation(direction);

        var yRotation = agent.transform.eulerAngles.y + 70;
        agent.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    private void UpdateHealthUI()
    {
        if(unitHealth > 0)
        {
            healthTracker.UpdateSliderValue(unitHealth, unitMaxHealth);
        }
        else
        {
            healthTracker.UpdateSliderValue(1.0f, unitMaxHealth);
        }
    }

    private void ClearAllLists()
    {
        suspectToArrest.Clear();
        civilianToDetain.Clear();
        evidenceToCollect.Clear();
        orderedTarget.Clear();
        autoTarget.Clear();
        doorToOpenClose.Clear();
    }

    public void GetEquiped(bool _isLethal, float _damage, float _rateOfFire, float _range, float _precision, int _throwableID, int _weaponID)
    {
        isLethal = _isLethal;
        damage = _damage;
        rateOfFire = _rateOfFire;
        range = _range;
        precision = _precision;
        weaponID = _weaponID;

        switch(_throwableID)
        {
            case 1:
                equipedThrowable = Throwables.Flashbang;
                break;
            case 2:
            equipedThrowable = Throwables.Stinger;
                break;
        }
        
    }

}