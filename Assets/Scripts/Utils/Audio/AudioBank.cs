using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName ="Audio/Audio Bank")]
public class AudioBank : ScriptableObject
{
    [SerializeField]
    [FormerlySerializedAs("_audioClips")]
    protected AudioClip[] audioClips;

    public AudioClip GetClip(int index)
    {        
        if(index >= audioClips.Length )
        {
            Debug.LogError($"Clip {name}:{index} does not exist");
            return null;
        }

        return audioClips[index];
    }

    public virtual AudioClip GetRandom()
    {
        var rand = UnityEngine.Random.Range( 0, audioClips.Length );
        return audioClips[rand];
    }
}
