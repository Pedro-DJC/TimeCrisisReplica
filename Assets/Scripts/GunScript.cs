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

    private float FireTime = 0f;
    private bool fireRequested = false;
    [HideInInspector] public bool isReloading = false;
    public PlayerCover playerCover;
    public PlayerHealthManager playerHealthManager;

    // debug
    private Vector3 lastShotOrigin;
    private Vector3 lastShotDirection;
    private Vector3 lastHitPoint;
    private bool hitSomething;

    void Start()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoUI();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            fireRequested = true;
        }
    }

    void LateUpdate()
    {
        if (fireRequested)
        {
            fireRequested = false;
            TryFire();
        }
    }

    void TryFire()
    {
        if (isReloading || (playerCover != null && playerCover.isCovering))
            return;

        if (Time.time < FireTime)
            return;

        FireTime = Time.time + fireRate;

        if (currentAmmo <= 0)
        {
            Debug.Log("Reload");
            return;
        }
        currentAmmo--;
        UpdateAmmoUI();

        Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        Vector3 targetPoint;
        if (Physics.Raycast(cameraRay, out RaycastHit camHit, maxDistance))
            targetPoint = camHit.point;
        else
            targetPoint = cameraRay.origin + cameraRay.direction * maxDistance;

        Vector3 origin = gunPos.position;
        Vector3 shootDirection = (targetPoint - origin).normalized;

        float originBias = 0.05f;
        Vector3 biasedOrigin = origin + shootDirection * originBias;

        lastShotOrigin = biasedOrigin;
        lastShotDirection = shootDirection;

        Debug.Log($"[Gun] origin={origin} biasedOrigin={biasedOrigin} cameraPos={mainCamera.transform.position} targetPoint={targetPoint}");

        if (Physics.Raycast(biasedOrigin, shootDirection, out RaycastHit hitInfo, maxDistance))
        {
            hitSomething = true;
            lastHitPoint = hitInfo.point;

            if (shootAudio != null) shootAudio.Play();
            Debug.DrawLine(biasedOrigin, hitInfo.point, Color.red, 1f);
            Debug.Log($"[Gun] Hit {hitInfo.collider.name} (tag:{hitInfo.collider.tag})");

            EnemyPatrol enemy = hitInfo.collider.GetComponent<EnemyPatrol>()
                                ?? hitInfo.collider.GetComponentInParent<EnemyPatrol>()
                                ?? hitInfo.collider.GetComponentInChildren<EnemyPatrol>();

            if (enemy != null && enemy.enemyAlive)
            {
                enemy.KillEnemy();
            }
            else if (hitInfo.collider.CompareTag("Barrel"))
            {
                ExplosiveBarrel barrel = hitInfo.collider.GetComponent<ExplosiveBarrel>();
                if (barrel != null) barrel.OnHit();
            }
            else
            {
                // otros impactos
            }
        }
        else
        {
            hitSomething = false;
            if (shootAudio != null) shootAudio.Play();
            Debug.DrawRay(biasedOrigin, shootDirection * maxDistance, Color.yellow, 1f);
            Debug.Log("[Gun] No hit");
        }
    }

    public void Reload()
    {
        if (!isReloading) StartCoroutine(ReloadCoroutine());
    }

    private System.Collections.IEnumerator ReloadCoroutine()
    {
        isReloading = true;
        Debug.Log("Reloading...");
        yield return new WaitForSeconds(1f);
        currentAmmo = maxAmmo;
        isReloading = false;
        UpdateAmmoUI();
        Debug.Log("Reload complete");
    }

    private void UpdateAmmoUI()
    {
        if (ammoText != null) ammoText.text = currentAmmo.ToString();
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(lastShotOrigin, 0.05f);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(lastShotOrigin, lastShotDirection * 10f);

        if (hitSomething)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(lastHitPoint, 0.1f);
        }
    }
}
