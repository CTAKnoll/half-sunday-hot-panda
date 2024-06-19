using System.Runtime.CompilerServices;
using Services;
using UnityEngine;

public class Box : MonoBehaviour
{
    public WeaponData Contents;

    private ObjectDictionary ObjectDictionary;
    private int RoadScrollSpeed;
    private Vector3 RoadVector;
    public void OnTriggerEnter(Collider target)
    {
        var player = target.GetComponent<PlayerController>();
        if (player != null)
        {
            Contents.Unbox(player);
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        ServiceLocator.TryGetService(out ObjectDictionary);
        RoadScrollSpeed = ObjectDictionary.Road.Speed;
        RoadVector = ObjectDictionary.Road.transform.right;
    }
    
    public void Update()
    {
        // YO MAGIC NUMBER POG
        //Debug.Log(RoadVector);
        transform.position += RoadVector * RoadScrollSpeed * Time.deltaTime * 0.01f;
    }
}
