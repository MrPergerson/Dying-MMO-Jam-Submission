using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Agent))]
public class AgentMoveToTarget : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private bool playsAudio = true;
    [SerializeField] public AgentAudioData audioData;
    [SerializeField] private float footstepSpeed = .2f;
    private AudioSource audioSource;

    [Header("Debug")]
    public bool isMoving;
    NavMeshAgent navAgent;

    Agent agent;

    public delegate void DestinationToAgentCompleted(Agent agent);

    private void Awake()
    {
        agent = GetComponent<Agent>();
        navAgent = GetComponent<NavMeshAgent>();

        var audioContainer = new GameObject("AgentMove AudioSource");
        audioContainer.transform.parent = this.transform;
        audioContainer.transform.position = Vector3.zero + Vector3.up;
        audioSource = audioContainer.AddComponent<AudioSource>();
    }

    private void Start()
    {
        if (playsAudio)
        {
            if (audioData == null)
                Debug.LogError(gameObject.name + " -> " + this.ToString() + ": This component is set to play audio but there is no audioData found");
        }
    }

    public void SetDestination(Vector3 destination)
    {
        navAgent.destination = destination;
        navAgent.stoppingDistance = 0;
        isMoving = true;
        agent.Animator.SetFloat("Vertical", 1);
        StopAllCoroutines();
        StartCoroutine(PlayWalkingSound());
        StartCoroutine(CheckForDestinationCompleted());
    }

    public void SetDestination(Vector3 destination, float stoppingDistance)
    {
        navAgent.destination = destination;
        navAgent.stoppingDistance = stoppingDistance;
        isMoving = true;
        agent.Animator.SetFloat("Vertical", 1);
        StopAllCoroutines();
        StartCoroutine(PlayWalkingSound());
        StartCoroutine(CheckForDestinationCompleted());
    }

    public void SetDestination(Agent agent, float stoppingDistance, DestinationToAgentCompleted OnDestinationCompleted)
    {
        navAgent.destination = agent.transform.position;
        navAgent.stoppingDistance = stoppingDistance;
        isMoving = true;
        agent.Animator.SetFloat("Vertical", 1);
        StopAllCoroutines();
        StartCoroutine(PlayWalkingSound());
        StartCoroutine(CheckForDestinationCompleted(agent, OnDestinationCompleted));
    }

    IEnumerator CheckForDestinationCompleted()
    {
        var destinationReached = false;
        var distanceCheckErrors = 0;

        while (!destinationReached)
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


            if (remainingDistance <= navAgent.stoppingDistance && remainingDistance >= 0)
            {
                destinationReached = true;
                StopMoving();
            }

            yield return new WaitForSeconds(.1f);
        }


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


            if (remainingDistance <= navAgent.stoppingDistance && remainingDistance >= 0)
            {
                if (OnDestinationCompleted != null)
                    OnDestinationCompleted(agent);
                else
                    Debug.LogError(gameObject.name + " in AgentMoveToTarget.cs -> CheckForDestinationCompleted(): Callback is null");

                destinationReached = true;
                StopMoving();
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

    IEnumerator PlayWalkingSound()
    {
        if(playsAudio)
        {
            if(audioData == null)
            {
                Debug.LogWarning("There are no audioData file.");
            }
            else if (audioData.FootStepAudio.defaultFootsteps == null || audioData.FootStepAudio.defaultFootsteps.Count == 0)
            {
                Debug.LogWarning("There are no footstep sounds to play in defaultFootSteps.");

            }
            else
            {
                while (isMoving)
                {
                    var footstepSounds = new List<AudioClip>(audioData.FootStepAudio.defaultFootsteps);
                    int randomIndex = Random.Range(0, footstepSounds.Count);
                    audioSource.clip = footstepSounds[randomIndex];
                    audioSource.Play();

                    yield return new WaitForSeconds(footstepSpeed);
                }
            }
        }


        // do coroutines stop automatically if they reac 
    }

    public void StopMoving()
    {
        isMoving = false;
        agent.Animator.SetFloat("Vertical", 0);
    }

}
