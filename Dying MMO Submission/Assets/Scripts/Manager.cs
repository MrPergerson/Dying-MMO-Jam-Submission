using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Manager : MonoBehaviour
{
    protected bool isAwake = false;

    public abstract void AwakeManager();

    public abstract void OnNewLevelLoaded();

    public abstract void OnSceneChangeRequested();

    public abstract bool IsReadyToChangeScene();
}
