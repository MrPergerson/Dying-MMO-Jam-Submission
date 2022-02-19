using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class Level : SerializedMonoBehaviour
{
    private static Level Instance;

    [SerializeField, ReadOnly] private Objective currentObjective; 
    public Dictionary<string, Objective> objectivesInLevel = new Dictionary<string, Objective>();

    public static Level GetInstance()
    {
        return Instance;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetCurrentObjective(string id)
    {
        objectivesInLevel.TryGetValue(id, out currentObjective);
        if(currentObjective)
        {
            currentObjective.SetAsCurrentObjective();
        }
    }

    public bool CurrentObjectiveMet()
    {
        if (currentObjective)
            return currentObjective.ObjectiveHasBeenMet();
        else
            return false;
    }

}
