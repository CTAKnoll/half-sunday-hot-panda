using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceTuner : MonoBehaviour
{
    public AudioSource audioSource;

    private float basePitch = 0f;

    private float _setpoint = 0;
    private float _currPoint = 0;

    [Range(0, 1.5f)]
    public float maxPitchVariance = 0.3f;
    public float smoothTime = 0.8f;

    private void Awake()
    {
        if(audioSource != null)
        {
            basePitch = audioSource.pitch;
        }
    }
    /// <summary>
    /// Sets the tuner setpoint. Value will be clamped between -1 and 1
    /// </summary>
    /// <param name="value"></param>
    public void SetSetpoint(float value)
    {
        _setpoint = Mathf.Clamp(value, -1, 1);
    }

    private void Update()
    {
        var dist = _setpoint - _currPoint;

        if (Mathf.Abs(dist) < Mathf.Epsilon)
        {
            _currPoint = _setpoint;
            return;
        }

        _currPoint += (dist/10) * smoothTime ;
        var lerpValue = (_currPoint / 2) + 0.5f;
        var currPitch = Mathf.Lerp(basePitch - maxPitchVariance, basePitch + maxPitchVariance, lerpValue);

        audioSource.pitch = currPitch;
    }
}
