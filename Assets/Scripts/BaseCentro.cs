using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BaseCentro : MonoBehaviour
{
    public Slider baseSlider; // Referencia al slider de la vida de la base
    public int maxVidaBase = 100; // Vida máxima de la base
    public int vidaBaseActual; // Vida actual de la base
    private List<GameObject> jugadoresEnBase = new List<GameObject>();

    void Start()
    {
        vidaBaseActual = maxVidaBase;
        ActualizarSliderVidaBase();
    }

    void ActualizarSliderVidaBase()
    {
        baseSlider.maxValue = maxVidaBase;
        baseSlider.value = vidaBaseActual;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            jugadoresEnBase.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
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
            Debug.Log("¡El enemigo gana!");
            SceneManager.LoadScene(3);
            // Aquí puedes agregar el código para cargar una escena de victoria del jugador
        }
    }
}
