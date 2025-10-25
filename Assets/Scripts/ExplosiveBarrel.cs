using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    [Header("Materials")]
    public Material normalMat;
    public Material shaderMat;

    public float destroyDelay = 2f;

    private MeshRenderer rend;
    private bool hasExploded = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rend = GetComponent<MeshRenderer>();
        if (normalMat != null)
            rend.material = normalMat;
    }

    public void OnHit()
    {
        if (hasExploded) return;

        hasExploded = true;

        if (shaderMat != null)
            rend.material = shaderMat;

        StartCoroutine(DestroyAfterDelay());
    }

    private System.Collections.IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
}
