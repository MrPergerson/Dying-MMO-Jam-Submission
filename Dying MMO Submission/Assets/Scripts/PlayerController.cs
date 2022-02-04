using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput), typeof(AgentMoveToTarget))]
public class PlayerController : MonoBehaviour
{
    private PlayerControls controls;
    private PlayerInput playerInput;
    private AgentMoveToTarget move;
    private AgentAttack attack;
    [SerializeField] LayerMask layerMask;

    void Awake()
    {    
        playerInput = GetComponent<PlayerInput>();
        move = GetComponent<AgentMoveToTarget>();
        attack = GetComponent<AgentAttack>();
        controls = new PlayerControls();
    }

    private void Start()
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
                AgentMoveToTarget.DestinationCompleted onDestinationCompleted = attack.Attack;
                var destination = hit.collider.gameObject.transform.position;
                move.SetDestination(destination, 2f, onDestinationCompleted);
            }

        }
    }
}
