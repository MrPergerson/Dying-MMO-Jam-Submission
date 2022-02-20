using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AgentAttack))]
public class EnemyAIBrain : NPC
{

    [SerializeField] LayerMask layerMask;

    public static PlayerController playerController;
    float cooldownTimeRemaining = 0.0f;
    bool waitingToFollow = false;

    public float followRadius;
    public Vector3 guardPosition;
    private AgentAudioPlayer audioPlayer;

    protected override void Awake()
    {
        base.Awake();

        if(playerController==null)
            playerController=FindObjectOfType<PlayerController>();
        guardPosition = transform.position;
        audioPlayer = GetComponent<AgentAudioPlayer>();
    }

    protected override void Start()
    {
        base.Start();
        layerMask = LayerMask.GetMask("Ground, NPC, Player");

    }

    IEnumerator waitAndFollow()
    {
        waitingToFollow = true;
        yield return new WaitForSeconds(1.0f);
        if (Vector3.Distance(guardPosition, playerController.gameObject.transform.position) < followRadius &&
            Vector3.Distance(guardPosition, playerController.gameObject.transform.position) > attackRange)
        {
            AgentMoveToTarget.DestinationToAgentCompleted onDestinationToAgentCompleted = GetComponent<AgentAttack>().EnterCombat;
            move.SetDestination(playerController, attackRange - 0.5f, onDestinationToAgentCompleted);
    }
        waitingToFollow = false;
        yield return null;
    }

    protected override void Update()
    {

        base.Update();
        if (isAlive)
        {



            //check if player is around
            if (playerController != null && GetComponent<AgentAttack>() != null && cooldownTimeRemaining <= 0.0f)
            {

                if (Vector3.Distance(playerController.gameObject.transform.position, transform.position) < attackRange)
                {
                    //move.SetDestination(playerController.gameObject.transform.position);
                    AddThreat(playerController);
                    cooldownTimeRemaining = 2.0f;
                }
                //            if (!waitingToFollow && Vector3.Distance(guardPosition, playerController.gameObject.transform.position) < followRadius)
                //            else if(!waitingToFollow && !IsInCombat && Vector3.Distance(transform.position, guardPosition)>1.0f)
                else if (!waitingToFollow && Vector3.Distance(guardPosition, playerController.gameObject.transform.position) < followRadius)
                {
                    //move.SetDestination(playerController.gameObject.transform.position, 0);
                    StartCoroutine(waitAndFollow());
                }
                else if (!waitingToFollow && !GetComponent<AgentAttack>().IsInCombat && Vector3.Distance(transform.position, guardPosition) > 1.0f)
                {
                    move.SetDestination(guardPosition, 0);
                }
            }
            if (cooldownTimeRemaining > 0.0f)
                cooldownTimeRemaining -= Time.deltaTime;
        }
    }

    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, followRadius);

    }
    
    public override void PlayCombatAnimation(int index)
    {
        switch (index)
        {
            case 0:
                Animator.SetTrigger("Ability1");   
                break;
            default:
                Animator.SetTrigger("Ability1");
                break;
        }
    }

    public override void Respawn()
    {
        base.Respawn();
        Animator.SetBool("IsDead", false);
    }

    public override void Die()
    {
        base.Die();
        Animator.SetBool("IsDead", true);
    }
}
