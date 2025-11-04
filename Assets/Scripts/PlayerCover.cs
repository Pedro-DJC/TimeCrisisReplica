using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class PlayerCover : MonoBehaviour
{
    [HideInInspector] public bool isCovering = false;
    private KeyCode coverKey = KeyCode.Space;
    private float originalY;
    private float coverEndTime = 0f;
    public PostprocessManager postProcessManager;
    public GunScript gunScript;
    public float coverY;
    public float reloadTime = 1f;

    PlayerHealthManager healthManager;

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

        if (isCovering == true)
        {
            CrouchVignetteFx(true);
        }

        if (isCovering == false)
        {
            UncrouchVignette(true);
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
        Debug.Log(isCovering);
    }

    private void CrouchVignetteFx(bool crouching)
    {
        float vignetteIntensity = crouching ? 0.5f : 0.3f;
        postProcessManager. CrouchVignette(Color.black, vignetteIntensity, 0.1f, 0);
    }

    private void UncrouchVignette(bool standing)
    {
        float vignetteIntensity = standing ? 0.3f : 0.5f;
        postProcessManager.CrouchVignette(Color.black, vignetteIntensity, 0.1f, 0);
    }
}
