using System;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class WeaponData : ScriptableObject
{
    public ProjectileTemplate ProjectileType;
    public int MaxAmmo;
    public float RateOfFire;
}

public class Weapon
{
    public float CurrentAmmo;
    public WeaponData Data;
    public event Action OnWeaponEmpty;

    private float LastFireTime;

    public Weapon(WeaponData data)
    {
        Data = data;
        CurrentAmmo = Data.MaxAmmo;
    }

    public void TryFireWeapon(GameObject source, Vector3 target)
    {
        if (!CanFireWeapon())
            return;

        LastFireTime = Time.time;
        var projectile = Data.ProjectileType.Instantiate();
        projectile.InitialDirection = (target - source.transform.position).normalized;
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
}