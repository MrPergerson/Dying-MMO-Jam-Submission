using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StoryCondition : MonoBehaviour
{

    protected bool conditionMet = false;

    public abstract bool ConditionMet();


}
