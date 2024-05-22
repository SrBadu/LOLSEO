using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int maxHealth = 100;          // Vida máxima del jugador
    public int attackDamage = 20;        // Daño de ataque
    private int currentHealth;
    private bool isInContactWithEnemy = false;
    private bool isInContactWithBase = false; // Nuevo: indica si el jugador está en contacto con la base
    private float lastAttackTime;
    private Animator anima;
    private bool collisionBase;
    private float lastBaseAttackTime;
    public float baseAttackCooldown = 2f;

    public Transform modelTransform; // Transform del modelo del jugador
    public Slider healthSlider; // Referencia al slider de vida

    void Start()
    {
        currentHealth = maxHealth;
        lastAttackTime = -3f; // Inicializamos para que el primer ataque se pueda realizar de inmediato
        //Debug.Log("Jugador inicializado con vida: " + currentHealth);
        anima = GetComponent<Animator>();

        // Configura el slider de vida
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth; // Configura el valor máximo del slider
            healthSlider.value = currentHealth; // Configura el valor inicial del slider
        }
        else
        {
            Debug.LogError("No se ha asignado el Slider de vida en el Inspector.");
        }
    }

    void Update()
    {
        if (collisionBase || isInContactWithBase)
        {
            if (Time.time >= lastBaseAttackTime + baseAttackCooldown) // Nuevo: verifica si se ha cumplido el tiempo de enfriamiento
            {
                AttackBase(); // Nuevo: si está en contacto con la base y el cooldown ha terminado, ataca la base
                lastBaseAttackTime = Time.time; // Nuevo: actualiza el tiempo del último ataque
            }
            anima.SetFloat("mov", 1f);
        }
        else if (isInContactWithEnemy && Time.time >= lastAttackTime + 3f)
        {
            lastAttackTime = Time.time;
            AttackEnemy();
        }
        
    }

    public void TakeDamage1(int damage)
    {
        currentHealth -= damage;
        //Debug.Log("Jugador recibió daño: " + damage + ". Vida restante: " + currentHealth);

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth; // Actualiza el valor del slider
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void AttackEnemy()
    {
        anima.SetFloat("mov", 1f);
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 2f); // Rango de contacto
        foreach (var hitCollider in hitColliders)
        {
            Enemy enemy = hitCollider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(attackDamage);
                //Debug.Log("Jugador infligió daño: " + attackDamage + " al enemigo.");
            }
        }
    }

    void AttackBase()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1.5f); // Rango de contacto
        foreach (var hitCollider in hitColliders)
        {
            BaseBoss baseBoss = hitCollider.GetComponent<BaseBoss>();
            if (baseBoss != null)
            {
                baseBoss.ReducirVidaBase(attackDamage);
            }
        }
    }

    void Die()
    {
        //Debug.Log("Jugador ha muerto");
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            isInContactWithEnemy = true;
            //Debug.Log("Jugador en contacto con enemigo");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            isInContactWithEnemy = false;
            //Debug.Log("Jugador salió de contacto con enemigo");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BaseBoss"))
        {
            isInContactWithBase = true; // Nuevo: el jugador está en contacto con la base
            //Debug.Log("Jugador en contacto con base");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BaseBoss"))
        {
            isInContactWithBase = false; // Nuevo: el jugador ya no está en contacto con la base
            //Debug.Log("Jugador salió de contacto con base");
        }
    }

    // Función para rotar hacia el objetivo
    void RotateTowards(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        modelTransform.rotation = Quaternion.Slerp(modelTransform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    
}
