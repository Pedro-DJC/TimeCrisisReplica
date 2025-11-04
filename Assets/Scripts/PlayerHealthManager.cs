using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerHealthManager : MonoBehaviour
{
    public PostprocessManager postProcessManager;
    public static PlayerHealthManager Instance;

    public bool damaged;

    float playerHP = 3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Debug.Log("Player Health Manager Initialized with HP: " + playerHP);
        damaged = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DamageTaken()
    {
        playerHP--;
        Debug.Log("Player took damage! Current HP: " + playerHP);
        DamagedEffect();
    }

    private void DamagedEffect()
    {
        postProcessManager.DamageColorFilter(Color.white, new Color(2f, 0, 0), 0.05f);
    }
}
