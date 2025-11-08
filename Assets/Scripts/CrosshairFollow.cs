using UnityEngine;
using UnityEngine.UI; 

public class CrosshairFollow : MonoBehaviour
{
    public RectTransform crosshair;
    public Canvas canvas;
    public Camera mainCamera;

    void Update()
    {
        if (crosshair == null || canvas == null) return;

        Vector2 mousePos = Input.mousePosition;
        Vector2 anchoredPos;

        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            crosshair.position = mousePos;
        }
        else
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                mousePos,
                canvas.renderMode == RenderMode.ScreenSpaceCamera ? mainCamera : null,
                out anchoredPos
            );

            crosshair.anchoredPosition = anchoredPos;
        }
    }
}
