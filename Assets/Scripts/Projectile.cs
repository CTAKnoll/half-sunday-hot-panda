using Services;
using UnityEngine;

namespace DefaultNamespace
{
    public class ProjectileTemplate : Template<Projectile> { }
    public abstract class Projectile : MonoBehaviour
    {
        public float Speed;
        public void Start()
        {
            
        }
    }
}