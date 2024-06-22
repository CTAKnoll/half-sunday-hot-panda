using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Audio/Weighted Audio Bank")]
public class WeightedAudioBank : AudioBank
{
    
    [Space]
    [Header("Weighted Audio Clips")]
    [Range(0f, 1f)]
    [Tooltip("The probability that a random \"chance Clip\" will be played instead of a normal audio clip")]
    [SerializeField] private float probability;

    [SerializeField]
    [Tooltip("If the \"probability\" roll succeeeds, return a clip from this list")]
    private AudioClip[] chanceClips;

    public AudioClip GetChanceClip(int index)
    {
        if(index >= chanceClips.Length )
        {
            Debug.LogError($"Chance clip {name}:{index} does not exist");
            return null;
        }

        return chanceClips[index];

    }

    public override AudioClip GetRandom()
    {
        var rand = UnityEngine.Random.Range(0f,1f);
        int randIndex;
        if(rand < probability)
        {// If roll succeeds, return a clip from the special "chance clips"
            randIndex = UnityEngine.Random.Range(0, chanceClips.Length);
            return chanceClips[randIndex];
        }
        else
        {// If roll fails, return a default audio clip
            randIndex = UnityEngine.Random.Range(0, audioClips.Length);
            return audioClips[randIndex];
        }

    }
}
