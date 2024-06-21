using System;
using System.Collections.Generic;
using Spawner;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<EnemySubstage> Substages;

    private List<GameObject> spawnedObjects;

    private int _currentSubstage = 0;
    private void Start()
    {
        spawnedObjects = new List<GameObject>(Substages[_currentSubstage].SpawnAll());
    }

    private void Update()
    {
        if (spawnedObjects.Count == 0)
            return;

        foreach(var obj in spawnedObjects)
        {
            if(obj == null)
                continue;

        }

        if(AllEnemiesDestroyed())
        {
            NextSubstage();
        }
    }

    private void NextSubstage()
    {
        if (_currentSubstage >= Substages.Count - 1)
        {
            Debug.Log("No more enemy stages to spawn");
            return;
        }

        spawnedObjects.Clear();

        _currentSubstage++;
        spawnedObjects = new List<GameObject>(Substages[_currentSubstage].SpawnAll());
    }

    private bool AllEnemiesDestroyed()
    {
        foreach (var obj in spawnedObjects)
        {
            if (obj != null)
                return false;
        }

        //All objects were null
        return true;
    }
}

[Serializable]
public struct EnemySubstage
{
    public List<EnemySpawn> Spawns;

    public GameObject[] SpawnAll()
    {
        List<GameObject> spawnedObjects = new List<GameObject>();
        foreach(EnemySpawn s in Spawns)
        {
            var enemy = s.EnemyType.Instantiate(s.SpawnPoint.position);
            spawnedObjects.Add(enemy.gameObject);
            enemy.FollowTarget = s.DestinationPoint;
        }

        return spawnedObjects.ToArray();
    }
}

[Serializable]
public struct EnemySpawn
{
    public EnemyTemplate EnemyType;
    public Transform SpawnPoint;
    public Transform DestinationPoint;
}