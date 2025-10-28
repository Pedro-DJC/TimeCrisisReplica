using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    // Variables desde el inspector
    public Transform[] patrolPoints;
    public int targetPoint = 0;
    public float waitTime = 2f; // segundos de espera en cada punto
    [SerializeField] public int enemyType;
    public NavMeshAgent agent;

    // Variables internas
    public bool enemyCover = false;
    public bool enemyAlive = true;
    int warningShots = 0;

    void Start()
    {
        agent.SetDestination(patrolPoints[targetPoint].position);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enemy entered trigger with " + other.gameObject.name);
        if (other.CompareTag("PatrolPoint") && enemyAlive)
        {
            switch (enemyType)
            {
                case 0: // Enemigo que llega y dispara, sin ocultarse
                    StartCoroutine(ShootingLoop(0));
                    break;

                /*case 1: // Enemigo que llega, se oculta y dispara
                    StartCoroutine(WaitAndMoveNext());
                    break;*/

                case 2: // Enemigo con escudo
                    StartCoroutine(ShootingLoop(2));
                    break;

                default:
                    break;
            }
        }
    }

    IEnumerator ShootingLoop(int Type)
    {
        switch (Type)
        {
            case 0:
                ShootingNormal();
                yield return StartCoroutine(EnemyReload(Type)); // espera la recarga antes de continuar
                break;

            case 2:
                ShootingNormal();
                yield return new WaitForSeconds(2f);
                StartCoroutine(EnemyShield());
                yield return StartCoroutine(EnemyReload(Type)); // espera la recarga antes de continuar
                break;


            default:
                break;
        }
    }

    public void ShootingNormal()
    {
        if (warningShots < 5)
        {
            Debug.Log("Enemy is shooting, but misses");
            warningShots++;
        }

        if (warningShots == 5)
        {
            Debug.Log("Enemy is shooting at point " + targetPoint);
            PlayerHealthManager.Instance.DamageTaken();
            warningShots = 0;
        }
    }

    IEnumerator EnemyReload(int Type)
    {
        Debug.Log("Enemy is reloading...");

        yield return new WaitForSeconds(5f); // Tiempo de recarga

        if (enemyAlive)
        {
            StartCoroutine(ShootingLoop(Type));
        }
        else
        {
            Debug.Log("Enemy is dead, stopping reload.");
        }
    }

    IEnumerator EnemyShield()
    {
        Debug.Log("Enemy is raising shield...");
        enemyCover = true;
        yield return new WaitForSeconds(3f); // Tiempo con escudo
        Debug.Log("Enemy is lowering shield...");
        enemyCover = false;
    }

    IEnumerator WaitAndMoveNext()
    {
        yield return new WaitForSeconds(waitTime); // Espera unos segundos

        // Cambiar al siguiente punto
        targetPoint++;
        if (targetPoint >= patrolPoints.Length)
        {
            targetPoint = 0;
        }

        if (targetPoint != 0)
        {
            //Shooting();
        }
    }
}