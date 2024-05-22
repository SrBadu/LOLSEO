using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BaseBoss : MonoBehaviour
{
    public Slider baseSlider; // Referencia al slider de la vida de la base
    public int maxVidaBase = 100; // Vida máxima de la base
    public float dañoPorSegundoBase = 1f; // Daño por segundo a la base por cada jugador presente
    public GameObject prefabToSpawn; // Prefab a spawnear
    public Transform spawnPoint; // Punto de spawn del prefab
    public float spawnInterval = 30f; // Intervalo de tiempo entre spawns

    private int vidaBaseActual; // Vida actual de la base
    private List<GameObject> jugadoresEnBase = new List<GameObject>(); // Lista de jugadores dentro de la base

    void Start()
    {
        vidaBaseActual = maxVidaBase;
        ActualizarSliderVidaBase();
        StartCoroutine(SpawnUnitsRoutine()); // Iniciar la corrutina de generación de unidades
    }

    void ActualizarSliderVidaBase()
    {
        baseSlider.maxValue = maxVidaBase;
        baseSlider.value = vidaBaseActual;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadoresEnBase.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadoresEnBase.Remove(other.gameObject);
        }
    }

    public void ReducirVidaBase(int cantidad)
    {
        vidaBaseActual -= cantidad;
        vidaBaseActual = Mathf.Clamp(vidaBaseActual, 0, maxVidaBase); // Asegurar que la vida no sea menor que 0 ni mayor que maxVidaBase
        ActualizarSliderVidaBase();

        if (vidaBaseActual <= 0)
        {
            // La base ha sido destruida, el jugador gana
            SceneManager.LoadScene(2);
            Debug.Log("¡El jugador gana!");
            
        }
    }

    void SpawnPrefab()
    {
        Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);
    }

    IEnumerator SpawnUnitsRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnPrefab();
        }
    }
}
