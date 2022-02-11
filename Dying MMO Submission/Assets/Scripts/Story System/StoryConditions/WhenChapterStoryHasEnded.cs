using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class WhenChapterStoryHasEnded : StoryCondition
{
    [SerializeField, ReadOnly] protected bool conditionMet = false;

    [SerializeField] string globalVarName;
    //[SerializeField] TextAsset globalVarJSONFile;
    private string chapterEndedVarName;

    public override bool IsConditionMet()
    {
        // does this code continue after the parent chapter object is deactive?
 
        conditionMet = ((Ink.Runtime.BoolValue)DSInkGetVar.GetInstance().GetVariable(globalVarName)).value;

        return conditionMet == true;
    }

    public override void ResetCondition()
    {
        conditionMet = false;
    }


}
