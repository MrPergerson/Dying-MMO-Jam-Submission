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
        if(playerInput.actions == null) playerInput.actions = controls.asset;
        controls.Main.CursorPrimaryClick.performed += HandlePrimaryCursorInput;
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

    protected override void Die()
    {
        print("player has died");
    }

    public override void TakeDamage(Agent threat, float damage)
    {
        Health -= damage;
    }
}
