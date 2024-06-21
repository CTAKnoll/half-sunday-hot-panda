using System;
using System.Collections;
using System.Collections.Generic;
using Services;
using UnityEngine;

public class UnitNavigator : MonoBehaviour
{
    private SpacialPartitionAgent _partitionAgent;
    private Rigidbody _rb;
    private Vector3 _destination;
    private Vector3 _startPosition;
    private float _lerpPosition;

    public AnimationCurve AnimCurve;

    public float MoveSpeed;
    [NonSerialized] public bool IsMoving = true;
    public event Action DestinationReached;

    private SpacialManager SpacialManager;

    private void Awake()
    {
        _partitionAgent = GetComponent<SpacialPartitionAgent>();
        ServiceLocator.TryGetService(out SpacialManager);
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        var moveDistance = Vector3.Distance(_destination, _startPosition);
        if (moveDistance <= float.Epsilon)
        {
            IsMoving = false;
            return;
        }

        var motion = (MoveSpeed/moveDistance) * Time.deltaTime;
        _lerpPosition += motion;
        var curveOutput = AnimCurve.Evaluate(_lerpPosition);
        var movement = Vector3.LerpUnclamped(_startPosition, _destination, curveOutput);
        
        if (_lerpPosition >= 1)
        {
            Stop();
            DestinationReached?.Invoke();
            return;
        }

        _rb.MovePosition(movement);
    }

    public void SetDestination(Vector3Int newDestination)
    {
        if (newDestination == SpacialManager.INVALID_PARTITION)
        {
            Debug.LogError("TRYING TO SET INVALID DESTINATION");
            return;
        }
        Vector3 worldPosition = SpacialManager.PartitionToWorld(newDestination);
        SpacialManager.UpdateDestinationPartition(_partitionAgent, newDestination);
        
        IsMoving = !worldPosition.Equals(transform.position);
        _lerpPosition = 0;
        _startPosition = transform.position;
        _destination = worldPosition;
    }

    public void Stop()
    {
        IsMoving = false;
        _lerpPosition = 1;
    }
}
