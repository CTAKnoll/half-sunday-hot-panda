using Services;
using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Enemy : MonoBehaviour, Damageable
{
    public int MaxHealth;
    private int Health;

    private UnitNavigator nav;
    public Transform FollowTarget;

    public WeaponData WeaponData;
    public Weapon Weapon;

    [SerializeField]
    HurtboxType _hurtboxType;
    public HurtboxType HurtboxType => _hurtboxType;

    private AudioService _audio;
    [Header("Damage SFX IDs")]
    [Min(0)]
    public int damage_sfx = 3;
    [Min(0)]
    public int death_sfx = 4;

    private void Awake()
    {
        nav = GetComponent<UnitNavigator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (ServiceLocator.TryGetService(out TemplateServer server))
        {
            Weapon = new Weapon(WeaponData != null ? WeaponData : server.EnemySMG, gameObject);
        }

        ServiceLocator.TryGetService(out _audio);
       Health = MaxHealth;
    }

    public void Damage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            _audio.PlaySound(Damageable.DAMAGE_SFX_BANK, death_sfx);
            Destroy(gameObject);
            return;
        }
        _audio.PlaySound(Damageable.DAMAGE_SFX_BANK, damage_sfx);
    }
}
