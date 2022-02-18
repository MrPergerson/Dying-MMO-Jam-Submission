using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class DialogueVariables
{
    public Dictionary<string, Ink.Runtime.Object> variables { get; private set; }

    public DialogueVariables(TextAsset loadGlobalJSON)
    {
        Story globalVariableInkFile = new Story(loadGlobalJSON.text);

        variables = new Dictionary<string, Ink.Runtime.Object>();
        foreach(string name in globalVariableInkFile.variablesState)
        {
            Ink.Runtime.Object value = globalVariableInkFile.variablesState.GetVariableWithName(name);
            variables.Add(name, value);
           // Debug.Log("Initialized global variable: " + name + " = " + value);
        }
    }

    public void StartListening(Story story)
    {
        LoadVariablesInToStory(story); // must be called before event
        story.variablesState.variableChangedEvent += VariableChanged;
    }

    public void StopListening(Story story)
    {
        story.variablesState.variableChangedEvent -= VariableChanged;
    }

    public void VariableChanged(string name, Ink.Runtime.Object value)
    {
        if (variables.ContainsKey(name))
        {
            variables.Remove(name);
            variables.Add(name, value);
        }
        // else do nothing?
    }

    private void LoadVariablesInToStory(Story story)
    {
        foreach(KeyValuePair<string, Ink.Runtime.Object> variable in variables)
        {
            story.variablesState.SetGlobal(variable.Key, variable.Value);
        }
    }

}
