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

    public Weapon Weapon;

    private void Awake()
    {
        nav = GetComponent<UnitNavigator>();

    }

    // Start is called before the first frame update
    void Start()
    {
        if (ServiceLocator.TryGetService(out TemplateServer server))
        {
            Weapon = new Weapon(server.EnemySMG);
        }
       Health = MaxHealth;
    }

    public void Damage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
