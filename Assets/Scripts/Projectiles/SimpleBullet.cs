using UnityEngine;

public class SimpleBullet : Projectile
{
    protected override void Move()
    {
        transform.position += Speed * Time.deltaTime * InitialDirection;
    }
}