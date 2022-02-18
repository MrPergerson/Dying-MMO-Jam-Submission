using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ally : NPC
{

    public bool followPlayer = false;
    PlayerController playerController;

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
                        AgentMoveToTarget.DestinationToAgentCompleted onDestinationToAgentCompleted = GetComponent<AgentAttack>().EnterCombat;
                        move.SetDestination(target, 2, onDestinationToAgentCompleted);
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
