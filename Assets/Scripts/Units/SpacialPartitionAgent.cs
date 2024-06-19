using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpacialPartitionAgent : MonoBehaviour
{
    private Vector3Int _partition;
    public Vector3Int Partition => _partition;

    private static SpacialManager Manager;

    private void Start()
    {
        if(Manager == null)
        {
            ServiceLocator.TryGetService(out Manager);
        }
    }

    // Update is called once per frame
    void Update()
    {
        var newPartition = Manager.UpdatePartition(this);
        _partition = newPartition;
    }
}
