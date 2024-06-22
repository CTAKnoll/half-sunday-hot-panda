using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenIndicator : MonoBehaviour
{
    ScreenSpaceManager screenSpaceManager;
    RectTransform _rectTransform;
    // Start is called before the first frame update
    void Start()
    {
        ServiceLocator.TryGetService(out screenSpaceManager);
        _rectTransform = GetComponent<RectTransform>();
        screenSpaceManager.ClampToCanvas(_rectTransform);
    }

    // Update is called once per frame
    void Update()
    {
        screenSpaceManager.ClampToCanvas(_rectTransform);
    }
}
