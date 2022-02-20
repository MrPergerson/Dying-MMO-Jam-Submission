using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TriggerZone))]
public class ObjectiveWhenPlayerEntersTrigger : Objective
{
    TriggerZone trigger;


    [SerializeField] private UnityEvent onTriggerEnter;
    [SerializeField] private UnityEvent onTriggerExit;

    private void Awake()
    {
        trigger = GetComponent<TriggerZone>();
        trigger.onTriggerZoneEnter += PlayerEntered;
        trigger.onTriggerZoneEnter += SetObjectiveToTrue;
        trigger.onTriggerZoneExit += PlayerExited;
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

    public void PlayerEntered(GameObject obj)
    {
        if (IsCurrentObjective()) onTriggerEnter.Invoke();
    }

    public void PlayerExited(GameObject obj)
    {
        onTriggerExit.Invoke();
    }
}
