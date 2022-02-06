using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

[RequireComponent(typeof(PlayerInput), typeof(AgentAttack))]
public class PlayerController : Agent
{
    private PlayerControls controls;
    private PlayerInput playerInput;
    
    private AgentAttack attack;
    [SerializeField] LayerMask layerMask;


    protected override void Awake()
    {
        base.Awake();
        playerInput = GetComponent<PlayerInput>();
        attack = GetComponent<AgentAttack>();
        controls = new PlayerControls();
    }

    protected override void Start()
    {
        base.Start();
        attack.audioData = audioData;

        if (playerInput.actions == null) playerInput.actions = controls.asset;
        controls.Main.CursorPrimaryClick.performed += HandlePrimaryCursorInput;
        controls.Main.CombatAbility1.performed += PerformCombatAbility;
        controls.Main.CombatAbility2.performed += PerformCombatAbility;
        controls.Main.CombatAbility3.performed += PerformCombatAbility;
        controls.Main.CombatAbility4.performed += PerformCombatAbility;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    public void HandlePrimaryCursorInput(InputAction.CallbackContext context)
    {
        // move, attack, or interact?
        var mousePosition = controls.Main.CursorPosition.ReadValue<Vector2>();
        var ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, layerMask))
        {

            if(hit.collider.tag == "Ground")
            {
                move.SetDestination(hit.point, 0);
                
            } 
            else if(hit.collider.tag == "Enemy")
            {

                if(hit.collider.gameObject.TryGetComponent<EnemyAIBrain>(out EnemyAIBrain enemy))
                {
                    AgentMoveToTarget.DestinationToAgentCompleted onDestinationToAgentCompleted = attack.StartAttack;
                    move.SetDestination(enemy, attack.AttackDistance, onDestinationToAgentCompleted);

                }
                else
                {
                    Debug.LogError(gameObject.name + " -> PlayerController.cs -> HandlePrimaryCursorInput: Failed to get EnemyAIBrain");
                }

            }

        }
    }

    public void PerformCombatAbility(InputAction.CallbackContext context)
    {
        if(context.action.Equals(controls.Main.CombatAbility1))
        {
            attack.PerformCombat(0);
        }
        else if(context.action.Equals(controls.Main.CombatAbility2))
        {
            attack.PerformCombat(1);
        }
        else if (context.action.Equals(controls.Main.CombatAbility3))
        {
            attack.PerformCombat(2);
        }
        else if (context.action.Equals(controls.Main.CombatAbility4))
        {
            attack.PerformCombat(3);
        }
        else
        {
            Debug.LogError(gameObject.name + " -> " + this.ToString() + " -> PerformCombatAbility(): Function failed to match combat ability.");
        }

    }

    protected override void Die()
    {
        print("player has died");
    }

    public override void TakeDamage(Agent threat, float damage)
    {
        Health -= damage;
    }
}
