using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioService : MonoBehaviour, IService
{
    public List<AudioBank> banks = new List<AudioBank>();

    private AudioSource _source;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
        ServiceLocator.RegisterAsService(this);
    }

    public AudioClip GetClip(string bankName, int id)
    {
        //search for audio bank
        var bank = banks.Find((bank) => bank.name.Equals(bankName));

        if(bank == null )
        {
            Debug.LogWarning($"Audio bank {bankName} not found");
            return null;
        }

        if(id >= bank.AudioClips.Length )
        {
            Debug.LogError($"Clip {bankName}:{id} does not exist");
            return null;
        }

        return bank.AudioClips[id];
    }

    public void PlaySound(string bankName, int id)
    {
        //search for audio bank
        var bank = banks.Find((bank) => bank.name.Equals(bankName));

        if(bank == null )
        {
            Debug.LogWarning($"Audio bank {bankName} not found");
            return;
        }

        if(id >= bank.AudioClips.Length )
        {
            Debug.LogError($"Clip {bankName}:{id} does not exist");
            return;
        }

        //Play sound in audio source
        var clip  = bank.AudioClips[id];
        _source.PlayOneShot(clip);
    }

    public void PlaySound(AudioClip clip, float volume)
    {
        _source.PlayOneShot(clip, volume);
    }

    public void PlaySound(AudioClip clip)
    {
        _source.PlayOneShot(clip);
    }
}
