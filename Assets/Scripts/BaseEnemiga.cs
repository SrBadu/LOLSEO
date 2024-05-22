using UnityEngine;
using UnityEngine.UI;

public class BaseEnemiga : MonoBehaviour
{
    public int diplomacia; // 1 enemigo, 2 aliado
    public Text textoTropas;

    public Slider vidaBaseSlider;
    public int vidaMaxima;
    public int tropasEnemigas;
    public GameObject tropaEnemigaPrefab;
    public GameObject SpawnTropasBase;
    public float spawnRadius = 1f; // Radio de spawn
    public GameObject bandera1;
    public GameObject bandera2;

    public int tropasAliadas;

    private int tropasInstanciadas; // Contador de tropas enemigas instanciadas

    private void Start()
    {
        vidaMaxima = tropasEnemigas;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && diplomacia == 1)
        {
            vidaMaxima--;
            tropasAliadas++;
        }
        if (other.CompareTag("Player") && diplomacia == 2)
        {
            tropasAliadas++;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && diplomacia == 1)
        {
            tropasAliadas--;
        }
        if (other.CompareTag("Player") && diplomacia == 2)
        {
            tropasAliadas--;
        }
    }

    private void Update()
    {
        if(diplomacia == 2)
        {
            vidaMaxima = tropasAliadas;
            bandera1.SetActive(true);
            bandera2.SetActive(false);
        }
        if (diplomacia == 1)
        {
            bandera1.SetActive(false);
            bandera2.SetActive(true);
        }

        vidaBaseSlider.value = vidaMaxima;
        if (diplomacia == 1)
        {
            textoTropas.text = tropasEnemigas.ToString();
        }
        if (diplomacia == 2)
        {
            textoTropas.text = tropasAliadas.ToString();
        }

        // Si hay más tropas aliadas que enemigas, instanciar tropas enemigas
        if (tropasAliadas > tropasEnemigas)
        {
            // Instanciar el número de tropas enemigas igual a tropasEnemigas
            for (int i = 0; i < tropasEnemigas - tropasInstanciadas; i++)
            {
                // Generar una posición aleatoria dentro del radio de spawn
                Vector3 spawnPosition = SpawnTropasBase.transform.position + Random.insideUnitSphere * spawnRadius;
                // Instanciar la tropa enemiga en la posición aleatoria
                Instantiate(tropaEnemigaPrefab, spawnPosition, Quaternion.identity);
                tropasInstanciadas++; // Incrementar el contador de tropas instanciadas
            }
        }

        if (vidaMaxima < 0)
        {
            diplomacia = 2;
            vidaMaxima = tropasAliadas;
        }
    }
}
