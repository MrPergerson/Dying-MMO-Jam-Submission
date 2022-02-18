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
        Level.GetInstance().SetCurrentObjective(objectiveID);

    }

    public override bool IsConditionMet()
    {
       return Level.GetInstance().CurrentObjectiveMet();
    }
}
