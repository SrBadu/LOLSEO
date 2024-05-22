using UnityEngine;
using UnityEngine.UI;

public class ContadorRecursos : MonoBehaviour
{
    public int recursos = 100; // Variable entera para almacenar la cantidad de recursos
    public Text textoRecursos; // Referencia al Texto en UI que mostrará la cantidad de recursos
    private int recursosPorSegundo = 1; // Cantidad de recursos que se ganarán por segundo

    void Start()
    {
        //ActualizarTextoRecursos();
        // Llama a la función GanarRecursosPorSegundo cada segundo
        InvokeRepeating("GanarRecursosPorSegundo", 0f, 1f);
    }

    // Función para ganar recursos progresivamente por segundo
    private void GanarRecursosPorSegundo()
    {
        GanarRecursos(recursosPorSegundo);
    }

    // Función para ganar recursos
    public void GanarRecursos(int cantidad)
    {
        recursos += cantidad;
        ActualizarTextoRecursos();
        
    }

    // Función para quitar recursos
    public void GastarRecursos(int cantidad)
    {
        if (recursos >= cantidad)
        {
            recursos -= cantidad;
            ActualizarTextoRecursos();
            
        }
        else
        {
            Debug.Log("No tienes suficientes recursos para gastar " + cantidad + ".");
        }
    }

    // Función para actualizar el Texto en UI con la cantidad actual de recursos
    private void ActualizarTextoRecursos()
    {
        if (textoRecursos != null)
        {
            textoRecursos.text = recursos.ToString();
        }
    }

    // Función para establecer la cantidad de recursos ganados por segundo
    public void EstablecerRecursosPorSegundo(int cantidad)
    {
        recursosPorSegundo = cantidad;
    }
}
