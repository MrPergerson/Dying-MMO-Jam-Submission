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

    public AgentAttack attackAbility;
    [SerializeField] LayerMask layerMask;

    // Added handleMouseOnUI
    private HandleMouseOnUI handleMouseOnUI;
    protected override void Awake()
    {
        base.Awake();
        playerInput = GetComponent<PlayerInput>();
        attackAbility = GetComponent<AgentAttack>();
        controls = new PlayerControls();
    }

    protected override void Start()
    {
        base.Start();

        if (playerInput.actions == null) playerInput.actions = controls.asset;
        controls.Main.CursorPrimaryClick.performed += HandlePrimaryCursorInput;
        controls.Main.CombatAbility1.performed += PerformCombatAbility;
        controls.Main.CombatAbility2.performed += PerformCombatAbility;
        controls.Main.CombatAbility3.performed += PerformCombatAbility;
        controls.Main.CombatAbility4.performed += PerformCombatAbility;
        controls.Main.EndGame.performed += QuitApp;

        handleMouseOnUI = GetComponent<HandleMouseOnUI>();// Added

        StartCoroutine(startAutoHeal());
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

        // Added && !handleMouseOnUI.IsMouseOnUI() to check if mouse is NOT on UI
        if (Physics.Raycast(ray, out hit, layerMask) && !handleMouseOnUI.IsMouseOnUI())
        {
            var tag = hit.collider.tag;
            if (tag == "Ground" || tag == "Ground_Grass" || tag == "Ground_Stone")
            {
                move.SetDestination(hit.point, 0);
                attackAbility.EndCombat();
                RemoveThreat();

            }
            else if (hit.collider.tag == "Enemy")
            {
                if (hit.collider.gameObject.TryGetComponent<EnemyAIBrain>(out EnemyAIBrain enemy))
                {
                    AddThreat(enemy);

                    AgentMoveToTarget.DestinationToAgentCompleted onDestinationToAgentCompleted = attackAbility.EnterCombat;
                    // function requires attack distance, but this is a hack. Cole, remember to look back at this
                    move.SetDestination(threat, 1, onDestinationToAgentCompleted);

                    threat.onDeath += attackAbility.EndCombat;
                    threat.onDeath += RemoveThreat;

                }
                else
                {
                    Debug.LogError(gameObject.name + " -> PlayerController.cs -> HandlePrimaryCursorInput: Failed to get EnemyAIBrain");
                }
            }
            else if(hit.collider.tag == "Selectable")
            {
                var obj = hit.collider.gameObject.GetComponent<ISelectable>();
                obj.Select();

                attackAbility.EndCombat();
                RemoveThreat();
            }

        }
    }

    public void PerformCombatAbility(InputAction.CallbackContext context)
    {
        if (context.action.Equals(controls.Main.CombatAbility1))
        {
            attackAbility.PerformCombat(0);
        }
        else if (context.action.Equals(controls.Main.CombatAbility2))
        {
            attackAbility.PerformCombat(1);
        }
        else if (context.action.Equals(controls.Main.CombatAbility3))
        {
            attackAbility.PerformCombat(2);
        }
        else if (context.action.Equals(controls.Main.CombatAbility4))
        {
            attackAbility.PerformCombat(3);
        }
        else
        {
            Debug.LogError(gameObject.name + " -> " + this.ToString() + " -> PerformCombatAbility(): Function failed to match combat ability.");
        }

    }

    public override void Die()
    {
        base.Die();
        isAlive = false;
        SceneManager.Instance.RestartLevel();
        // scene will need to restart
    }

    public override void TakeDamage(Agent threat, float damage)
    {
        Health -= damage;
    }

    private void QuitApp(InputAction.CallbackContext context)
    {
        Application.Quit();
    }

    public void attackEnemy(Agent target)
    {
        attackAbility.EnterCombat(target);
    }

    IEnumerator startAutoHeal()
    {
        while(isAlive)
        {
            if (Health < _maxHealth && !attackAbility.IsInCombat)
            {
                Health += _healingRate;
                if (Health > _maxHealth)
                    Health = _maxHealth;
            }
            yield return new WaitForSeconds(1.0f);
        }

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
