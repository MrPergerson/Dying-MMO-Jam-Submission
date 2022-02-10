using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class TestCondition : StoryCondition
{
    [Button("Set Condition to Met")]
    private void SetConditionToTrue()
    {
        conditionMet = true;
    }


    public override bool ConditionMet()
    {
        return conditionMet;
    }

}
