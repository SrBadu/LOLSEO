using UnityEngine;

public class GimbalController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 2f;
    public float maxPosition = 10f;

    private void Update()
    {
        // Movimiento de la cámara con las flechas direccionales o WASD
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.Self);

        // Limitar la posición en todas las direcciones
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -maxPosition, maxPosition),
            Mathf.Clamp(transform.position.y, -maxPosition, maxPosition),
            Mathf.Clamp(transform.position.z, -maxPosition, maxPosition)
        );

        // Rotación de la cámara al presionar el scroll del ratón y mover el mouse
        if (Input.GetMouseButton(2)) // Botón del scroll del ratón
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // Rotación horizontal
            transform.Rotate(Vector3.up, mouseX * rotationSpeed, Space.World);

            // Rotación vertical
            transform.Rotate(Vector3.right, -mouseY * rotationSpeed, Space.Self);
        }
    }
}
