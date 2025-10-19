using System.Collections;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform[] patrolPoints;
    public int targetPoint = 0;
    public float speed = 2f;
    public float waitTime = 2f; // segundos de espera en cada punto

    private bool waiting = false;

    void Update()
    {
        if (!waiting)
        {
            // Mover hacia el punto objetivo
            transform.position = Vector3.MoveTowards(
                transform.position,
                patrolPoints[targetPoint].position,
                speed * Time.deltaTime
            );

            // Si llegó al punto, iniciar espera
            if (Vector3.Distance(transform.position, patrolPoints[targetPoint].position) < 0.01f)
            {
                StartCoroutine(WaitAndMoveNext());
            }
        }
    }

    IEnumerator WaitAndMoveNext()
    {
        waiting = true;
        yield return new WaitForSeconds(waitTime); // Espera unos segundos

        // Cambiar al siguiente punto
        targetPoint++;
        if (targetPoint >= patrolPoints.Length)
        {
            targetPoint = 0;
        }

        waiting = false;
    }
}