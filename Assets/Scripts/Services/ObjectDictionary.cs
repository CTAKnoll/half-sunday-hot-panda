using UnityEngine;

namespace Services
{
    public class ObjectDictionary : MonoBehaviour, IService
    {
        public ScrollingBackground Road;

        public void Awake()
        {
            ServiceLocator.RegisterAsService(this);
        }
    }
}