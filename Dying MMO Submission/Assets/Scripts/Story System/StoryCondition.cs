using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StoryCondition : MonoBehaviour
{
    [SerializeField,ReadOnly] protected bool isCurrentCondition = false;

    public abstract bool IsConditionMet();

    public abstract void ResetCondition();

    public virtual bool IsCurrentCondition()
    {
        return isCurrentCondition;
    }


    public virtual void SetAsCurrentCondition(bool value)
    {
        isCurrentCondition = value;
    }
}
