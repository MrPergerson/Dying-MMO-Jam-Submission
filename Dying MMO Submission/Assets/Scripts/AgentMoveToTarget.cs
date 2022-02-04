using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AgentMoveToTarget : MonoBehaviour
{
    NavMeshAgent navAgent;
    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }

    public void SetDestination(Vector3 postion)
    {
        navAgent.destination = postion;
    }
}
