using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBarManager : Manager
{
    [Header("Combat Abilities")]
    [SerializeField] CombatAbilitySet combatAbilitySet;

    private static ActionBarManager instance;
    public HealthBar healthBar;
    private PlayerController playerController;

    [SerializeField] private List<HandleAbilityUI> abilitiesUI = new List<HandleAbilityUI>();

    private void Awake()
    {
        // Check that there is only one Dialogue Manager in scene
        if (instance != null)
        {
            Debug.LogError(this.gameObject + " Awake: More than one Action Bar Manager detected");

        }
        instance = this;
    }
    public static ActionBarManager GetInstance() { return instance; }

    public override void AwakeManager()
    {
        StartCoroutine(PassAbilityCooldownNumbers());
    }

    public override bool IsReadyToChangeScene()
    {
        return true;
    }

    public override void OnNewLevelLoaded()
    {
        if (!healthBar) { Debug.LogError(this + ": healthBar component is missing!"); }
        playerController = FindObjectOfType<PlayerController>();
        playerController.SetHealthBarComponent(healthBar);
        SetCombatAbility(playerController.GetComponent<AgentAttack>().GetCombatAbilitySet());
    }

    public override void OnSceneChangeRequested()
    {
        //
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

    IEnumerator PassAbilityCooldownNumbers()
    {
        foreach (CombatAbility ability in combatAbilitySet.abilities)
        {
            string abilityName = ability.name;
            float cooldownNum = ability.TimeUntilCoolDownEnds;
            switch (abilityName)
            {
                case "BasicAttack_d20":
                    abilitiesUI[0].SetCooldownNumber(cooldownNum);
                    break;
                case "PlayerBlueAttack":
                    abilitiesUI[1].SetCooldownNumber(cooldownNum);
                    break;
                case "PlayerGreenAttack":
                    abilitiesUI[2].SetCooldownNumber(cooldownNum);
                    break;
                case "PlayerRedAttack":
                    abilitiesUI[3].SetCooldownNumber(cooldownNum);
                    break;
                case null:
                    Debug.LogError(this + ": could not find ability name.");
                    break;
            }
        }

        yield return new WaitForSeconds(0.1f);
    }
}
