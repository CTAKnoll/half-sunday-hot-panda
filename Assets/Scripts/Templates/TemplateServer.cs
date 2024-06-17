using UnityEngine;

namespace Services
{
    public class TemplateServer : MonoBehaviour, IService
    {
        protected void Awake()
        {
            ServiceLocator.RegisterAsService(this);
        }
    }
}