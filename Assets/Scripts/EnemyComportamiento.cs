using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent navAgent;
    private Animator anima;
    public Slider healthSlider;
    public Transform baseTarget;
    public BaseCentro baseScript;

    private GameObject player;
    private GameObject nearestAlly;
    private bool isContactWithPlayer;
    private bool attackBase = false;
    private int currentHealth;

    public float detectionRange = 10f;
    public float attackRange = 1f;
    public float speed = 5f;
    public int maxHealth = 100;
    public int attackDamage = 10;
    public float attackCooldown = 3f;

    private float lastAttackTime;
    private float lastAllyAttackTime;

    public Transform destino;
    private float lastBaseAttackTime;
    public float baseAttackCooldown = 2f;

    public float maxAttackBaseDelay = 30f;

    void Start()
    {
        currentHealth = maxHealth;
        lastAttackTime = -attackCooldown;
        lastAllyAttackTime = -attackCooldown;
        anima = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.speed = speed;
        player = GameObject.FindGameObjectWithTag("Player");
        baseScript = GetComponent<BaseCentro>();
        // Inicializar baseScript a partir del baseTarget
        if (baseTarget != null)
        {
            baseScript = baseTarget.GetComponent<BaseCentro>();
        }

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
        else
        {
            Debug.LogError("No se ha asignado el Slider de vida en el Inspector.");
        }

            StartCoroutine(StartAttackBaseAfterDelay(maxAttackBaseDelay));

    }

    void Update()
    {
       
        if (attackBase)
        {
            FindNearestAlly();

            if (nearestAlly != null)
            {
                float allyDistance = Vector3.Distance(transform.position, nearestAlly.transform.position);

                if (allyDistance <= attackRange && Time.time >= lastAllyAttackTime + attackCooldown)
                {
                    lastAllyAttackTime = Time.time;
                    AttackAlly();
                }
                else
                {
                    MoveToAlly();
                }
            }
            else
            {
                MoveToBase();
            }
        }
        else
        {
            FindPlayer();

            if (player != null)
            {
                float distance = Vector3.Distance(transform.position, player.transform.position);

                if (isContactWithPlayer && Time.time >= lastAttackTime + attackCooldown)
                {
                    lastAttackTime = Time.time;
                    Attack();
                }
                else if (distance < detectionRange)
                {
                    ChaseTarget();
                }
            }
            else
            {
                MoveToBase();
            }
        }
        if (!AnyPlayersLeft())
        {
            MoveToAlternativeDestination(); // Método para moverse a otro destino
            if (Time.time >= lastBaseAttackTime + baseAttackCooldown) // Nuevo: verifica si se ha cumplido el tiempo de enfriamiento
            {
                AttackBase(); // Nuevo: si está en contacto con la base y el cooldown ha terminado, ataca la base
                lastBaseAttackTime = Time.time; // Nuevo: actualiza el tiempo del último ataque
            }
        }
    }

    void FindPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void FindNearestAlly()
    {
        GameObject[] allies = GameObject.FindGameObjectsWithTag("Player");
        float nearestDistance = Mathf.Infinity;
        nearestAlly = null;

        foreach (GameObject ally in allies)
        {
            float distance = Vector3.Distance(transform.position, ally.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestAlly = ally;
            }
        }
    }

    void ChaseTarget()
    {
        anima.SetBool("IsWalking", true);
        navAgent.SetDestination(player.transform.position);
    }

    void MoveToAlly()
    {
        if (nearestAlly != null)
        {
            anima.SetBool("IsWalking", true);
            navAgent.SetDestination(nearestAlly.transform.position);
        }
    }

    void MoveToBase()
    {
        anima.SetBool("IsWalking", true);
        navAgent.SetDestination(baseTarget.position);

        float distance = Vector3.Distance(transform.position, baseTarget.position);

    }

    void Attack()
    {
        anima.SetBool("IsWalking", false);
        anima.SetTrigger("Attack");
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1.5f);
        foreach (var hitCollider in hitColliders)
        {
            Player player = hitCollider.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage1(attackDamage);
            }
        }
    }

    void AttackAlly()
    {
        anima.SetBool("IsWalking", false);
        anima.SetTrigger("Attack");
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1.5f);
        foreach (var hitCollider in hitColliders)
        {
            Player ally = hitCollider.GetComponent<Player>();
            if (ally != null)
            {
                ally.TakeDamage1(attackDamage);
            }
        }
    }

    void AttackBase()
    {
        anima.SetTrigger("Attack");
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1.5f); // Rango de contacto
        foreach (var hitCollider in hitColliders)
        {
            BaseCentro baseCentro = hitCollider.GetComponent<BaseCentro>();
            if (baseCentro != null)
            {
                baseCentro.ReducirVidaBase(attackDamage);
            }
        }
    }



    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isContactWithPlayer = true;
            Debug.Log("Jugador en contacto con enemigo");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isContactWithPlayer = false;
            Debug.Log("Jugador salió de contacto con enemigo");
        }
    }

    private IEnumerator StartAttackBaseAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        attackBase = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isContactWithPlayer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isContactWithPlayer = false;
        }
    }

    void MoveToAlternativeDestination()
    {

        anima.SetBool("IsWalking", true);
        navAgent.SetDestination(destino.position); // Asigna el destino alternativo al NavMeshAgent
    }

    bool AnyPlayersLeft()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        return players.Length > 0;
    }
}
