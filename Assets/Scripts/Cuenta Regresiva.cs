using UnityEngine;
using UnityEngine.UI;

public class CuentaRegresiva : MonoBehaviour
{
    public Text countdownText; // Referencia al componente Text
    public Slider countdownSlider; // Referencia al componente Slider
    private float timeRemaining = 60f; // Tiempo inicial de la cuenta regresiva en segundos

    void Start()
    {
        if (countdownText == null)
        {
            Debug.LogError("No se ha asignado el componente Text en el Inspector.");
        }
        if (countdownSlider == null)
        {
            Debug.LogError("No se ha asignado el componente Slider en el Inspector.");
        }
        else
        {
            countdownSlider.maxValue = timeRemaining; // Configurar el valor máximo del slider
            countdownSlider.value = timeRemaining; // Configurar el valor inicial del slider
        }
    }

    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            countdownText.text = "El enemigo Atacara en: " + Mathf.Ceil(timeRemaining).ToString() + " s";
            countdownSlider.value = timeRemaining; // Actualizar el valor del slider
        }
        else
        {
            Destroy(countdownText.gameObject); // Eliminar el componente Text
            Destroy(countdownSlider.gameObject); // Eliminar el componente Slider
        }
    }
}
