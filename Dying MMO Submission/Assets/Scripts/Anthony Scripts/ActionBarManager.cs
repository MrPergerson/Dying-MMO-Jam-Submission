using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActionBarManager : Manager
{
    [Header("Combat Abilities")]
    [SerializeField] CombatAbilitySet combatAbilitySet;

    private static ActionBarManager instance;
    public HealthBar healthBar;
    private PlayerController playerController;

    [SerializeField] private List<HandleAbilityUI> abilitiesUI = new List<HandleAbilityUI>();

    private PlayerControls controls;

    private void Awake()
    {
        // Check that there is only one Dialogue Manager in scene
        if (instance != null)
        {
            Debug.LogError(this.gameObject + " Awake: More than one Action Bar Manager detected");

        }
        instance = this;
        controls = new PlayerControls();
    }
    public static ActionBarManager GetInstance() { return instance; }

    public override void AwakeManager()
    {
        controls = new PlayerControls();
        controls.Main.CombatAbility1.started += PassAbilityCooldownNumbers;
        controls.Main.CombatAbility2.started += PassAbilityCooldownNumbers;
        controls.Main.CombatAbility3.started += PassAbilityCooldownNumbers;
        controls.Main.CombatAbility4.started += PassAbilityCooldownNumbers;
    }

    public override bool IsReadyToChangeScene()
    {
        return true;
    }

    public override void OnNewLevelLoaded()
    {
        controls = new PlayerControls();
        controls.Main.CombatAbility1.performed += PassAbilityCooldownNumbers;
        controls.Main.CombatAbility2.performed += PassAbilityCooldownNumbers;
        controls.Main.CombatAbility3.performed += PassAbilityCooldownNumbers;
        controls.Main.CombatAbility4.performed += PassAbilityCooldownNumbers;
        if (!healthBar) { Debug.LogError(this + ": healthBar component is missing!"); }
        playerController = FindObjectOfType<PlayerController>();
        playerController.SetHealthBarComponent(healthBar);
        SetCombatAbility(playerController.GetComponent<AgentAttack>().GetCombatAbilitySet());
        print(combatAbilitySet.abilities[0].Name);
        //StartCoroutine(PassAbilityCooldownNumbers());
    }

    public override void OnSceneChangeRequested()
    {
        //
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    /*public void DisplayAbilityCooldown(int abilityNum, float cooldownNum)
    {
        if (abilityNum <= 0 || abilityNum > 4)
        {
            Debug.LogError(this + ": Ability Number out of range.");
        }
        abilitiesUI[abilityNum-1].SetCooldownNumber(cooldownNum);
    }*/

    public void SetCombatAbility(CombatAbilitySet abilitySet)
    {
        combatAbilitySet = abilitySet;
    }

    private void PassAbilityCooldownNumbers(InputAction.CallbackContext context)
    {
        print("passing abilites");
        foreach (CombatAbility ability in combatAbilitySet.abilities)
        {
            string abilityName = ability.Name;
            float cooldownNum = ability.TimeUntilCoolDownEnds;
            switch (abilityName)
            {
                case "Basic Attack":
                    abilitiesUI[0].SetCooldownNumber(cooldownNum);
                    break;
                case "Red Attack":
                    abilitiesUI[1].SetCooldownNumber(cooldownNum);
                    break;
                case "Green Attack":
                    abilitiesUI[2].SetCooldownNumber(cooldownNum);
                    break;
                case "Blue Attack":
                    abilitiesUI[3].SetCooldownNumber(cooldownNum);
                    break;
                case null:
                    Debug.LogError(this + ": could not find ability name.");
                    break;
            }
        }

        //yield return new WaitForSeconds(0.1f);
    }
}
