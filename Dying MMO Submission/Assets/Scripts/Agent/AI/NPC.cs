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
    public DeathState deathState = new DeathState();

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
        currentState.ExitState(this);
        currentState = state;
        state.EnterState(this);
    }

    // only supports one threat atm
    public override void AddThreat(Agent threat)
    {
        if (this.threat == null && isAlive)
        {
            this.threat = threat;
            SwitchState(attackState);
        }
    }

    public override void RemoveThreat()
    {
        threat = null;
    }

    public override void TakeDamage(Agent threat, float damage)
    {
        AddThreat(threat);
        //audioPlayer.playDamagedSound();
        Health -= damage;
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

    public override void Die()
    {
        base.Die();
        SwitchState(deathState);
        StartCoroutine(DeactivateNPC());

    }

    //this is a hack. Without this, all agents will no longer attack after one enemy dies.
    // has to do with the ProcessCombatAbilities() in AgentAttack not stopping in time
    IEnumerator DeactivateNPC()
    {
        yield return new WaitForSeconds(5f);
        this.gameObject.SetActive(false);
    }

    public override void Respawn()
    {
        base.Respawn();
        SwitchState(idleState);
        this.gameObject.SetActive(true);
    }

}
