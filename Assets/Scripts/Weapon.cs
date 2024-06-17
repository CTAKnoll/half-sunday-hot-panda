using System;
using UnityEngine;

namespace DefaultNamespace
{
    [Serializable]
    public struct WeaponData
    {
        public ProjectileTemplate ProjectileType;
        public int MaxAmmo;
        public float RateOfFire;
    }
    
    public class Weapon : MonoBehaviour
    {
        
    }
}