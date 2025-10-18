using UnityEngine;

public class CamaraPosition : MonoBehaviour
{
    [Header("Player")]
    public Transform player;

    [Header("Offset Player")]
    public Vector3 offset = new Vector3(0, 1.5f, -3f);

    [Header("Smooth")]
    [Range(0, 1)] public float smoothSpeed = 0.125f;

    void LateUpdate()
    {
        if (player == null)
        {
            return;
        }

        Vector3 desiredPosition = player.position + player.rotation * offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        transform.rotation = Quaternion.Lerp(transform.rotation, player.rotation, smoothSpeed);
    }
}
