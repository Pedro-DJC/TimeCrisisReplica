using System.Collections.Generic;
using UnityEngine;

public class CombatZones : MonoBehaviour
{
    public List<GameObject> enemies = new List<GameObject>();

    public RailMovementController railController;

    public bool autoAdvanceOnClear = true;

    private bool zoneCleared = false;
    private bool playerInside = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("[CombatZone] Jugador entró a la zona");
            playerInside = true;

            if (railController != null)
                railController.StopMoving();

            ActivateEnemies(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("[CombatZone] Jugador salió de la zona");
            playerInside = false;
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
                if (enemyPatrol != null && enemyPatrol.enemyAlive)
                    enemy.SetActive(true);
            }
            else
            {
                enemy.SetActive(false);
            }
        }
    }

    public void NotifyEnemyKilled(GameObject enemy)
    {
        enemies.RemoveAll(e => e == null);

        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
            Debug.Log("[CombatZone] Enemigo eliminado. Restantes: " + enemies.Count);
        }

        if (enemies.Count == 0 && !zoneCleared)
        {
            zoneCleared = true;
            Debug.Log("[CombatZone] Zona despejada");

            if (autoAdvanceOnClear && railController != null)
            {
                Debug.Log("[CombatZone] Avanzando...");
                railController.StartMoving();
            }
        }
    }
    public void ResetZone()
    {
        zoneCleared = false;
        playerInside = false;
    }
}
