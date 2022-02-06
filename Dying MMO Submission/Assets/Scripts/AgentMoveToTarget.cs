using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AgentMoveToTarget : MonoBehaviour
{
    public AgentAudioData audioData;

    NavMeshAgent navAgent;

    public delegate void DestinationToAgentCompleted(Agent agent);

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }

    public void SetDestination(Vector3 destination)
    {
        navAgent.destination = destination;
        navAgent.stoppingDistance = 0;
    }

    public void SetDestination(Vector3 destination, float stoppingDistance)
    {
        navAgent.destination = destination;
        navAgent.stoppingDistance = stoppingDistance;
    }

    public void SetDestination(Agent agent, float stoppingDistance, DestinationToAgentCompleted OnDestinationCompleted)
    {
        navAgent.destination = agent.transform.position;
        navAgent.stoppingDistance = stoppingDistance;
        StopAllCoroutines();
        StartCoroutine(CheckForDestinationCompleted(agent, OnDestinationCompleted));
    }

    IEnumerator CheckForDestinationCompleted(Agent agent, DestinationToAgentCompleted OnDestinationCompleted)
    {
        var destinationReached = false;
        var distanceCheckErrors = 0;

        while(!destinationReached)
        {
            var remainingDistance = GetPathRemainingDistance(navAgent);

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


            if (remainingDistance <= navAgent.stoppingDistance && remainingDistance > 0)
            {
                if (OnDestinationCompleted != null)
                    OnDestinationCompleted(agent);
                else
                    Debug.LogError(gameObject.name + " in AgentMoveToTarget.cs -> CheckForDestinationCompleted(): Callback is null");

                destinationReached = true;
            }

            yield return new WaitForSeconds(.1f);
        }


    }

    // from https://stackoverflow.com/a/61449518
    public float GetPathRemainingDistance(NavMeshAgent navMeshAgent)
    {
        if (navMeshAgent.pathPending ||
            navMeshAgent.pathStatus == NavMeshPathStatus.PathInvalid ||
            navMeshAgent.path.corners.Length == 0)
            return -1f;

        float distance = 0.0f;
        for (int i = 0; i < navMeshAgent.path.corners.Length - 1; ++i)
        {
            distance += Vector3.Distance(navMeshAgent.path.corners[i], navMeshAgent.path.corners[i + 1]);
        }

        return distance;
    }


}
