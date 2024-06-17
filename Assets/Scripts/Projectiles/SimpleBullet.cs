public class SimpleBullet : Projectile
{
    protected override void Move()
    {
        transform.position += Speed * InitialDirection;
    }
}