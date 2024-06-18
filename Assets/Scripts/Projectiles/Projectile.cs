using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class Projectile : MonoBehaviour
{
    public float Speed;
    public int Damage;
    public float Lifetime;
    [NonSerialized] public Vector3 InitialDirection;

    private float SpawnTime;

    protected abstract void Move();

    public void OnEnable()
    {
        SpawnTime = Time.time;
    }
    
    public void Update()
    {
        Move();
        if (Time.time - SpawnTime >= Lifetime)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        var damageable = other.GetComponent<Damageable>();
        if (damageable != null)
        {
            damageable.Damage(Damage);
            Destroy(gameObject);
        }
    }
}