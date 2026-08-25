using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkerFollow : MonoBehaviour
{

    private Transform target;
    private RectTransform rect;
    private Canvas canvas;
    private Image image;

    public void Initialize(Transform target, Canvas canvas)
    {
        this.target = target;
        this.canvas = canvas;
        rect = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position);

        // If behind camera, hide marker
        if (screenPos.z < 0)
        {
            image.enabled = false;
            return;
        }
        else
        {
            image.enabled = true;
        }

        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.GetComponent<RectTransform>(),
            screenPos,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out localPos
        );

        rect.localPosition = localPos;
    }
}
