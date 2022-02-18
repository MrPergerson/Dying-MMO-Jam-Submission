using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ally : Agent
{

    public bool followPlayer = false;
    PlayerController playerController;

    public override void TakeDamage(Agent origin, float damage)
    {
        //audioPlayer.playDamageSound();
        Health -= damage;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerController=GameObject.FindObjectOfType<PlayerController>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (followPlayer)
        {
            if (playerController != null)
            {
                if (playerController.IsInCombat)
                {
                    Agent target = playerController.gameObject.GetComponent<AgentAttack>().Target;
                    //or check other targets nearby
                    if (target != null)
                    {
                        AgentMoveToTarget.DestinationToAgentCompleted onDestinationToAgentCompleted = GetComponent<AgentAttack>().EnterCombat;
                        move.SetDestination(target, GetComponent<AgentAttack>().AttackDistance, onDestinationToAgentCompleted);
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
