using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartySystem : Manager
{
    public static PartySystem Instance;

    [SerializeField]
    private List<GameObject> allies;

    [SerializeField, ReadOnly] private Ally[] currentParty;

    float timer = 0.0f;

    bool partyCreated = false;
    bool partyFollowing = false;
    bool partyStopped = false;

    Transform allySpawnPoint;

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
    }

    public override void AwakeManager()
    {
        //
    }

    public void createParty(Vector3 spawnLocation)
    {
        currentParty= new Ally[allies.Count];
        for (int i = 0; i < currentParty.Length; i++)
        {
            currentParty[i] = Instantiate(allies[i].GetComponent<Ally>());
            currentParty[i].transform.position =spawnLocation+ new Vector3(Random.Range(-2,2), 0, Random.Range(-2, 2));
            currentParty[i].GetComponent<Ally>().followPlayer = false;
        }
    }

    public override bool IsReadyToChangeScene()
    {
        return true;
    }

    public override void OnNewLevelLoaded()
    {
        var allySpawnPointObj = GameObject.FindGameObjectWithTag("PartySystem_AllySpawn");
        if(allySpawnPointObj)
        {
            allySpawnPoint = allySpawnPointObj.transform;
            createParty(allySpawnPoint.position);
            startFollowing();

        }

    }

    public override void OnSceneChangeRequested()
    {
        throw new System.NotImplementedException();
    }

    public void startFollowing()
    {
        for (int i = 0; i < currentParty.Length; i++)
        {
            currentParty[i].followPlayer = true;
        }
    }

    public void stopFollowing()
    {
        for (int i = 0; i < currentParty.Length; i++)
        {
            currentParty[i].followPlayer = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
