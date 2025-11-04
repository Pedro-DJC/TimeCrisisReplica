using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    PlayerCover PlayerCover;

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
                    Debug.Log("Type 0 Secuence");
                    StartCoroutine(ShootingLoop(0));
                    break;

                case 1: // Enemigo que llega, se oculta y dispara
                    Debug.Log("Type 1 Secuence");
                    StartCoroutine(ShootingLoop(1));
                    break;

                case 2: // Enemigo con escudo
                    StartCoroutine(ShootingLoop(2));
                    break;

                case 3: // Enemigo con Bazooka
                    StartCoroutine(BazookaShoot());

                    break;

                default:
                    break;
            }
        }
    }

    IEnumerator BazookaShoot()
    {
        Debug.Log("Enemy is preparing to shoot bazooka...");
        yield return new WaitForSeconds(2f); // Tiempo de preparación
        Debug.Log("Enemy fires bazooka at player!");
        yield return new WaitForSeconds(1f); // Tiempo de vuelo del bazooka

        /*if (PlayerCover.isCovering == true)
        {
            Debug.Log("Player is in cover, bazooka missed!");
        }
        else
        {
            PlayerHealthManager.Instance.DamageTaken(); // Asume que el bazooka siempre acierta
        }*/
        PlayerHealthManager.Instance.DamageTaken(); // Asume que el bazooka siempre acierta

        if (targetPoint == 0)
        {
            yield return StartCoroutine(WaitAndMoveNext(3)); // se mueve al siguiente punto después de disparar
        }
        else
        {
            yield return StartCoroutine(EnemyReload(3)); // espera la recarga antes de continuar
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

            case 1:
                //Debug.Log("Case 1 Started");
                StartCoroutine(WaitAndMoveNext(0));
                break;


            case 2:
                ShootingNormal();
                yield return new WaitForSeconds(2f);
                StartCoroutine(EnemyShield());
                yield return StartCoroutine(EnemyReload(Type)); // espera la recarga antes de continuar
                break;

            case 3:
                yield return StartCoroutine(BazookaShoot());
                break;

            default:
                break;
        }
    }

    public void ShootingNormal()
    {
        if (warningShots < 5)
        {
            Debug.Log("Enemy is shooting, but misses, shots: " + warningShots);
            warningShots++;
        }

        if (warningShots == 5)
        {
            Debug.Log("Enemy is shooting at player!");
            PlayerHealthManager.Instance.DamageTaken();
            warningShots = 0;
        }
    }

    IEnumerator EnemyReload(int Type)
    {
        Debug.Log("Enemy is reloading...");

        yield return new WaitForSeconds(3f); // Tiempo de recarga

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

    IEnumerator WaitAndMoveNext(int type)
    {
        if (targetPoint == 0)
        {
            Debug.Log("Waiting");
            yield return new WaitForSeconds(waitTime); // Espera unos segundos

            // Cambiar al siguiente punto
            Debug.Log("Moving");
            targetPoint++;
            agent.SetDestination(patrolPoints[targetPoint].position);

            yield return new WaitForSeconds(waitTime);

            /*if (targetPoint >= patrolPoints.Length)
            {
                Debug.Log("Patrol finished.");
            }

            if (targetPoint != 0)
            {
                //Shooting();
            }*/

            ShootingLoop(type);
        }
        else
        {
            Debug.Log("Waiting");
            yield return new WaitForSeconds(1f); // Espera unos segundos

            yield return ShootingLoop(0);
        }
    }
}