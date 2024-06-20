public interface Damageable
{
    HurtboxType HurtboxType { get; }
    void Damage(int damage);
}