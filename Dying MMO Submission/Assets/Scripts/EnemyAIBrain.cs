using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AgentAttack))]
public class EnemyAIBrain : Agent
{
    public Agent threat;
    private AgentAttack attack;
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
        attack = GetComponent<AgentAttack>();
    }

    protected override void Start()
    {
        base.Start();
        if(playerController==null)
            playerController=FindObjectOfType<PlayerController>();
        layerMask = LayerMask.GetMask("Ground, NPC, Player");
        guardPosition = transform.position;
        audioPlayer = GetComponent<AgentAudioPlayer>();
    }

    // only supports one threat atm
    void AddThreat(Agent threat)
    {
        if(this.threat == null)
        {
            this.threat = threat;
            attack.StartAttack(threat);
            attack.onAttackEnded += RemoveThreat;

        }
    }

    void RemoveThreat()
    {
        threat = null;
    }


    public override void TakeDamage(Agent threat, float damage)
    {
        AddThreat(threat);
        audioPlayer.playDamageSound();
        Health -= damage;
    }

    IEnumerator waitAndFollow()
    {
        waitingToFollow = true;
        yield return new WaitForSeconds(1.0f);
        if (Vector3.Distance(guardPosition, playerController.gameObject.transform.position) < followRadius &&
            Vector3.Distance(guardPosition, playerController.gameObject.transform.position) > GetComponent<AgentAttack>().AttackDistance)
                move.SetDestination(playerController.gameObject.transform.position, 0);
        waitingToFollow = false;
    }

    void Update()
    {
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
}
