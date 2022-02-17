using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExit : MonoBehaviour, ILevelExit, ISelectable
{
    
    public event ILevelExit.GoToLevel onGoToLevel;

    [SerializeField] private string levelToExitTo;

    public virtual void TravelToLevel(string name)
    {
        onGoToLevel?.Invoke(name);
    }

    public virtual void Select()
    {
        TravelToLevel(levelToExitTo);
    }
}
