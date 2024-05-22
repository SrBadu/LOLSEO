using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mina : MonoBehaviour
{
    public ContadorRecursos contador;

    // Variable para rastrear si el objeto está dentro de la colisión
    public  bool dentroDeColision = false;

    // Método llamado cuando otro Collider entra en la colisión de este objeto
    public void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Trabajador")){
            dentroDeColision = true; // El objeto está dentro de la colisión
            Debug.Log("Estamos chocando");
            StartCoroutine(GenerarRecursosDespuesDeDelay(3f)); // Comienza a generar recursos después de un retraso de 3 segundos
        }
    }

    // Método llamado cuando otro Collider sale de la colisión de este objeto
    public  void OnTriggerExit(Collider other) {
        if(other.CompareTag("Trabajador")){
            dentroDeColision = false; // El objeto ha salido de la colisión
        }
    }

    // Método para generar recursos después de un retraso
    IEnumerator GenerarRecursosDespuesDeDelay(float delay) {
        yield return new WaitForSeconds(delay);
        while (dentroDeColision) {
            contador.GanarRecursos(10); // Genera 10 recursos cada 3 segundos mientras el objeto esté dentro de la colisión
            yield return new WaitForSeconds(delay);
        }
    }
}
