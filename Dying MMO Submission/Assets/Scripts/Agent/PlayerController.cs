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
    //Added HealthBar
    private HealthBar healthBar;


    public enum PlayerState { Idle, ControlledMove, NavMove, Attacking }
    public PlayerState currentPlayerState;

    protected override void Awake()
    {
        base.Awake();
        playerInput = GetComponent<PlayerInput>();
        attackAbility = GetComponent<AgentAttack>();
        controls = new PlayerControls();
        handleMouseOnUI = GetComponent<HandleMouseOnUI>();
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
        //StartCoroutine(lookForEnemies());


        StartCoroutine(startAutoHeal());
        //StartCoroutine(lookForEnemies());
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }


    protected override void Update()
    {
        base.Update();

        switch (currentPlayerState)
        {
            case PlayerState.Idle:
                break;
            case PlayerState.NavMove:
                break;
            case PlayerState.Attacking:
                break;
        }
    }


    void FixedUpdate()
    {
        switch (currentPlayerState)
        {
            case PlayerState.ControlledMove:
                HandleMoveInput();
                break;
        }
        
    }

    public void HandleMoveInput()
    {
        var direction = controls.Main.Move.ReadValue<Vector2>();
        if(direction.x != 0 || direction.y != 0)
        {
            move.WalkToDirection(direction);
        }
    }

    public void HandlePrimaryCursorInput(InputAction.CallbackContext context)
    {
        var mousePosition = controls.Main.CursorPosition.ReadValue<Vector2>();
        var ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, layerMask) && !handleMouseOnUI.IsMouseOnUI())
        {
            var tag = hit.collider.tag;
            if (tag == "Ground" || tag == "Ground_Grass" || tag == "Ground_Stone")
            {
                SwitchToNavMove(hit.point);
            }
            else if (hit.collider.tag == "Enemy")
            {
                if (hit.collider.gameObject.TryGetComponent<EnemyAIBrain>(out EnemyAIBrain enemy))
                {
                    var heading = enemy.transform.position - this.transform.position;
                    if(heading.sqrMagnitude >= 1) // needs an attack range
                    {
                        AgentMoveToTarget.DestinationToThreatCompleted onDestinationToAgentCompleted = SwitchToAttacking;
                        SwitchToNavMove(threat.transform.position, onDestinationToAgentCompleted);                     
                    }
                    else
                    {
                        SwitchToAttacking(enemy);
                    }
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

                SwitchToIdle();
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
        if(healthBar != null)
        {
            healthBar.SetHealth(Health);
        }
        else
        {
            Debug.LogWarning(this + ": HealthBar component is empty");
        }
    }

    // Added SetHealthBarComponent
    public void SetHealthBarComponent(HealthBar hb)
    {
        healthBar = hb;
        healthBar.SetMaxHealth(Health);
        //print(healthBar + " " + Health);
    }

    /*private void QuitApp(InputAction.CallbackContext context)
    {
        Application.Quit();
    }*/

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
            
            // Added
            if (healthBar != null)
            {
                healthBar.SetHealth(Health);
            }
            else
            {
                Debug.LogWarning(this + ": HealthBar component is empty");
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

    IEnumerator lookForEnemies()
    {
        EnemyAIBrain[] enemies = GameObject.FindObjectsOfType<EnemyAIBrain>();
        while (true)
        {
            if (!GetComponent<AgentAttack>().IsInCombat)
            {
                for (int i = 0; i < enemies.Length; i++)
                {
                    if (enemies[i].isActiveAndEnabled && enemies[i].Health>=0 && Vector3.Distance(transform.position, enemies[i].transform.position) <= 2.0f)
                    {
                        Debug.Log("[player] attacking");
                        attackEnemy(enemies[i]);
                        break;
                    }
                }
            }

            yield return new WaitForSeconds(1);
        }
    }

    private void SwitchToIdle()
    {
        move.navAgent.isStopped = true;
        attackAbility.EndCombat();
        RemoveThreat();
    }

    private void SwitchToControlledMove()
    {
        move.navAgent.isStopped = true;
        attackAbility.EndCombat();
        RemoveThreat();

    }

    private void SwitchToNavMove(Vector3 destination, AgentMoveToTarget.DestinationToThreatCompleted callback = null)
    {
        move.navAgent.isStopped = false;
        attackAbility.EndCombat();
        RemoveThreat();

        if (callback != null)
            move.SetDestination(destination, 1, callback);
        else
            move.SetDestination(destination, 0);
    }

    private void SwitchToAttacking(Agent enemy)
    {
        move.navAgent.isStopped = true;
        AddThreat(enemy);

        threat.onDeath += attackAbility.EndCombat;
        threat.onDeath += RemoveThreat;
        attackAbility.EnterCombat(enemy);
    }
}
