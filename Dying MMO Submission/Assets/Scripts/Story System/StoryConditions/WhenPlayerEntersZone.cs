using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class WhenPlayerEntersZone : StoryCondition
{
    [Title("Trigger Zone", "Attach a trigger zone in the scene here and" +
        " this condition will automatically listen to it.")]
    [SerializeField] TriggerZone zone;


    private void OnEnable()
    {
        zone.onTriggerZoneEnter += CheckIfPlayerEnteredZone;
    }

    private void OnDisable()
    {
        zone.onTriggerZoneEnter -= CheckIfPlayerEnteredZone;
    }

    private void CheckIfPlayerEnteredZone(GameObject gameObject)
    {
        if (gameObject.tag == "Player" && IsCurrentCondition())
        {
            conditionMet = true;   
        }
            
    }

    public override bool IsConditionMet()
    {
        if (conditionMet) onConditionMet.Invoke();
        return conditionMet;
    }

    public override void ResetCondition()
    {
        conditionMet = false;
    }

}
