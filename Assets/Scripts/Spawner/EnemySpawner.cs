using System;
using System.Collections;
using System.Collections.Generic;
using Services;
using Spawner;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private static readonly string GENERAL_SFX_BANK = "general";
    private static readonly string SPAWN_SFX_BANK = "spawn_bark";

    public bool loop = true;

    [Header("Game SFX")]
    public int spawnWarning_sfx = 0;

    [Header("Spawn Settings")]
    public List<EnemySubstage> Substages;

    private List<GameObject> spawnedObjects = new();

    private UIRoot _uiRoot;

    private int _currentSubstage = 0;

    private AudioService _audio;

    private void Start()
    {
        ServiceLocator.TryGetService(out _audio);
        ServiceLocator.TryGetService(out _uiRoot);

        _currentSubstage = -1;
        NextSubstage();
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
            if (loop)
            {
                _currentSubstage = -1;
            }
            else
            {
                Debug.Log("No more enemy stages to spawn");
                return;
            }
        }

        spawnedObjects.Clear();

        _currentSubstage++;

        StartCoroutine(nameof(WarnThenSpawn), _uiRoot);
    }

    IEnumerator WarnThenSpawn(UIRoot uiRoot)
    {
        Substages[_currentSubstage].SpawnWarnings(uiRoot);
        _audio.PlaySound(GENERAL_SFX_BANK, spawnWarning_sfx);
        yield return new WaitForSeconds(Substages[_currentSubstage].warningTime);

        spawnedObjects = new List<GameObject>(Substages[_currentSubstage].SpawnAll());
        _audio.PlayRandomSound(SPAWN_SFX_BANK);
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
    public float warningTime;
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

    public void SpawnWarnings(UIRoot uiRoot)
    {
        foreach(EnemySpawn s in Spawns)
        {
            var prefab = s.EnemyType.warningPrefab;
            ScreenIndicator indicator = GameObject.Instantiate(prefab, s.SpawnPoint.position, uiRoot.transform.rotation, uiRoot.transform).GetComponent<ScreenIndicator>();
            indicator.Target = s.SpawnPoint;
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