using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;          // Arrastra aquí a tu Paladín
    public Vector3 offset = new Vector3(0, 2.5f, -4.0f); // Distancia desde atrás y arriba
    public float sensitivity = 3.0f;   // Sensibilidad del ratón
    public float smoothSpeed = 10f;    // Suavizado de la cámara

    private float mouseX, mouseY;

    void Start()
    {
        // Bloquea el cursor para que no se salga de la pantalla mientras juegas
        Cursor.lockState = CursorLockMode.Locked;
        
        if (target == null)
            Debug.LogWarning("¡Cámara: No has asignado un Target (personaje)!");
    }

    void LateUpdate() // LateUpdate es mejor para cámaras
    {
        if (target == null) return;

        // 1. Obtener movimiento del ratón
        mouseX += Input.GetAxis("Mouse X") * sensitivity;
        mouseY -= Input.GetAxis("Mouse Y") * sensitivity;
        mouseY = Mathf.Clamp(mouseY, -20f, 60f); // Limitamos para no dar la vuelta completa verticalmente

        // 2. Calcular rotación y posición
        Quaternion rotation = Quaternion.Euler(mouseY, mouseX, 0);
        Vector3 targetPosition = target.position + rotation * offset;

        // 3. Aplicar con suavizado
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        transform.LookAt(target.position + Vector3.up * 1.5f); // Mira un poco por encima de los pies
    }
}