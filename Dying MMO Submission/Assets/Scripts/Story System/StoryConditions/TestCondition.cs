using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class TestCondition : StoryCondition
{
    [Button("Set Condition to Met")]
    private void SetConditionToTrue()
    {
        if(IsCurrentCondition())
            conditionMet = true;
        else
        {
            Debug.LogWarning(this + ": This condition was met but it is not the current condition so this check was ignored.");
        }
    }


    public override bool IsConditionMet()
    {
        return conditionMet;
    }

    public override void InitializeCondition()
    {
        conditionMet = false;
        SetAsCurrentCondition(false);
    }

    public override bool IsCurrentCondition()
    {
        return isCurrentCondition;
    }

    public override void SetAsCurrentCondition(bool value)
    {
        isCurrentCondition = value;
    }
}
