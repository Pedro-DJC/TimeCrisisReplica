using System.Collections.Generic;
using UnityEngine;

public class CombatZones : MonoBehaviour
{
    public RailMovementController railController;
    public bool autoAdvanceOnClear = true;
    public float checkDelay = 1.5f;

    public List<GameObject> enemies = new List<GameObject>();

    private bool playerInZone = false;
    public bool zoneCleared = false;

    void Start()
    {
        foreach (var enemy in enemies)
        {
            if (enemy != null)
                enemy.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !playerInZone)
        {
            playerInZone = true;
            Debug.Log("[CombatZone] Jugador entró a la zona: DETENIENDO carrito y activando enemigos");

            if (railController != null)
                railController.StopMoving();

            ActivateEnemies();
            InvokeRepeating(nameof(CheckEnemiesStatus), checkDelay, checkDelay);
        }
    }

    void ActivateEnemies()
    {
        foreach (var enemy in enemies)
        {
            if (enemy != null)
                enemy.SetActive(true);
        }
    }

    void CheckEnemiesStatus()
    {
        if (zoneCleared) return;
        bool allDead = true;
        foreach (var enemy in enemies)
        {
            if (enemy != null && enemy.activeInHierarchy)
            {
                allDead = false;
                break;
            }
        }

        if (allDead)
        {
            zoneCleared = true;
            Debug.Log("[CombatZone] Zona despejada. Avanzando al siguiente punto...");

            CancelInvoke(nameof(CheckEnemiesStatus));

            if (autoAdvanceOnClear && railController != null)
                railController.StartMoving();
        }
    }
}
