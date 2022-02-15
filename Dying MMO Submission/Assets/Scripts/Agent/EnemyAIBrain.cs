using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AgentAttack))]
public class EnemyAIBrain : Agent
{
    public Agent threat;
    private AgentAttack attack;
    [SerializeField] LayerMask layerMask;



    protected override void Awake()
    {
        base.Awake();
        attack = GetComponent<AgentAttack>();
    }

    protected override void Start()
    {
        base.Start();
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


    public override void TakeDamage(Agent threat, float damage)
    {
        AddThreat(threat);
        Health -= damage;
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
