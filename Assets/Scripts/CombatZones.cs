using UnityEngine;

public class CombatZones : MonoBehaviour
{
    public RailMovementController railMovement;
    public bool zoneCleared = false;

    bool playerInside = false;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log("[CombatZone] Jugador entró a la zona: DETENIENDO carrito");

        playerInside = true;

        if (railMovement != null)
            railMovement.StopMoving();
    }

    void Update()
    {
        if (playerInside && zoneCleared)
        {
            ZoneCleared();
        }
    }

    public void ZoneCleared()
    {
        if (railMovement != null)
        {
            Debug.Log("[CombatZone] Zona despejada: reanudando movimiento");
            railMovement.StartMoving();
        }

        playerInside = false;
        zoneCleared = false;
    }
}
