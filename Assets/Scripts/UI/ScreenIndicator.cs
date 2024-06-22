using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ScreenIndicator : MonoBehaviour
{
    UIRoot screenSpaceManager;
    RectTransform _rectTransform;

    public GameObject centerGraphic;
    [FormerlySerializedAs("direction")]
    public GameObject arrowGraphic;

    public float lifeTime = 1f;

    public Transform Target;

    // Start is called before the first frame update
    void Start()
    {
        ServiceLocator.TryGetService(out screenSpaceManager);
        _rectTransform = GetComponent<RectTransform>();
        screenSpaceManager.ClampToCanvas(_rectTransform);


        InvokeRepeating(nameof(ToggleGraphic), 0.2f, 0.15f);
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        screenSpaceManager.ClampToCanvas(_rectTransform);
    }

    public void SetIndicationDirection(Vector3 direction)
    {
        var angle = Vector2.SignedAngle(Vector2.up, direction);
        var orig = arrowGraphic.transform.eulerAngles;
        arrowGraphic.transform.eulerAngles = new Vector3(orig.x, orig.y, angle);
    }

    void ToggleGraphic()
    {
        centerGraphic.SetActive(!centerGraphic.activeInHierarchy);
    }
}
