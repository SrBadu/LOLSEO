using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tuto : MonoBehaviour
{
    private float timeRemaining = 5f; // Tiempo inicial de la cuenta regresiva en segundos
    public GameObject tuto1; // Referencia al GameObject que será destruido

    // Start is called before the first frame update
    void Start()
    {
        if (tuto1 == null)
        {
            Debug.LogError("No se ha asignado el GameObject tuto1 en el Inspector.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime; // Decrementa el tiempo restante
        }
        else
        {
            if (tuto1 != null)
            {
                Destroy(tuto1); // Destruye el GameObject tuto1
                tuto1 = null; // Evita que se intente destruir nuevamente
            }
        }

        // Verificar si la tecla Enter (Return) está siendo presionada
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (tuto1 != null)
            {
                Destroy(tuto1); // Destruye el GameObject tuto1
                tuto1 = null; // Evita que se intente destruir nuevamente
            }
        }
    }
}
