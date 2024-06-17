using UnityEngine;

namespace Services
{
    public class TemplateServer : MonoBehaviour, IService
    {
        public WeaponData PistolTemplate;
        
        protected void Awake()
        {
            ServiceLocator.RegisterAsService(this);
        }
    }
}