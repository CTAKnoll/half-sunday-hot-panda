using System.Runtime.CompilerServices;
using Services;
using UnityEngine;

public class Box : MonoBehaviour
{
    public static readonly string PICKUP_SFX_BANK = "pickups";
    public WeaponData Contents;

    private ObjectDictionary ObjectDictionary;
    private int RoadScrollSpeed;
    private Vector3 RoadVector;

    private AudioService _audio;
    [Header("Pickup SFX IDs")]
    public int commonPickup_sfx = 0;

    public int gunSound_sfx = 3;

    public void OnTriggerEnter(Collider target)
    {
        var player = target.GetComponent<PlayerController>();
        if (player != null)
        {
            Contents.Unbox(player);
            _audio.PlaySound(PICKUP_SFX_BANK, commonPickup_sfx);
            _audio.PlaySound(PICKUP_SFX_BANK, gunSound_sfx);
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        ServiceLocator.TryGetService(out ObjectDictionary);
        ServiceLocator.TryGetService(out _audio);
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
