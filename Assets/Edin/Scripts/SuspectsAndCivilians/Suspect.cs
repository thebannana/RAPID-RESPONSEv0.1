using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Suspect : MonoBehaviour
{
    public enum SuspectState
    {
        Idle,
        Patroling,
        Concussed,
        Flee,
        Surrendered,
        Attack,
        Killed,
        Arrested
    }

    // Health
    public bool isInactive = false; // Arrested or Killed
    public bool isSurrendered = false;
    public bool isConcussed = false;
    private bool isArrested = false;
    public float suspectHealth;
    public float suspectMaxHealth = 100f;
    public bool hasArmor = false;
    public float suspectWill;
    public float suspectMaxWill = 100f;


    // Combat (default settings are for 9mm pistols)
    public float damage = 16.7f;
    public float rateOfFire = 3.0f;
    public float range = 25.0f;
    public float precision = 80.0f;
    public List<GameObject> currentTarget = new List<GameObject>();


    // Movement & other
    public SuspectState currentState = SuspectState.Idle;
    NavMeshAgent agent;
    public float patrolRadius = 5f;
    public int patrolChance = 20;
    private float rollInterval = 15f;
    private float rollTimer = 0f;
    private float surrenderTimer = 0f;
    private float concussionTimer = 0f;
    private float interactionRange = 1f;

    // Player detection
    public LayerMask officerLayer;
    public LayerMask wallLayer;
    public LayerMask coverLayer;
    private float viewRange = 12f;
    public List<GameObject> officersInCollider = new List<GameObject>();
    public List<GameObject> visibleOfficers = new List<GameObject>();
    public List<GameObject> officerTarget = new List<GameObject>();
    private float timer = 0f;



    void Start()
    {
        LevelStatus.Instance.allSuspects.Add(gameObject);
        currentState = SuspectState.Idle;
        agent = GetComponent<NavMeshAgent>();
        
        suspectWill = suspectMaxWill;
        suspectHealth = suspectMaxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PoliceOfficer"))
        {
            GameObject officerGameObject = other.gameObject;
            if (!officersInCollider.Contains(officerGameObject))
            {
                officersInCollider.Add(officerGameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PoliceOfficer"))
        {
            GameObject officerGameObject = other.gameObject;
            officersInCollider.Remove(officerGameObject);
        }
    }

    void Update()
    {
        if(suspectHealth <= 0 && isInactive!= true)
        {
            this.gameObject.transform.GetChild(2).gameObject.GetComponent<AudioSource>().Play();
            isInactive = true;
            currentState = SuspectState.Killed;
            LevelStatus.Instance.inactiveSuspects.Add(gameObject);
            LevelStatus.Instance.killedSuspects.Add(gameObject);
            this.transform.GetChild(0).gameObject.SetActive(true);
        }

        if(suspectWill <= 0)
        {
            currentState = SuspectState.Surrendered;
        }

        if(isArrested == true)
        {
            currentState = SuspectState.Arrested;
        }

        if(currentState != SuspectState.Killed && currentState != SuspectState.Arrested)
        {
            DetectOfficers();
        }

        switch(currentState)
        {
            case SuspectState.Killed:
                RunningAudio(false);
                agent.isStopped = true;
                this.enabled = false;
                break;

            case SuspectState.Arrested:
                RunningAudio(false);
                agent.isStopped = true;
                this.enabled = false;
                break;

            case SuspectState.Surrendered:
                RunningAudio(false);
                surrenderTimer += Time.deltaTime;
                //Debug.Log("Surrender timer: " + surrenderTimer);
                isSurrendered = true;

                if(suspectHealth <= 0)
                {
                    isInactive = true;
                    currentState = SuspectState.Killed;
                    this.transform.GetChild(0).gameObject.SetActive(true);
                }

                if(surrenderTimer >= 20) // 20 is the amount of time that the suspect stays surrendered
                {
                    surrenderTimer = 0;
                    suspectWill += 15;
                    isSurrendered = false;
                    currentState = SuspectState.Idle;
                }
                break;

            case SuspectState.Flee:
                RunningAudio(true);
                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                {
                    currentState = SuspectState.Idle; 
                }
                break;

            case SuspectState.Concussed:
                RunningAudio(false);
                isConcussed = true;
                concussionTimer += Time.deltaTime;

                if(concussionTimer >= 10) // 10 is the amount of time that the suspect stays concussed
                {
                    isConcussed = false;
                    int concussionRoll = UnityEngine.Random.Range(1, 11);

                    if(concussionRoll <= 7)
                    {
                        currentState = SuspectState.Surrendered;
                    }
                    else
                    {
                        Flee();
                        currentState = SuspectState.Flee;
                    }
                }
                break;

            case SuspectState.Patroling:
                RunningAudio(true);
                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                {
                    currentState = SuspectState.Idle; 
                }
                break;

            case SuspectState.Attack:
                RunningAudio(false);
                Attack(officerTarget[0]);
                break;

            case SuspectState.Idle:
                RunningAudio(false);
                rollTimer += Time.deltaTime;

                if(visibleOfficers.Count > 0)
                {
                    foreach(var visibleOfficer in visibleOfficers)
                    {
                        if(visibleOfficer.GetComponent<Unit>().isIncapacitated == false)
                        {
                            officerTarget.Clear();
                            officerTarget.Add(visibleOfficer);
                            timer = 0;
                            currentState = SuspectState.Attack;
                        }
                    }
                }

                if (rollTimer >= rollInterval)
                {
                    int roll = UnityEngine.Random.Range(1, 101);
                    if(roll < patrolChance)
                    {
                        currentState = SuspectState.Patroling;
                        Patrol();
                    }

                    rollTimer = 0;
                }
                break;
        }
    }

    private void RunningAudio(bool trigger)
    {
        if (trigger && !this.gameObject.transform.GetChild(4).gameObject.GetComponent<AudioSource>().isPlaying)
        {
            this.gameObject.transform.GetChild(4).gameObject.GetComponent<AudioSource>().Play();
        }
    }
    public void TakeDamage(GameObject officer, float damage, float precision, bool isLethal, float distance)
    {
        float hitChance = 100;

        Vector3 directionToOfficer = officer.transform.position - transform.position;

        Ray ray = new Ray(transform.position, directionToOfficer);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, distance, coverLayer))
        {
            if(hit.distance < 1.5) // If the cover is very close to the officer, it means that it shouldn't behave like its cover for the suspect
            {
                hitChance -= 20; 
            }
        }

        hitChance -= 100 - precision;
        //Debug.Log("hitChance " + hitChance);

        int roll = UnityEngine.Random.Range(1, 101);

        if((float)roll < hitChance)
        {
            if(isLethal == true)
            {
                int willDamage = UnityEngine.Random.Range(1, 71);
                //Debug.Log("Will damage inflicted " + willDamage);

                if(hasArmor == true)
                {
                    willDamage /= 2; // Reduces randomly rolled will damage by half, but the rest of the will damage stays the same aka. equals to damage
                    suspectWill -= (float)willDamage; 

                    float damageTroughArmor = (damage / 4) * 3;  // Reduces damage taken by 25%
                    suspectHealth -= damageTroughArmor;
                }
                else
                {
                    suspectWill -= (float)willDamage;
                    suspectHealth -= damage;
                }

            }
            else
            {
                suspectWill -= damage;
            }
        }
    }

    private void Attack(GameObject officer)
    {
        if(officer.GetComponent<Unit>().isIncapacitated == true)
        {
            officerTarget.Clear();
            currentState = SuspectState.Idle;
        }

        if(!visibleOfficers.Contains(officer))
        {
            officerTarget.Clear();
            currentState = SuspectState.Idle;
        }

        if(agent.hasPath)
        {
            officerTarget.Clear();
            currentState = SuspectState.Patroling;
        }

        RotateTowardOfficer(officer);
        float distanceFromTarget = Vector3.Distance(this.gameObject.transform.position, officer.transform.position);
        if(distanceFromTarget <= range)
        {
                
            if(timer >= rateOfFire)
            {
                timer = 0;
                StartCoroutine(ShootEffect());
                this.gameObject.transform.GetChild(3).gameObject.GetComponent<AudioSource>().Play();
                officer.GetComponent<Unit>().TakeDamage(this.gameObject, damage, precision, distanceFromTarget);
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

    public void BecomeConcussed()
    {
        if(currentState != SuspectState.Killed && currentState != SuspectState.Arrested && isInactive == false)
        {
            currentState = SuspectState.Concussed;
        }
    }

    public void Arrest()
    {
        if(currentState == SuspectState.Surrendered)
        {
            isInactive = true;
            isArrested = true;
            LevelStatus.Instance.inactiveSuspects.Add(gameObject);
            LevelStatus.Instance.arrestedSuspects.Add(gameObject);
            this.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    private void Patrol()
    {
        Vector3 randomPoint = RandomNavMeshPoint(transform.position, patrolRadius); 
        agent.SetDestination(randomPoint);
    }

    private void Flee()
    {
        Vector3 randomPoint = RandomNavMeshPoint(transform.position, 15); // Flee radius is 15
        agent.SetDestination(randomPoint);
    }

    private void DetectOfficers()
    {

        foreach(var officer in officersInCollider)
        {
            Vector3 directionToOfficer = officer.transform.position - transform.position;

            Ray ray = new Ray(transform.position, directionToOfficer);
            RaycastHit hit;

            // Perform the raycast with the layer mask
            if(Physics.Raycast(ray, out hit, viewRange, officerLayer | wallLayer))
            {
                // Check if the hit object is the player officer
                if(hit.collider.CompareTag("PoliceOfficer") && hit.collider.gameObject == officer.gameObject)
                {
                    // The ray hit a suspect
                    if(!visibleOfficers.Contains(officer))
                    {
                        visibleOfficers.Add(officer);
                    }
                    //Debug.Log("Player officer is visible!");
                }

                if(hit.collider.CompareTag("Wall"))
                {
                    // The ray hit a wall
                    if(visibleOfficers.Contains(officer))
                    {
                        visibleOfficers.Remove(officer);
                    }
                    //Debug.Log("Player officer is mot visible!");
                }
            }

        }

    }

    private void LookAt(GameObject obj)
    {
        Vector3 direction = obj.transform.position - agent.transform.position;
        agent.transform.rotation = Quaternion.LookRotation(direction);

        var yRotation = agent.transform.eulerAngles.y;
        agent.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    private void RotateTowardOfficer(GameObject obj)
    {
        Vector3 direction = obj.transform.position - agent.transform.position;
        agent.transform.rotation = Quaternion.LookRotation(direction);

        var yRotation = agent.transform.eulerAngles.y + 70;
        agent.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    private Vector3 RandomNavMeshPoint(Vector3 origin, float distance)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;
        randomDirection += origin;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, distance, NavMesh.AllAreas);

        return hit.position;
    }
}
