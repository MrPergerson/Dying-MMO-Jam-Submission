using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class WhenChapterStoryHasEnded : StoryCondition
{
    [SerializeField] string globalVarName;
    //[SerializeField] TextAsset globalVarJSONFile;
    private string chapterEndedVarName;

    public override void SetAsCurrentCondition(bool value)
    {
        base.SetAsCurrentCondition(value);
        if(value == true)
        {
            DialogueManagerAS2.GetInstance().EnterDialogueMode();
        }
    }

    public override bool IsConditionMet()
    {
        
        if (DialogueManagerAS2.GetInstance() != null)
        {
            // does this code continue after the parent chapter object is deactive?
            conditionMet = ((Ink.Runtime.BoolValue)DialogueManagerAS2.GetInstance().GetVariable(globalVarName)).value;
        }
        else
        {
            conditionMet = ((Ink.Runtime.BoolValue)DialogueManagerAS2.GetInstance().GetVariable(globalVarName)).value;
        }

        if (conditionMet) onConditionMet.Invoke(); print("called");

        return conditionMet == true;
    }

    public override void InitializeCondition()
    {
        conditionMet = false;
    }

    public override void DeinitializeCondition()
    {
        // nothing
    }
}
