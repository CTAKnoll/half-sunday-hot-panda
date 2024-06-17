using UnityEngine;

namespace Services
{
    public class Template<T> : ScriptableObject where T: MonoBehaviour
    {
        public T Prefab;

        public T Instantiate(GameObject parent = null)
        {
            if(parent == null)
                return GameObject.Instantiate(Prefab);
            return GameObject.Instantiate(Prefab, parent.transform);
        }
    }
}