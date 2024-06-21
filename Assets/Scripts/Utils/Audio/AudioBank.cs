using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Audio Bank")]
public class AudioBank : ScriptableObject
{
    [SerializeField]
    private AudioClip[] _audioClips;
    public AudioClip[] AudioClips => _audioClips;
}
