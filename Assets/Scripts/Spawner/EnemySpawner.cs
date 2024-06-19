using System;
using System.Collections.Generic;
using Spawner;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<EnemySubstage> Substages;

    private void Start()
    {
        Substages[0].SpawnAll();
    }

}

[Serializable]
public struct EnemySubstage
{
    public List<EnemySpawn> Spawns;

    public void SpawnAll()
    {
        foreach(EnemySpawn s in Spawns)
        {
            var enemy = s.EnemyType.Instantiate(s.SpawnPoint.position);
            enemy.FollowTarget = s.DestinationPoint;
        }
    }
}

[Serializable]
public struct EnemySpawn
{
    public EnemyTemplate EnemyType;
    public Transform SpawnPoint;
    public Transform DestinationPoint;
}