using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class StoryCondition : MonoBehaviour
{
    [Title("Debug")]
    [SerializeField,ReadOnly] protected bool isCurrentCondition = false;
    [SerializeField, ReadOnly] protected bool conditionMet = false;

    [Title("Events")]
    public UnityEvent onSelectedAsCurrentCondition;
    public UnityEvent onConditionMet;
    

    public abstract bool IsConditionMet();

    public abstract void ResetCondition();

    public virtual bool IsCurrentCondition()
    {
        if (isCurrentCondition) onSelectedAsCurrentCondition.Invoke();
        return isCurrentCondition;
    }


    public virtual void SetAsCurrentCondition(bool value)
    {
        isCurrentCondition = value;
    }
}