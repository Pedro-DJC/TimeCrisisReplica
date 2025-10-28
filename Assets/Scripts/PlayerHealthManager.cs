using UnityEngine;
using DG.Tweening;

public class PlayerHealthManager : MonoBehaviour
{
    public static PlayerHealthManager Instance;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DamageTaken()
    {
        playerHP--;
        Debug.Log("Player took damage! Current HP: " + playerHP);
    }
}
