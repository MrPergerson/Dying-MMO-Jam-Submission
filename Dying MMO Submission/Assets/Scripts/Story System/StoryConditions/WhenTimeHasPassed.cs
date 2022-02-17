using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhenTimeHasPassed : StoryCondition
{
    [SerializeField] private float timeInSecondsToWait = 5;
    [SerializeField, ReadOnly] private float timeLeft;

    private void Update()
    {
        if(IsCurrentCondition())
        {
            timeLeft -= Time.deltaTime;
        }
    }

    public override bool IsConditionMet()
    {
        if (timeLeft <= 0)
            conditionMet = true;

        return conditionMet;
    }

    public override void InitializeCondition()
    {
        timeLeft = timeInSecondsToWait;
        conditionMet = false;
    }
}
