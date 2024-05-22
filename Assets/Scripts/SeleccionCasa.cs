using UnityEngine;
using UnityEngine.EventSystems;

public class SeleccionCasa : MonoBehaviour
{
    public GameObject button; // El botón a activar/desactivar
    public GameObject button2;
    public LayerMask houseLayer; // Capa de las casas
    [SerializeField]
    private GameObject SelectionSprite;

    private void Start()
    {
        button.SetActive(false);
        button2.SetActive(false); // Asegúrate de que el botón esté desactivado al inicio
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Detecta clic izquierdo del ratón
        {
            // Verifica si el clic fue sobre un elemento de la UI
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return; // No hagas nada si el clic fue en la UI
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, houseLayer))
            {
                // Si se hace clic en una casa
                button.SetActive(true);
                button2.SetActive(true);
                SelectionSprite.gameObject.SetActive(true);
            }
            else
            {
                // Si se hace clic en cualquier otro lugar
                button.SetActive(false);
                button2.SetActive(false);
                SelectionSprite.gameObject.SetActive(false);
            }
        }
    }
}
