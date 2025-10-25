using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class GunScript : MonoBehaviour
{
    public Transform gunPos;
    public Camera mainCamera;

    public float maxDistance = 1000f;
    public float fireRate = 0.3f;
    public AudioSource shootAudio;
    public TMP_Text ammoText;

    public int maxAmmo = 6;
    public int currentAmmo;

    private float FireTime = 0;
    [HideInInspector] public bool isReloading = false;
    public PlayerCover playerCover;

    void Start()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoUI();
    }

    void Update()
    {
        if (isReloading || (playerCover != null && playerCover.isCovering))
            return;

        UpdateAmmoUI();

        if (Input.GetMouseButtonDown(0) && Time.time >= FireTime)
        {
            if (currentAmmo <= 0)
            {
                Debug.Log("Reload");
                return;
            }

            FireTime = Time.time + fireRate;
            currentAmmo--;

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
                if (shootAudio != null)
                    shootAudio.Play();

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
                if (shootAudio != null)
                    shootAudio.Play();
                Debug.Log("No hit");
            }
        }
    }
    public void Reload()
    {
        if (!isReloading)
            StartCoroutine(ReloadCoroutine());
    }

    private System.Collections.IEnumerator ReloadCoroutine()
    {
        isReloading = true;

        Debug.Log(" Reloading...");

        yield return new WaitForSeconds(1f);

        currentAmmo = maxAmmo;
        isReloading = false;
        UpdateAmmoUI();

        Debug.Log("Reload complete");
    }
    private void UpdateAmmoUI()
    {
        if (ammoText != null)
            ammoText.text = currentAmmo.ToString();
    }
}
