using UnityEngine;

namespace Services
{
    public class TemplateServer : MonoBehaviour, IService
    {
        public WeaponData PistolTemplate;

        public WeaponData EnemySMG;

        protected void Awake()
        {
            ServiceLocator.RegisterAsService(this);
        }
    }
}