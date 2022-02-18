using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Agent
{
    [SerializeField] public float attackRange = 2;
    [SerializeField] public float chaseTime = 2;



    public AgentAttack attackAbility;

    protected State currentState;
    public IdleState idleState = new IdleState();
    public AttackState attackState = new AttackState();

    protected override void Awake()
    {
        base.Awake();
        attackAbility = GetComponent<AgentAttack>();
    }

    protected override void Start()
    {
        currentState = idleState;
        currentState.EnterState(this);
    }
    
    protected override void Update()
    {
        base.Update();
        currentState.Update(this);
    }

    public void SwitchState(State state)
    {
        currentState = state;
        state.EnterState(this);
    }

    // only supports one threat atm
    public override void AddThreat(Agent threat)
    {
        if (this.threat == null)
        {
            this.threat = threat;
            SwitchState(attackState);


        }
    }

    public override void RemoveThreat()
    {
        threat = null;
        SwitchState(idleState);
    }

    public override void TakeDamage(Agent origin, float damage)
    {
        //
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
