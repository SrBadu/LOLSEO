using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpawnTrabajador : MonoBehaviour
{
    public GameObject textoTropasLimite;
    public GameObject troopPrefab; // Prefab de la tropa
    public Transform spawnAreaMin; // Punto mínimo del área de spawn (esquina inferior izquierda)
    public Transform spawnAreaMax; // Punto máximo del área de spawn (esquina superior derecha)
    public int cost = 50; // Costo de la tropa

    public Text resourceText; // Texto para mostrar el contador de recursos
    public Slider spawnCooldownSlider; // Slider para mostrar el tiempo de recarga
    public GameObject cooldownBar; // Objeto de la barra de cooldown
    public GameObject textotropaColdown; // Objeto de texto para mostrar el cooldown

    private ContadorRecursos resourceCounter; // Referencia al script ContadorRecursos
    private ContadorTropas troopCounter; // Referencia al script ContadorTropas
    private bool isCooldown; // Bandera para verificar si está en cooldown
    private float currentCooldown; // Tiempo actual del temporizador
    public float destroyDelay = 1f; // Tiempo que el texto estará visible

    private void Start()
    {
        textoTropasLimite.SetActive(false);
        cooldownBar.SetActive(false);
        textotropaColdown.SetActive(false);
        resourceCounter = FindObjectOfType<ContadorRecursos>(); // Encontrar el script ContadorRecursos en la escena
        troopCounter = FindObjectOfType<ContadorTropas>(); // Encontrar el script ContadorTropas en la escena
        UpdateResourceUI();

        // Configurar el cooldown inicial
        currentCooldown = spawnCooldownSlider.maxValue;
        spawnCooldownSlider.value = spawnCooldownSlider.maxValue;
    }

    private void Update()
    {
        // Reducir el tiempo de cooldown si está activo
        if (isCooldown)
        {
            currentCooldown -= Time.deltaTime;

            // Actualizar el slider para mostrar el tiempo restante
            spawnCooldownSlider.value = currentCooldown;

            // Si el cooldown llega a cero, desactivar la barra de cooldown
            if (currentCooldown <= 0)
            {
                isCooldown = false;
                cooldownBar.SetActive(false);
            }
        }
    }

    public void UpdateResourceUI()
    {
        resourceText.text = resourceCounter.recursos.ToString();
    }

    public void SpawnTroop()
    {
        // Verificar si está en cooldown
        if (isCooldown)
        {
            Debug.Log("La tropa aún está en cooldown.");
            StartCoroutine(ShowCooldownText());
            return;
        }

        // Verificar si hay suficientes recursos disponibles para crear la tropa
        if (resourceCounter.recursos < cost)
        {
            Debug.Log("No tienes suficientes recursos para crear esta tropa.");
            return;
        }

        // Verificar si el límite de tropas ha sido alcanzado
        if (troopCounter.GetPlayerTroopCount() >= troopCounter.limiteTropas)
        {
            textoTropasLimite.SetActive(true);
            return;
        }

        // Gastar los recursos necesarios
        resourceCounter.GastarRecursos(cost);

        // Actualizar el UI del contador de recursos
        UpdateResourceUI();

        // Instanciar la tropa en una posición aleatoria dentro del área de spawn
        Vector3 randomPosition = new Vector3(
            Random.Range(spawnAreaMin.position.x, spawnAreaMax.position.x),
            spawnAreaMin.position.y,
            Random.Range(spawnAreaMin.position.z, spawnAreaMax.position.z)
        );
        Instantiate(troopPrefab, randomPosition, Quaternion.identity);

        // Activar el cooldown
        currentCooldown = spawnCooldownSlider.maxValue;
        cooldownBar.SetActive(true);

        // Ahora, la tropa se ha instanciado, así que activamos el cooldown
        isCooldown = true;
    }

    public void OnButtonPressed()
    {
        // Llamar a la función de spawn al presionar el botón
        SpawnTroop();
    }

    private IEnumerator ShowCooldownText()
    {
        textotropaColdown.SetActive(true);
        yield return new WaitForSeconds(destroyDelay);
        textotropaColdown.SetActive(false);
    }
}
