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

    public override void TakeDamage(Agent threat, float damage)
    {
        AddThreat(threat);
        audioPlayer.playDamagedSound();
        Health -= damage;
    }

    IEnumerator waitAndFollow()
    {
        /*
        waitingToFollow = true;
        yield return new WaitForSeconds(1.0f);
        if (Vector3.Distance(guardPosition, playerController.gameObject.transform.position) < followRadius &&
            Vector3.Distance(guardPosition, playerController.gameObject.transform.position) > GetComponent<AgentAttack>().AttackDistance)
                move.SetDestination(playerController.gameObject.transform.position, 0);
        waitingToFollow = false;
        */
        yield return null;
    }

    protected override void Update()
    {

        base.Update();
        /*
        //check if player is around
        if (playerController != null && GetComponent<AgentAttack>()!= null && cooldownTimeRemaining <= 0.0f)
        {
            
            if(Vector3.Distance(playerController.gameObject.transform.position, transform.position) < GetComponent<AgentAttack>().AttackDistance)
            {
                AddThreat(playerController);
                cooldownTimeRemaining = 2.0f;
            }
            else if (!waitingToFollow && Vector3.Distance(guardPosition, playerController.gameObject.transform.position) < followRadius)
            {
                //move.SetDestination(playerController.gameObject.transform.position, 0);
                StartCoroutine(waitAndFollow());
            }
            else
            {
                move.SetDestination(guardPosition, 0);
            }
        }
        if(cooldownTimeRemaining>0.0f)
            cooldownTimeRemaining -= Time.deltaTime;
            */
    }

    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, followRadius);

    }
    
    public override void Die()
    {
        base.Die();
        this.gameObject.SetActive(false);
    }

    public override void Respawn()
    {
        base.Respawn();
        this.gameObject.SetActive(true);
    }

    public override void PlayCombatAnimation(int index)
    {
        switch (index)
        {
            case 0:
                Animator.SetTrigger("Ability1");
                break;
            case 1:
                Animator.SetTrigger("Ability2");
                break;
            case 2:
                Animator.SetTrigger("Ability3");
                break;
            case 3:
                Animator.SetTrigger("Ability4");
                break;
            default:
                Animator.SetTrigger("Ability1");
                break;
        }
    }
}
