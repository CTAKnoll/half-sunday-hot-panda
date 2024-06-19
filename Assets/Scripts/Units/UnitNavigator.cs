using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitNavigator : MonoBehaviour
{
    private CharacterController _charController;
    private Rigidbody _rb;
    private Vector3 _destination;
    private Vector3 _startPosition;
    private float _lerpPosition;

    public AnimationCurve AnimCurve;

    public float MoveSpeed;
    public bool IsMoving = true;
    public event Action DestinationReached;

    private void Awake()
    {
        _charController = GetComponent<CharacterController>();
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        var moveDistance = Vector3.Distance(_destination, _startPosition);
        if (moveDistance <= float.Epsilon)
            return;

        var motion = (MoveSpeed/moveDistance) * Time.deltaTime;
        _lerpPosition += motion;
        var curveOutput = AnimCurve.Evaluate(_lerpPosition);
        var movement = Vector3.LerpUnclamped(_startPosition, _destination, curveOutput);
        //_charController.Move(movement);
        if (_lerpPosition >= 1)
        {
            Stop();
            DestinationReached?.Invoke();
        }

        _rb.MovePosition(movement);
    }

    public void SetDestination(Vector3 destination)
    {
        IsMoving = !_destination.Equals(transform.position);
        _lerpPosition = 0;
        _startPosition = transform.position;
        _destination = destination;
    }

    public void Stop()
    {
        IsMoving = false;
        _lerpPosition = 0;
        _destination = transform.position;
    }
}
