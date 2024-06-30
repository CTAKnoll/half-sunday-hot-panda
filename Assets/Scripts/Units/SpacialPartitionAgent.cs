using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpacialPartitionAgent : MonoBehaviour
{
    private Vector3Int _partition;
    public Vector3Int Partition => _partition;

    SpacialManager _manager;

    private void Start()
    {
        ServiceLocator.TryGetService(out _manager);
    }

    private void Update()
    {
        var newPartition = _manager.UpdateCurrentPartition(this);
        _partition = newPartition;
    }
}
