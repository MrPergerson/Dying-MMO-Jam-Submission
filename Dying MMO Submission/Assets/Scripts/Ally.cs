using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AgentAttack))]
public class Ally : Agent
{

    public bool followPlayer = false;
    PlayerController playerController;
    AgentAttack attackAbility;

    protected override void Awake()
    {
        base.Awake();
        attackAbility = GetComponent<AgentAttack>();
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

    public override void TakeDamage(Agent origin, float damage)
    {
        //audioPlayer.playDamageSound();
        Health -= damage;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        playerController=GameObject.FindObjectOfType<PlayerController>(); 
    }

    // Update is called once per frame
    protected override void Update()
    {
        // The party system should assign the Ally's target
        if (followPlayer)
        {
            if (playerController != null)
            {
                if (playerController.attackAbility.IsInCombat)
                {
                    Agent target = playerController.gameObject.GetComponent<AgentAttack>().Target;
                    //or check other targets nearby
                    if (target != null)
                    {
                        
                        AgentMoveToTarget.DestinationToAgentCompleted onDestinationToAgentCompleted = attackAbility.EnterCombat;
                        move.SetDestination(target, 2, onDestinationToAgentCompleted);
                        target.onDeath += attackAbility.EndCombat;
                    }
                }
                else
                    move.SetDestination(playerController.gameObject.transform.position, 3.0f);

            }
        }
        else
        {
            move.SetDestination(transform.position);
        }
    }
}
