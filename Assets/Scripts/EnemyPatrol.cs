using System.Collections;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    PlayerCover PlayerCover;

    // Variables desde el inspector
    public Transform[] patrolPoints;
    public int targetPoint = 0;
    public float waitTime = 4f; // segundos de espera en cada punto
    [SerializeField] public int enemyType;
    public CombatZones combatZone;
    public NavMeshAgent agent;
    public TrialAnimation animationScript;
    public Transform player;

    Rigidbody rb;

    // Variables internas
    public bool enemyCover = false;

    public bool enemyAlive = true;
    public int health = 100;

    int warningShots = 0;
    private int currentPoint = 0;

    private bool isBusy = false; // Indica si el enemigo está en una secuencia especial

    [Header("Zona asignada")]
    public CombatZones currentZone;

    private void OnEnable()
    {
        enemyAlive = true;
        health = 100;

        if (agent != null && patrolPoints.Length > 0)
            agent.SetDestination(patrolPoints[currentPoint].position);
    }

    void Start()
    {
        animationScript = GetComponent<TrialAnimation>();
        if (agent != null && patrolPoints.Length > 0)
        {
            agent.SetDestination(patrolPoints[currentPoint].position);
        }
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (!enemyAlive || agent == null || patrolPoints.Length == 0) return;

        // Mientras está en una secuencia (bazooka, etc.), no patrullar
        if (isBusy) return;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentPoint = (currentPoint + 1) % patrolPoints.Length;
            agent.SetDestination(patrolPoints[currentPoint].position);
        }
        transform.LookAt(player);
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
                    Debug.Log("Type 3 Secuence");
                    StartCoroutine(BazookaShoot());

                    break;

                default:
                    break;
            }
        }
    }

    IEnumerator BazookaShoot()
    {
        isBusy = true;
        agent.isStopped = true;   // se queda quieto

        Debug.Log("Enemy is preparing to shoot bazooka...");
        yield return new WaitForSeconds(2f);

        Debug.Log("Enemy fires bazooka at player!");
        yield return new WaitForSeconds(1f);
        PlayerHealthManager.Instance.DamageTaken();

        // Aquí decides cuándo lo dejas moverse otra vez
        agent.isStopped = false;
        isBusy = false;
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
                yield return new WaitForSeconds(10f);
                break;

            default:
                break;
        }
    }

    public void ShootingNormal()
    {
        animationScript.Shoot();
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
        animationScript.CoverShield();
        yield return new WaitForSeconds(3f); // Tiempo con escudo
        Debug.Log("Enemy is lowering shield...");
        enemyCover = false;
        animationScript.UncoverShield();
    }

    IEnumerator WaitAndMoveNext(int type)
    {
        if (targetPoint == 0)
        {
            animationScript.Stand();
            Debug.Log("Waiting");
            yield return new WaitForSeconds(waitTime); // Espera unos segundos

            // Cambiar al siguiente punto
            Debug.Log("Moving");
            targetPoint++;
            agent.SetDestination(patrolPoints[targetPoint].position);
            animationScript.Run();

            yield return new WaitForSeconds(waitTime);

            yield return ShootingLoop(type);
        }
        else
        {
            Debug.Log("Waiting");
            yield return new WaitForSeconds(1f); // Espera unos segundos

            yield return ShootingLoop(0);
        }
    }
    public void TakeDamage(int amount)
    {
        if (!enemyAlive) return;

        health -= amount;
        if (health <= 0)
        {
            KillEnemy();
        }
    }
    public void KillEnemy()
    {
        animationScript.Dead();

        if (!enemyAlive) return;
        enemyAlive = false;
        Debug.Log($"[EnemyPatrol] {name} murió.");

        if (currentZone != null)
        {
            Debug.Log($"[EnemyPatrol] Notificando muerte a zona: {currentZone.name}");
            currentZone.NotifyEnemyKilled(gameObject);
        }
        else
        {
            Debug.LogWarning($"[EnemyPatrol] {name} no tiene zona asignada.");
        }
        gameObject.SetActive(false);
    }
}