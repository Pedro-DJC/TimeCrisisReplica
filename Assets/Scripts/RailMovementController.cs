using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.Splines;
using System.Collections;

public class RailMovementController : MonoBehaviour
{
    public CinemachineSplineCart splineCart;

    public float speed = 5f;
    public bool loop = false;

    private bool isMoving = true;

    void Start()
    {
        if (splineCart == null)
            splineCart = GetComponent<CinemachineSplineCart>();
    }

    void Update()
    {
        if (!isMoving || Time.timeScale == 0f) return;
        if (splineCart == null || splineCart.Spline == null) return;

        float splineLength = splineCart.Spline.CalculateLength();

        float delta;
        if (splineCart.PositionUnits == PathIndexUnit.Distance)
        {
            delta = speed * Time.deltaTime;
            splineCart.SplinePosition += delta;
        }
        else
        {
            float speedNormalized = (splineLength > 0f) ? speed / splineLength : speed;
            delta = speedNormalized * Time.deltaTime;
            splineCart.SplinePosition += delta;
        }

        float maxPos = (splineCart.PositionUnits == PathIndexUnit.Distance) ? splineLength : 1f;
        if (splineCart.SplinePosition >= maxPos)
        {
            if (loop)
                splineCart.SplinePosition = 0f + (splineCart.SplinePosition - maxPos);
            else
            {
                splineCart.SplinePosition = maxPos;
                isMoving = false;
            }
        }
    }

    public void MoveToPosition(float targetPosition)
    {
        StartCoroutine(MoveToPositionCoroutine(targetPosition));
    }

    private IEnumerator MoveToPositionCoroutine(float target)
    {
        float current = splineCart.SplinePosition;
        float dir = Mathf.Sign(target - current);

        while ((dir > 0 && splineCart.SplinePosition < target) ||
               (dir < 0 && splineCart.SplinePosition > target))
        {
            float delta = (speed * Time.deltaTime) / splineCart.Spline.CalculateLength();
            splineCart.SplinePosition += delta * dir;
            yield return null;
        }

        splineCart.SplinePosition = target;
        StopMoving();
    }

    public void StartMoving()
    {
        if (!loop)
        {
            float maxPos = (splineCart.PositionUnits == PathIndexUnit.Distance)
                ? splineCart.Spline.CalculateLength()
                : 1f;

            if (splineCart.SplinePosition >= maxPos)
                splineCart.SplinePosition = Mathf.Max(0f, maxPos - 0.05f);
        }

        isMoving = true;
        Debug.Log("[Rail] StartMoving() ejecutado");
    }

    public void StopMoving()
    {
        isMoving = false;
        Debug.Log("[Rail] StopMoving() ejecutado");
    }

    public void SetSpeed(float s) => speed = s;
}
