using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhenObjectiveIsMet : StoryCondition
{
    [SerializeField] private string objectiveID;
    [SerializeField, ReadOnly] Objective objective;

    public override void DeinitializeCondition()
    {
        
    }

    public override void InitializeCondition()
    {
        if(Level.GetInstance() != null)
            Level.GetInstance().SetCurrentObjective(objectiveID);

    }

    public override bool IsConditionMet()
    {
       var conditionMet = Level.GetInstance().CurrentObjectiveMet();

        if (conditionMet) onConditionMet.Invoke();

        return conditionMet;
    }
}
