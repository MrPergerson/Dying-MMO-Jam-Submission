using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AgentMoveToTarget : MonoBehaviour
{
    [Header("Debug")]
    public bool isMoving;
    NavMeshAgent navAgent;
    Vector3 targetLocation;

    Agent agent;

    public delegate void DestinationToAgentCompleted(Agent agent);

    private void Awake()
    {
        agent = GetComponent<Agent>();
        navAgent = GetComponent<NavMeshAgent>();
    }

    public void SetDestination(Vector3 destination)
    {
        navAgent.SetDestination(destination);
        targetLocation = destination;
        navAgent.stoppingDistance = 2f;
        isMoving = true;
        agent.Animator.SetFloat("Vertical", 1);
        StopAllCoroutines();
        //StartCoroutine(PlayWalkingSound());
        StartCoroutine(CheckForDestinationCompleted());
    }

    public void SetDestination(Vector3 destination, float stoppingDistance)
    {
        if (stoppingDistance == 0) stoppingDistance = 0.5f;
        navAgent.SetDestination(destination);
        navAgent.stoppingDistance = stoppingDistance;
        targetLocation = destination;
        //Debug.Log("setting destination-" + targetLocation);
        isMoving = true;
        agent.Animator.SetFloat("Vertical", 1);
        StopAllCoroutines();
        //StartCoroutine(PlayWalkingSound());
        StartCoroutine(CheckForDestinationCompleted());
    }

    public void SetDestination(Agent targetAgent, float stoppingDistance, DestinationToAgentCompleted OnDestinationCompleted)
    {
        if (stoppingDistance == 0) stoppingDistance = 0.5f;
        //Debug.Log("setting destination");
        navAgent.SetDestination(targetAgent.transform.position);
        targetLocation = targetAgent.transform.position;
        //Debug.Log("setting destination-" + targetLocation);
        navAgent.stoppingDistance = stoppingDistance;
        isMoving = true;
        agent.Animator.SetFloat("Vertical", 1);
        //Debug.Log("animating to destination");
        StopAllCoroutines();
        //StartCoroutine(PlayWalkingSound());
        StartCoroutine(CheckForDestinationCompleted(targetAgent, stoppingDistance, OnDestinationCompleted));
    }

    IEnumerator CheckForDestinationCompleted()
    {
        var destinationReached = false;
        var distanceCheckErrors = 0;

        while (!destinationReached)
        {
            //var remainingDistance = GetPathRemainingDistance(navAgent);
            var remainingDistance = Vector3.Distance(transform.position, targetLocation);
            //Debug.Log("["+gameObject.name+"] remaining distance-" + remainingDistance+", "+ navAgent.stoppingDistance);
            if (distanceCheckErrors >= 5)
            {
                Debug.LogError(gameObject.name + " in AgentMoveToTarget.cs -> CheckForDestinationCompleted(): Too many distanceErrorChecks. " +
                    "GetPathRemainingDistance is returning -1 too many times, which means there is an issue with the path being created");
                break;
            }

            if (remainingDistance < 0)
            {
                distanceCheckErrors++;
            }

            
            if (remainingDistance < navAgent.stoppingDistance)
            {
                destinationReached = true;
                StopMoving();
                //if(GetComponent<PlayerController>() != null)
                    //lookAroundAndStartAttack();
            }

            yield return null;// new WaitForSeconds(.1f);
        }


    }

    IEnumerator CheckForDestinationCompleted(Agent agent, float stoppingDistance, DestinationToAgentCompleted OnDestinationCompleted)
    {
        var destinationReached = false;
        var distanceCheckErrors = 0;

        while(!destinationReached)
        {
            
            var remainingDistance = Vector3.Distance(transform.position, targetLocation);// GetPathRemainingDistance(navAgent);

            if (distanceCheckErrors >= 5)
            {
                Debug.LogError(gameObject.name + " in AgentMoveToTarget.cs -> CheckForDestinationCompleted(): Too many distanceErrorChecks. " +
                    "GetPathRemainingDistance is returning -1 too many times, which means there is an issue with the path being created");
                break;
            }

            if (remainingDistance < 0)
            {
                distanceCheckErrors++;
            }

            if (remainingDistance <= stoppingDistance)
            {
                if (OnDestinationCompleted != null)
                    OnDestinationCompleted(agent);
                else
                    Debug.LogError(gameObject.name + " in AgentMoveToTarget.cs -> CheckForDestinationCompleted(): Callback is null");

                destinationReached = true;
                StopMoving();
            }

            yield return null;// new WaitForSeconds(.1f);
        }


    }

    // from https://stackoverflow.com/a/61449518
    public float GetPathRemainingDistance(NavMeshAgent navMeshAgent)
    {
        /*if (navMeshAgent.pathPending ||
            navMeshAgent.pathStatus == NavMeshPathStatus.PathInvalid ||
            navMeshAgent.path.corners.Length == 0)
            return -1f;
        */
        float distance = 0.0f;
        for (int i = 0; i < navMeshAgent.path.corners.Length - 1; ++i)
        {
            distance += Vector3.Distance(navMeshAgent.path.corners[i], navMeshAgent.path.corners[i + 1]);
        }

        return distance;
    }


    // this gets called repeatedly when stopped
    public void StopMoving()
    {
        isMoving = false;
        navAgent.destination = transform.position;
        agent.Animator.SetFloat("Vertical", 0);
        Debug.Log("[" + gameObject.name + "] stopping");
    }

    public void lookAroundAndStartAttack()
    {
        if (GetComponent<PlayerController>() != null)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            for(int i = 0; i < enemies.Length; i++)
            {
                if(Vector3.Distance( transform.position, enemies[i].transform.position)<5.0f)
                {
                    GetComponent<PlayerController>().attackEnemy(enemies[i].GetComponent<EnemyAIBrain>());
                    break;
                }
            }
        }
    }

}
