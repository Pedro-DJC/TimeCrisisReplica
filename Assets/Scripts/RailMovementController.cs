using UnityEngine;
using Unity.Cinemachine;

public class RailMovementController : MonoBehaviour
{
    public CinemachineSplineCart splineCart;

    public float speed = 5f;

    public bool loop = false;

    bool isMoving = true;

    void Start()
    {
        if (splineCart == null)
            splineCart = GetComponent<CinemachineSplineCart>();
    }

    void Update()
    {
        if (!isMoving || Time.timeScale == 0f) return;
        if (splineCart == null) return;
        if (splineCart.Spline == null) return;

        float splineLength = splineCart.Spline.CalculateLength();

        float delta;
        if (splineCart.PositionUnits == Unity.Cinemachine.PathIndexUnit.Distance)
        {
            delta = speed * Time.deltaTime;
            splineCart.SplinePosition += delta;
        }
        else if (splineCart.PositionUnits == Unity.Cinemachine.PathIndexUnit.Normalized)
        {
            float speedNormalized = (splineLength > 0f) ? speed / splineLength : speed;
            delta = speedNormalized * Time.deltaTime;
            splineCart.SplinePosition += delta;
        }
        else
        {
            float speedNormalized2 = (splineLength > 0f) ? speed / splineLength : speed;
            delta = speedNormalized2 * Time.deltaTime;
            splineCart.SplinePosition += delta;
        }

        float maxPos = (splineCart.PositionUnits == Unity.Cinemachine.PathIndexUnit.Distance) ? splineLength : 1f;
        if (splineCart.SplinePosition >= maxPos)
        {
            if (loop)
            {
                splineCart.SplinePosition = 0f + (splineCart.SplinePosition - maxPos);
            }
            else
            {
                splineCart.SplinePosition = maxPos;
                isMoving = false;
            }
        }
    }
    public void StartMoving() => isMoving = true;
    public void StopMoving() => isMoving = false;
    public void SetSpeed(float s) => speed = s;
}
