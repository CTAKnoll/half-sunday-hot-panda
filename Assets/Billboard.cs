using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    //[SerializeField] private BillboardType billboardType;
    //public enum BillboardType {LookAtCamera, CameraForward};

    private void LateUpdate()
    {
        transform.forward = Camera.main.transform.forward;
    }
}
