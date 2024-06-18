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
        Debug.Log("START");
    }
    
    public void Update()
    {
        Move();
        if (Time.time - SpawnTime >= Lifetime)
        {
            Debug.Log("YAAAS");
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