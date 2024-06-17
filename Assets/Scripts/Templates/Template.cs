using UnityEngine;

namespace Services
{
    public class Template<T> : ScriptableObject where T: MonoBehaviour
    {
        public T Prefab;

        public T Instantiate(GameObject parent)
        {
            return GameObject.Instantiate(Prefab, parent.transform);
        }
        
        public T Instantiate(Vector3 position)
        {
            return GameObject.Instantiate(Prefab, position, Quaternion.identity);
        }
    }
}