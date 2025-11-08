using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatZones : MonoBehaviour
{
    [Header("Enemigos de esta zona")]
    public List<GameObject> enemies = new List<GameObject>();

    [Header("Control del rail (carrito)")]
    public RailMovementController railController;

    [Header("Avanzar automáticamente al limpiar zona")]
    public bool autoAdvanceOnClear = true;

    public bool zoneCleared = false;
    private bool playerInside = false;

    private void Start()
    {
        ActivateEnemies(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"[CombatZone] Jugador entró a la zona {gameObject.name}");
            playerInside = true;

            if (railController != null)
                railController.StopMoving();

            ActivateEnemies(true);

            DisableOtherZonesEnemies();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"[CombatZone] Jugador salió de la zona {gameObject.name}");
            playerInside = false;

            ActivateEnemies(false);
        }
    }

    public void ActivateEnemies(bool state)
    {
        foreach (var enemy in enemies)
        {
            if (enemy == null) continue;

            var enemyPatrol = enemy.GetComponent<EnemyPatrol>();
            if (state)
            {
                if (enemyPatrol == null || enemyPatrol.enemyAlive)
                    enemy.SetActive(true);
            }
            else
            {
                enemy.SetActive(false);
            }
        }
    }

    private void DisableOtherZonesEnemies()
    {
        CombatZones[] allZones = Object.FindObjectsByType<CombatZones>(FindObjectsSortMode.None);

        foreach (var zone in allZones)
        {
            if (zone != this)
                zone.ActivateEnemies(false);
        }
    }

    public void NotifyEnemyKilled(GameObject enemy)
    {
        enemies.RemoveAll(e => e == null);

        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
            Debug.Log($"[CombatZone] Enemigo eliminado en {gameObject.name}. Restantes: {enemies.Count}");
        }

        if (playerInside && enemies.Count == 0 && !zoneCleared)
        {
            zoneCleared = true;
            Debug.Log($"[CombatZone] Zona {gameObject.name} despejada.");

            if (autoAdvanceOnClear && railController != null)
            {
                Debug.Log("[CombatZone] Avanzando al siguiente punto...");
                railController.StartMoving();
            }
        }
    }
    public void ResetZone()
    {
        zoneCleared = false;
        playerInside = false;
        ActivateEnemies(false);
    }
}
