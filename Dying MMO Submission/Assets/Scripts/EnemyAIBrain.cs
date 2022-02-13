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


    protected override void Die()
    {
        // maybe later add delay 
        // there could be a death animation or partical effect
        Destroy(this.gameObject);
    }

    public override void TakeDamage(Agent threat, float damage)
    {
        AddThreat(threat);
        Health -= damage;
    }

    private void Update()
    {
        //check if player is around
        if (playerController != null && GetComponent<AgentAttack>()!=null)
        {
            if(Vector3.Distance(playerController.gameObject.transform.position, transform.position) < GetComponent<AgentAttack>().AttackDistance)
            {
                AddThreat(playerController);
            }
        }
    }


}
