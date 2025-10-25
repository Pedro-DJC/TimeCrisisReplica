using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    public Transform[] patrolPoints;
    public int targetPoint = 0;
    public float speed = 2f;
    public float waitTime = 2f; // segundos de espera en cada punto

    private bool waiting = false;

    public NavMeshAgent agent;

    void Update()
    {
        if (!waiting)
        {
            // Mover hacia el punto objetivo
            /*transform.position = Vector3.MoveTowards(
                transform.position,
                patrolPoints[targetPoint].position,
                speed * Time.deltaTime
            );*/
            
            agent.SetDestination(patrolPoints[targetPoint].position);
            //Debug.Log(Vector3.Distance(agent.transform.position, patrolPoints[targetPoint].position));

            // Si llegó al punto, iniciar espera
            if (Vector3.Distance(agent.transform.position, patrolPoints[targetPoint].position) < 0.5f)
            {
                StartCoroutine(WaitAndMoveNext());
            }
        }
    }

    public void Shooting()
    {
        Debug.Log("Enemy is shooting at point " + targetPoint);
        PlayerHealthManager.Instance.DamageTaken();
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

        if (targetPoint != 0)
        {
            Shooting();
        }

        waiting = false;
    }
}