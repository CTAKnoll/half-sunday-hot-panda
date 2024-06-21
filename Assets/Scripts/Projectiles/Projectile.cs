using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class Projectile : MonoBehaviour
{
    public float Speed;
    public int Damage;
    public float Lifetime;
    [NonSerialized] public Vector3 InitialDirection;
    [NonSerialized] public GameObject Owner;

    [Tooltip("What type of entity takes damage from this projectile")]
    [SerializeField] private HurtboxMask HurtboxMask;
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

    protected virtual bool IsSelfDamage(Collider other)
    {
        return Owner != null && other.transform.IsChildOf(Owner.transform);
    }

    protected bool CanHurt(Damageable damageable)
    {
        return HurtboxMask.HasFlag((HurtboxMask)damageable.HurtboxType);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (IsSelfDamage(other))
            return;

        var damageable = other.GetComponent<Damageable>();
        if (damageable != null && CanHurt(damageable))
        {
            damageable.Damage(Damage);
            Destroy(gameObject);
        }
    }
}