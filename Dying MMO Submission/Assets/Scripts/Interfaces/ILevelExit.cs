using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILevelExit
{
    public delegate void GoToLevel(string name);

    public void TravelToLevel(string name);
}
