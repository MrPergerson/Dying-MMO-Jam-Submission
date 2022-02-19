using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveTriggerThis : Objective
{
    public void TriggerThis()
    {
        if (IsCurrentObjective())
            objectiveMet = true;
    }
}
