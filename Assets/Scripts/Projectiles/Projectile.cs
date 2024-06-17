using Services;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class Projectile : MonoBehaviour
{
    public float Speed;
    public int Damage;
    [DoNotSerialize] public Vector3 InitialDirection;

    protected abstract void Move();

    public void Update()
    {
        Move();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        var damageable = other.GetComponent<Damageable>();
        if (damageable != null)
        {
            damageable.Damage(Damage);
            Destroy(this);
        }
    }
}