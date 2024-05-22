using UnityEngine;
using UnityEngine.UI;

public class ContadorTropas : MonoBehaviour
{
    public Text textoUI; // Referencia al componente de texto UI
    public int limiteTropas = 10; // Límite máximo de tropas

    private void Update()
    {
        // Llamar al método para actualizar el recuento de tropas
        ActualizarRecuentoTropas();
    }

    private void ActualizarRecuentoTropas()
    {
        // Buscar todas las unidades seleccionables en la escena
        SelectableUnit[] unidades = FindObjectsOfType<SelectableUnit>();
        AnimWorker[] trabajadores = FindObjectsOfType<AnimWorker>();

        // Contar el número de unidades del jugador
        int cantidadUnidadesPlayer = 0;

        foreach (SelectableUnit unidad in unidades)
        {
            cantidadUnidadesPlayer++;
        }

        foreach (AnimWorker trabajador in trabajadores)
        {
            cantidadUnidadesPlayer++;
        }

        // Actualizar el texto del componente de texto UI con el recuento de tropas del jugador sobre el límite
        if (textoUI != null)
        {
            textoUI.text = "Tropas del jugador: " + cantidadUnidadesPlayer + "/" + limiteTropas;
        }
        else
        {
            Debug.LogWarning("El componente de texto UI no está asignado en el Inspector.");
        }
    }

    public int GetPlayerTroopCount()
    {
        // Buscar todas las unidades seleccionables en la escena
        SelectableUnit[] unidades = FindObjectsOfType<SelectableUnit>();
        AnimWorker[] trabajadores = FindObjectsOfType<AnimWorker>();

        // Contar el número de unidades del jugador
        int cantidadUnidadesPlayer = 0;

        foreach (SelectableUnit unidad in unidades)
        {
            cantidadUnidadesPlayer++;
        }

        foreach (AnimWorker trabajador in trabajadores)
        {
            cantidadUnidadesPlayer++;
        }

        return cantidadUnidadesPlayer;
    }
}
