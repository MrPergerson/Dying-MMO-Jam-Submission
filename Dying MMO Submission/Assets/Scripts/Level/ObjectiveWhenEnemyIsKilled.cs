using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveWhenEnemyIsKilled : Objective
{
    [Title("The list of Enemies", "Place all enemy gameObjects that you want to check for as a child of one gameobject and attach it here.")]
    [SerializeField] Transform groupOfEnemies;

    [Title("List of found EnemyAIBrain components", "This script will each child of groupOfEnemies for the EnemyAIBrain component and update this list below. " +
        "This list should match the list of child gameObjects in groupOfEnemies.")]
    [SerializeField, ReadOnly] private List<EnemyAIBrain> enemies;

    [SerializeField, ReadOnly] private int enemiesAlive = 0;

    public override void SetAsCurrentObjective()
    {
        base.SetAsCurrentObjective();
        UpdateEnemyList();
    }

    [Button("Update Enemies list")]
    private void UpdateEnemyList()
    {
        if(enemies != null)
        {
            foreach(var enemy in enemies)
            {
                enemy.onDeath -= SubstractFromEnemiesAlive;
            }
        }

        enemies.Clear();
        for(int i = 0; i < groupOfEnemies.childCount; i++)
        {
            if(groupOfEnemies.GetChild(i).TryGetComponent<EnemyAIBrain>(out EnemyAIBrain enemy))
            {
                enemies.Add(enemy);
                enemy.onDeath += SubstractFromEnemiesAlive;
            }
            else
            {
                Debug.LogError(this + ": GroupOfEnemies contained an object that did not have an EnemyAIBrian component. " +
                    "All child objects of groupOfEnemies should be enemy AI gameObjects with the EnemyAIBrain component");
            }
        }

        enemiesAlive = enemies.Count;
    }
 

    [Button("Clear found data")]
    private void ClearData()
    {
        enemies.Clear();
        enemiesAlive = 0;
    }

    private void RespawnEnemies()
    {
        foreach (var enemy in enemies)
        {
            enemy.Respawn();
        }

        enemiesAlive = enemies.Count;
    }

    private void SubstractFromEnemiesAlive()
    {
        enemiesAlive--;
        if (enemiesAlive <= 0 && IsCurrentObjective()) objectiveMet = true;
    }
}
