using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBarManager : Manager
{
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
        if(!healthBar) { Debug.LogError(this + ": healthBar component is missing!"); }
        playerController = FindObjectOfType<PlayerController>();
        playerController.SetHealthBarComponent(healthBar);
    }

    public override bool IsReadyToChangeScene()
    {
        return true;
    }

    public override void OnNewLevelLoaded()
    {
        //
    }

    public override void OnSceneChangeRequested()
    {
        //
    }

    public void DisplayAbilityCooldown(int abilityNum, float cooldonwNum)
    {
        if (abilityNum <= 0 || abilityNum > 4)
        {
            Debug.LogError(this + ": Ability Number out of range.");
        }
        abilitiesUI[abilityNum-1].SetCooldownNumber(cooldonwNum);
    }
}
