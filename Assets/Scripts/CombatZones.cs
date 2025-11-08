using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatZones : MonoBehaviour
{
    [Header("Control de zona")]
    public RailMovementController railController;
    public bool autoAdvanceOnClear = true;

    public bool zoneCleared = false;
    private bool playerInside = false;

    private List<EnemyPatrol> enemies = new List<EnemyPatrol>();

    private void Awake()
    {
        enemies = new List<EnemyPatrol>(GetComponentsInChildren<EnemyPatrol>(true));

        foreach (var enemy in enemies)
        {
            if (enemy == null) continue;
            enemy.gameObject.SetActive(false);
            enemy.currentZone = this;
        }

        Debug.Log($"[CombatZone] {name} detectó {enemies.Count} enemigos hijos.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log($"[CombatZone] Jugador entró a {name}");
        playerInside = true;

        if (railController != null)
        {
            railController.StopMoving();
            Debug.Log("[CombatZone] Carro detenido");
        }

        ActivateEnemies(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"[CombatZone] Jugador salió de {name}");
            playerInside = false;
        }
    }

    public void ActivateEnemies(bool state)
    {
        foreach (var enemy in enemies)
        {
            if (enemy == null) continue;
            enemy.gameObject.SetActive(state);
            if (state)
            {
                enemy.enemyAlive = true;
                enemy.currentZone = this;
            }
        }
    }

    public void NotifyEnemyKilled(GameObject enemyGO)
    {
        enemies.RemoveAll(e => e == null);

        var enemyToRemove = enemies.Find(e => e != null && e.gameObject == enemyGO);
        if (enemyToRemove != null)
        {
            enemies.Remove(enemyToRemove);
        }

        int aliveCount = 0;
        foreach (var e in enemies)
        {
            if (e != null && e.gameObject.activeSelf)
                aliveCount++;
        }

        Debug.Log($"[CombatZone] {enemyGO.name} eliminado. Restantes: {aliveCount}");

        if (aliveCount <= 0 && !zoneCleared)
        {
            zoneCleared = true;
            Debug.Log($"[CombatZone] Zona {name} despejada completamente, avanzando...");

            if (autoAdvanceOnClear && railController != null)
            {
                Debug.Log("[CombatZone] Llamando a StartMoving()");
                railController.StartMoving();
            }
        }
    }
}
