using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "WeaponData", menuName = "Weapons/WeaponData", order = 1)]
public class WeaponData : ScriptableObject, Boxable
{
    public ProjectileTemplate ProjectileType;
    public int MaxAmmo;
    public float RateOfFire;

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

    public Weapon(WeaponData data)
    {
        Data = data;
        CurrentAmmo = Data.MaxAmmo;
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
}