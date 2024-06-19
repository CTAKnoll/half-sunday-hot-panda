using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Enemy : MonoBehaviour, Damageable
{
    public int MaxHealth;
    private int Health;

    private UnitNavigator nav;
    public Transform FollowTarget;

    private void Awake()
    {
        nav = GetComponent<UnitNavigator>();
        //nav.DestinationReached += OnMoveDone;
    }

    // Start is called before the first frame update
    void Start()
    {
        Health = MaxHealth;
        //nav.SetDestination(FollowTarget.position);
    }

    public void OnMoveDone()
    {
        //nav.SetDestination(FollowTarget.position);
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
