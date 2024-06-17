using UnityEngine;

public class SimpleBullet : Projectile
{
    private void Start()
    {
        transform.rotation = Quaternion.LookRotation(InitialDirection, Vector3.up);
    }
    
    protected override void Move()
    {
        transform.position += Speed * Time.deltaTime * InitialDirection;
    }
}