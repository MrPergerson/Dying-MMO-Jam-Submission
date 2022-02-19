using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TriggerZone))]
public class ObjectiveWhenPlayerEntersTrigger : Objective
{
    TriggerZone trigger;

    private void Awake()
    {
        trigger = GetComponent<TriggerZone>();
        trigger.onTriggerZoneEnter += SetObjectiveToTrue;
    }

    private void SetObjectiveToTrue(GameObject obj)
    {
        if (IsCurrentObjective() && obj.tag == "Player")
            objectiveMet = true;
    }

    public override bool ObjectiveHasBeenMet()
    {

        return objectiveMet;
        
    }
}
