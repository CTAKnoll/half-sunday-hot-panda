using System;
using System.Collections.Generic;
using Spawner;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<EnemySubstage> Substages;
}

[Serializable]
public struct EnemySubstage
{
    public List<EnemySpawn> Spawns;
}

[Serializable]
public struct EnemySpawn
{
    public EnemyTemplate EnemyType;
    public Vector3 SpawnPoint;
    public Vector3 DestinationPoint;
}