using System;
using System.Collections;
using System.Collections.Generic;
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

    public static readonly float AVOIDANCE_RADIUS = 0.9f;
    public event Action DestinationReached;

    private void Awake()
    {
        _partitionAgent = GetComponent<SpacialPartitionAgent>();
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

    public void SetDestination(Vector3 newDestination)
    {
        IsMoving = !newDestination.Equals(transform.position);
        _lerpPosition = 0;
        _startPosition = transform.position;
        _destination = newDestination;
    }

    public void Stop()
    {
        SetDestination(transform.position);
        IsMoving = false;
        _lerpPosition = 0;
    }
}
