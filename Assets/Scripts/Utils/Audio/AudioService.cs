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

    private bool TryGetAudioBank(string bankName, out AudioBank audioBank)
    {
        audioBank = null;
        //search for audio bank
        var bank = banks.Find((bank) => bank.name.Equals(bankName));

        if(bank != null)
        {
            audioBank = bank;
            return true;
        }

        return false;
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


        return bank.GetClip(id);
    }

    public void PlaySound(string bankName, int id)
    {
        //search for audio bank
        if(!TryGetAudioBank(bankName, out AudioBank bank))
        {
            Debug.LogWarning($"Audio bank \"{bankName}\" not found");
            return;
        }

        //Play sound in audio source
        var clip  = bank.GetClip(id);
        _source.PlayOneShot(clip);
    }

    public void PlaySound(AudioClip clip, float volume = 1)
    {
        _source.PlayOneShot(clip, volume);
    }

    public void PlayRandomSound(string bankName)
    {
        if(!TryGetAudioBank(bankName, out AudioBank bank))
        {
            Debug.LogWarning($"Audio bank \"{bankName}\" not found");
            return;
        }

        _source.PlayOneShot(bank.GetRandom());
    }
}
