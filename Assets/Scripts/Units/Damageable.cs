public interface Damageable
{
    public static readonly string DAMAGE_SFX_BANK = "damage";
    HurtboxType HurtboxType { get; }
    void Damage(int damage);
}