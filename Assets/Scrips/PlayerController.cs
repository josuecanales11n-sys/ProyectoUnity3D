using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 3.0f;
    public float runSpeed = 6.0f;
    public float rotationSmoothTime = 0.1f; // Qué tan rápido gira el personaje
    public float gravity = -20f;

    private CharacterController controller;
    private Animator animator;
    private Vector3 playerVelocity;
    private float rotationVelocity;
    private Transform cam;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        
        // Buscamos la cámara principal
        if (Camera.main != null) 
            cam = Camera.main.transform;
    }

    void Update()
    {
        // 1. Obtener Input (WASD)
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            // 2. CALCULAR DIRECCIÓN RELATIVA A LA CÁMARA
            // Calculamos el ángulo hacia donde debe mirar basándose en la rotación Y de la cámara
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            
            // Girar el personaje suavemente hacia ese ángulo
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationVelocity, rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // Convertir el ángulo en una dirección de movimiento
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            
            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            float currentSpeed = isRunning ? runSpeed : walkSpeed;
            
            // MOVER
            controller.Move(moveDir.normalized * currentSpeed * Time.deltaTime);

            // Animación (Speed: 0.5 caminar, 1.0 correr)
            float animValue = isRunning ? 1.0f : 0.5f;
            animator.SetFloat("Speed", animValue, 0.05f, Time.deltaTime);
        }
        else
        {
            // Parar animación
            animator.SetFloat("Speed", 0f, 0.1f, Time.deltaTime);
        }

        // 3. GRAVEDAD
        if (controller.isGrounded && playerVelocity.y < 0) playerVelocity.y = -2f;
        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}