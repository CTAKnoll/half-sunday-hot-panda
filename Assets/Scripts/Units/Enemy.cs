using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Enemy : MonoBehaviour, Damageable
{
    public int MaxHealth;
    private int Health;
    
    // Start is called before the first frame update
    void Start()
    {
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
