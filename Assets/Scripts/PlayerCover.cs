using UnityEngine;

public class PlayerCover : MonoBehaviour
{
    [Header("Referencia al arma")]
    public GunScript gunScript;
    [Header("Tecla para cubrirse")]
    public KeyCode coverKey = KeyCode.Space;

    [Header("Altura al cubrirse")]
    public float coverY = -0.5f;

    [Header("Tiempo que tarda la recarga")]
    public float reloadTime = 1f;

    [HideInInspector] public bool isCovering = false;
    private float originalY;
    private float coverEndTime = 0f;

    void Start()
    {
        originalY = transform.localPosition.y;
    }

    void Update()
    {
        if (Input.GetKey(coverKey) && !isCovering)
        {
            StartCoroutine(CoverAndReload());
        }

        if (isCovering)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, coverY, transform.localPosition.z);
        }
        else
        {
            if (Time.time >= coverEndTime)
                transform.localPosition = new Vector3(transform.localPosition.x, originalY, transform.localPosition.z);
        }
    }

    private System.Collections.IEnumerator CoverAndReload()
    {
        isCovering = true;
        coverEndTime = Time.time + reloadTime;

        if (gunScript != null)
            gunScript.Reload();

        yield return new WaitForSeconds(reloadTime);

        while (Input.GetKey(coverKey))
        {
            yield return null;
        }

        isCovering = false;
    }
}
