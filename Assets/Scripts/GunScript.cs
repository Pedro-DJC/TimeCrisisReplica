using UnityEngine;

public class GunScript : MonoBehaviour
{
    public Transform gunPos;
    public Camera mainCamera;

    public float maxDistance = 1000f;
    public float fireRate = 0.3f;

    private float FireTime = 0;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= FireTime)
        {
            FireTime = Time.time + fireRate;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            Vector3 targetPoint;
            if (Physics.Raycast(ray, out RaycastHit camHit, maxDistance))
            {
                targetPoint = camHit.point;
            }
            else
            {
                targetPoint = ray.origin + ray.direction * maxDistance;
            }

            Vector3 shootDirection = (targetPoint - gunPos.position).normalized;

            if (Physics.Raycast(gunPos.position, shootDirection, out RaycastHit hitInfo, maxDistance))
            {
                Debug.DrawLine(gunPos.position, hitInfo.point, Color.red, 1f);

                if (hitInfo.collider.CompareTag("Enemy"))
                {
                    Debug.Log("Enemy hit " + hitInfo.collider.name);
                    //Hitinfo para poder saber a que tipo de enemigo se le disparo
                }
                else if (hitInfo.collider.CompareTag("Box"))
                {
                    Debug.Log("Box hit");
                }
                else if (hitInfo.collider.CompareTag("Barrel"))
                {
                    Debug.Log("Barrel");
                }
            }
            else
            {
                Debug.DrawRay(gunPos.position, shootDirection * maxDistance, Color.yellow, 1f);
                Debug.Log("No hit");
            }
        }
    }
}
