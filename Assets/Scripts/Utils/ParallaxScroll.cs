using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScroll : MonoBehaviour
{
    public float masterSpeed = 50f;
    public Plane[] parallaxPlanes;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ApplyPlaneSpeeds();
    }

    void ApplyPlaneSpeeds()
    {
        foreach (var plane in parallaxPlanes)
        {
            plane.scrollingTexture.Speed = Mathf.CeilToInt(masterSpeed * plane.parallaxFactor);
        }
    }

    [Serializable]
    public struct Plane
    {
        public ScrollingBackground scrollingTexture;
        [Tooltip("The alters scroll speed of texture by this factor")]
        public float parallaxFactor;
    }
}
