using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartySystem : Manager
{
    public static PartySystem Instance;

    [SerializeField, ReadOnly] private List<GameObject> currentParty;
    private List<GameObject> instantiatedAllies;

    float timer = 0.0f;

    bool partyCreated = false;
    bool partyFollowing = false;
    bool partyStopped = false;

    Transform allySpawnPoint;


    bool isReadyToChangeScene = true;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        instantiatedAllies = new List<GameObject>();
    }

    public override void AwakeManager()
    {
        //
    }

    public void spawnParty()
    {
        var allySpawnPointObj = GameObject.FindGameObjectWithTag("PartySystem_AllySpawn");
        if (allySpawnPointObj != null)
        {

            allySpawnPoint = allySpawnPointObj.transform;

            for (int i = 0; i < currentParty.Count; i++)
            {
                var spawnedAlly = currentParty[i];
                spawnedAlly.transform.position = allySpawnPoint.position + new Vector3(Random.Range(-2,2), 0, Random.Range(-2, 2));
                instantiatedAllies.Add(Instantiate(spawnedAlly));
            }

            startFollowing();    
        
        }
    }

    public override bool IsReadyToChangeScene()
    {
        return isReadyToChangeScene;
    }

    public override void OnNewLevelLoaded()
    {
        if(currentParty.Count > 0)
        {
            spawnParty();
        }

    }

    public override void OnSceneChangeRequested()
    {
        if(instantiatedAllies.Count > 0)
        {

           foreach(var ally in instantiatedAllies)
            {
                Destroy(ally);
            }
            instantiatedAllies.Clear();
        }

        isReadyToChangeScene = true;
    }

    public void startFollowing()
    {
        for (int i = 0; i < instantiatedAllies.Count; i++)
        {
            instantiatedAllies[i].GetComponent<Ally>().followPlayer = true;
        }
    }

    public void stopFollowing()
    {
        for (int i = 0; i < instantiatedAllies.Count; i++)
        {
            instantiatedAllies[i].GetComponent<Ally>().followPlayer = false;
        }
    }

    [Button()]
    public void AddAllyToParty(GameObject ally)
    {
        if(ally != null)
            currentParty.Add(ally);
    }

    [Button()]
    public void RemoveAllyToParty(GameObject ally)
    {
        if (currentParty.Contains(ally))
            currentParty.Remove(ally);
    }
}
