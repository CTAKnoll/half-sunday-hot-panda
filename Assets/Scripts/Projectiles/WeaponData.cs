using Services;
using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "WeaponData", menuName = "Weapons/WeaponData", order = 1)]
public class WeaponData : ScriptableObject, Boxable
{
    public ProjectileTemplate ProjectileType;
    public int MaxAmmo;
    public float RateOfFire;

    public AudioClip[] FireSounds;

    public void Unbox(PlayerController player)
    {
        player.AcquireWeapon(new Weapon(this));
    }
}

public class Weapon
{
    public WeaponData Data;
    public float CurrentAmmo;
    public event Action OnWeaponEmpty;

    public GameObject Owner;
    private float LastFireTime;
    private int _lastFireSound;

    private AudioService _audio;
    public Weapon(WeaponData data)
    {
        Data = data;
        CurrentAmmo = Data.MaxAmmo;
        ServiceLocator.TryGetService(out  _audio);
    }

    public Weapon(WeaponData data, GameObject owner) : this(data)
    {
        Owner = owner;
    }

    public void TryFireWeapon(GameObject source, Vector3 target)
    {
        if (!CanFireWeapon())
            return;

        LastFireTime = Time.time;
        var projectile = Data.ProjectileType.Instantiate(source.transform.position);
        PlayRandomFireSound();
        projectile.InitialDirection = (target - source.transform.position).normalized;
        projectile.Owner = this.Owner;
        CurrentAmmo--;
        if (CurrentAmmo == 0)
            OnWeaponEmpty?.Invoke();
    }

    public string GetAmmoCount()
    {
        if (CurrentAmmo > 1000)
            return "∞";
        return $"{CurrentAmmo}";
    }

    private bool CanFireWeapon()
    {
        return (Time.time - LastFireTime) >= (1 / Data.RateOfFire);
    }

    private void PlayRandomFireSound()
    {
        var rand = UnityEngine.Random.Range(0, Data.FireSounds.Length);
        
        _audio.PlaySound(Data.FireSounds[rand]);
        _lastFireSound = rand;
    }
}