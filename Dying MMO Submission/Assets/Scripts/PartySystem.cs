using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartySystem : MonoBehaviour
{
    [SerializeField]
    private GameObject allyPrefab;

    private GameObject[] generatedAllies;

    float timer = 0.0f;

    bool partyCreated = false;
    bool partyFollowing = false;
    bool partyStopped = false;

    public void createParty(int numAlliesToCreate, Vector3 spawnLocation)
    {
        generatedAllies=new GameObject[numAlliesToCreate];
        for (int i = 0; i < numAlliesToCreate; i++)
        {
            generatedAllies[i] = Instantiate(allyPrefab);
            generatedAllies[i].transform.position =spawnLocation+ new Vector3(Random.Range(-5,5), 0, Random.Range(-5, 5));
            generatedAllies[i].GetComponent<Ally>().followPlayer = false;
        }
    }

    public void startFollowing()
    {
        for (int i = 0; i < generatedAllies.Length; i++)
        {
            generatedAllies[i].GetComponent<Ally>().followPlayer = true;
        }
    }

    public void stopFollowing()
    {
        for (int i = 0; i < generatedAllies.Length; i++)
        {
            generatedAllies[i].GetComponent<Ally>().followPlayer = false;
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
