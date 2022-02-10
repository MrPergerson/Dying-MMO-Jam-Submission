using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StoryCondition : MonoBehaviour
{
    public abstract bool ConditionMet();

    public abstract void ResetCondition();

    public abstract bool IsCurrentCondition();

    public abstract void SetAsCurrentCondition(bool value);
}
