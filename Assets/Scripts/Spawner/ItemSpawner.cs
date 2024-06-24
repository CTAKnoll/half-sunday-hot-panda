using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("Spawning")]
    public float spawnInterval = 10;
    public Transform[] spawnLocations;

    [Header("Loot Table")]
    public ItemSpawn[] itemSpawns;

    private SortedList<int, ItemSpawn> lootTable = new();
    private int MaxProbability = 1000;


    private void Awake()
    {
        GenerateLootTable();
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(Choose), spawnInterval, spawnInterval);
    }

    public Box Choose()
    {
        int randomValue = UnityEngine.Random.Range(0,MaxProbability);

        GameObject chosenLoot = null;

        for(int i =0; i< itemSpawns.Length; i++)
        {
            if (lootTable.Keys[i] >= randomValue)
            {
                chosenLoot = itemSpawns[i].itemPrefab;
                break;
            }
        }

        if (chosenLoot == null)
            return null;

        var randomLoc = spawnLocations[UnityEngine.Random.Range(0, spawnLocations.Length)];

        Instantiate(chosenLoot, randomLoc.position, Quaternion.identity);

        return chosenLoot.GetComponent<Box>();
    }

    void GenerateLootTable()
    {
        int accumulator = 0;
        for (int i = 0; i < itemSpawns.Length;i++)
        {
            accumulator += itemSpawns[i].probability;
            lootTable.Add(accumulator,itemSpawns[i]);
        }
    }

    [Serializable]
    public struct ItemSpawn
    {
        [Range(0, 1000)]
        public int probability;
        public GameObject itemPrefab;
    }
}
