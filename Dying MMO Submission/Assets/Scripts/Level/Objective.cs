using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour
{
    [SerializeField, ReadOnly] protected bool objectiveMet = false;
    [SerializeField, ReadOnly] private bool isTheCurrentObjective = false;

    public void SetAsCurrentObjective()
    {
        isTheCurrentObjective = true;
    }

    public virtual bool IsCurrentObjective()
    {
        return isTheCurrentObjective;
    }

    public virtual bool ObjectiveHasBeenMet()
    {
        return objectiveMet;
    }
}
