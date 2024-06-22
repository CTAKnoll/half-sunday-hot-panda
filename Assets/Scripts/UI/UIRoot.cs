using Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class UIRoot : MonoBehaviour, IService
{
    private Canvas _canvas;
    private Camera _mainCamera;

    private void Awake()
    {
        _canvas = GetComponentInParent<Canvas>();
        _mainCamera = Camera.main;
        ServiceLocator.RegisterAsService(this);
    }

    public Vector2 GetScreenPosition(Vector3 worldPos)
    {
        Debug.Log(_mainCamera.WorldToScreenPoint(worldPos));
        return worldPos;
    }

    public Rect ClampToCanvas(Rect rect)
    {
        var origWidth = rect.width;
        var origHeight = rect.height;

        var fitRect = new Rect(rect);
        var canvasRect = _canvas.pixelRect;
        canvasRect.center -= new Vector2(_canvas.pixelRect.width / 2, _canvas.pixelRect.height / 2);

        //Check if each corner of the rect is on-screen
        if (IsRectOnscreen(canvasRect, rect))
            return fitRect;

        //Try to clamp min point of rect
        //move input rect to nearest point on canvas rect
        fitRect.xMin = Mathf.Clamp(fitRect.xMin, canvasRect.xMin, canvasRect.xMax);
        fitRect.xMax = fitRect.xMin + origWidth;

        fitRect.yMin = Mathf.Clamp(fitRect.yMin, canvasRect.yMin, canvasRect.yMax);
        fitRect.yMax = fitRect.yMin + origHeight;

        fitRect.xMax = Mathf.Clamp(fitRect.xMax, canvasRect.xMin, canvasRect.xMax);
        fitRect.xMin = fitRect.xMax - origWidth;

        fitRect.yMax = Mathf.Clamp(fitRect.yMax, canvasRect.yMin, canvasRect.yMax);
        fitRect.yMin = fitRect.yMax - origHeight;

        return fitRect;
    }

    public void ClampToCanvas(RectTransform rectTransform)
    {
        var fitRect = rectTransform.rect;
        fitRect.center += (Vector2)rectTransform.localPosition;

        var origWidth = fitRect.width;
        var origHeight = fitRect.height;

        var canvasRect = _canvas.pixelRect;
        canvasRect.center -= new Vector2(_canvas.pixelRect.width / 2, _canvas.pixelRect.height / 2);

        //Check if each corner of the rect is on-screen
        if (IsRectOnscreen(canvasRect, fitRect))
            return;

        //Try to clamp min point of rect
        //move input rect to nearest point on canvas rect
        fitRect.xMin = Mathf.Clamp(fitRect.xMin, canvasRect.xMin, canvasRect.xMax);
        fitRect.xMax = fitRect.xMin + origWidth;

        fitRect.yMin = Mathf.Clamp(fitRect.yMin, canvasRect.yMin, canvasRect.yMax);
        fitRect.yMax = fitRect.yMin + origHeight;

        fitRect.xMax = Mathf.Clamp(fitRect.xMax, canvasRect.xMin, canvasRect.xMax);
        fitRect.xMin = fitRect.xMax - origWidth;

        fitRect.yMax = Mathf.Clamp(fitRect.yMax, canvasRect.yMin, canvasRect.yMax);
        fitRect.yMin = fitRect.yMax - origHeight;

        rectTransform.localPosition = fitRect.center;
    }

    public bool IsRectOnscreen(Rect canvasRect, Rect testRect)
    {
        return (canvasRect.Contains(testRect.min) && canvasRect.Contains(testRect.max));
    }

    public void InstantiateUIElement(GameObject prefab)
    {
        Instantiate(prefab, _canvas.transform);
    }
}
