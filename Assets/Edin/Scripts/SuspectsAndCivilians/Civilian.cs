using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Civilian : MonoBehaviour
{
    public enum CivilianState
    {
        Idle,
        Wandering,
        Detained
    }
    public bool isDetained = false;
    public CivilianState currentState = CivilianState.Idle;

    NavMeshAgent agent;
    public float wanderingRadius = 10f;
    public int wanderingChance = 10;
    private float rollInterval = 15f;
    private float rollTimer = 0f;


    // Start is called before the first frame update
    void Start()
    {
        LevelStatus.Instance.allCivilians.Add(gameObject);
        currentState = CivilianState.Idle;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case CivilianState.Detained:
                RunningAudio(false);
                agent.isStopped = true;
                break;
                
            case CivilianState.Wandering:
                RunningAudio(true);
                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                {
                    currentState = CivilianState.Idle; 
                }
                break;

            case CivilianState.Idle:
                RunningAudio(false);
                rollTimer += Time.deltaTime;
                if (rollTimer >= rollInterval)
                {
                    int roll = UnityEngine.Random.Range(1, 101);
                    if(roll < wanderingChance)
                    {
                        currentState = CivilianState.Wandering;
                        Wander();
                    }

                    rollTimer = 0;
                }
                break;
        }
    }

    private void RunningAudio(bool trigger)
    {
        if (trigger && !this.gameObject.transform.GetChild(1).gameObject.GetComponent<AudioSource>().isPlaying)
        {
            this.gameObject.transform.GetChild(1).gameObject.GetComponent<AudioSource>().Play();
        }
    }
    public void Detain()
    {
        isDetained = true;
        currentState = CivilianState.Detained;
        LevelStatus.Instance.detainedCivilians.Add(gameObject);
        this.transform.GetChild(0).gameObject.SetActive(true);
    }

    private void Wander()
    {
        Vector3 randomPoint = RandomNavMeshPoint(transform.position, wanderingRadius);
        agent.SetDestination(randomPoint);
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
