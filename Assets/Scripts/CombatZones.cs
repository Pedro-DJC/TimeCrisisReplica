using UnityEngine;

public class CombatZones : MonoBehaviour
{
    public RailMovementController railController;
    public bool autoAdvanceOnClear = true;
    public float autoAdvanceDelay = 3f;

    private bool playerInZone = false;
    private bool cleared = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !playerInZone)
        {
            playerInZone = true;
            Debug.Log("[CombatZone] Jugador entró a la zona: DETENIENDO carrito");

            if (railController != null)
                railController.StopMoving();

            if (autoAdvanceOnClear)
                Invoke(nameof(AutoAdvance), autoAdvanceDelay);
        }
    }

    void AutoAdvance()
    {
        if (railController != null)
        {
            Debug.Log("[CombatZone] Zona despejada (simulada), reanudando movimiento...");
            railController.StartMoving();
            cleared = true;
        }
    }

    public void ClearZone()
    {
        if (cleared) return;
        cleared = true;

        if (railController != null)
            railController.StartMoving();
    }
}
