using UnityEngine;
using UnityEngine.UI; 

public class CrosshairFollow : MonoBehaviour
{
    public RectTransform crosshair; 
    public Canvas canvas;

    void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        crosshair.position = mousePos;
    }
}
