using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiControlador : MonoBehaviour
{
    public float walkSpeed = 10f;
    public float runSpeed = 20f;
    public float jumpForce = 5f;
    public float crouchHeight = 0.2f;
    public float mouseSensitivity = 600f;
    public Transform cameraTransform;

    private float originalHeight;
    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;  // Nuevo: Referencia al CapsuleCollider
    public GameObject PuntoPatalla;

    private float rotationX = 0f;  // Para controlar la rotación en el eje X (arriba/abajo)

    // Variables para agacharse de forma suave
    private Vector3 escalaNormal;
    private Vector3 escalaAgachado;
    public float tiempoAgachado = 0.1f;  // Duración del agachado
    private bool agachado = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();  // Inicializa el CapsuleCollider
        Cursor.lockState = CursorLockMode.Locked;  // Bloquea el cursor en el centro de la pantalla
        originalHeight = transform.localScale.y;  // Guarda la altura original del jugador

        // Inicializamos la rotación de la cámara asegurando que empieza mirando hacia el frente
        rotationX = 0f;  // Rotación vertical en 0
        cameraTransform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);  // Aseguramos que la cámara comience mirando hacia el frente

        // Guardamos las escalas para agacharse y estar de pie
        escalaNormal = transform.localScale;
        escalaAgachado = new Vector3(transform.localScale.x, crouchHeight, transform.localScale.z);

        // Activa el crosshair al inicio del juego
        if (PuntoPatalla != null)
        {
            PuntoPatalla.SetActive(true);  // Activa el crosshair
        }
    }


    void Update()
    {
        // Movimiento
        Move();
        Look();

        // Salto con la tecla "Espacio"
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            Jump();
        }

        // Agacharse o pararse
        if (Input.GetKey(KeyCode.C))
        {
            agachado = true;
        }
        else
        {
            agachado = false;
        }

        // Interpolación suave entre agacharse y estar de pie
        transform.localScale = Vector3.Lerp(transform.localScale, agachado ? escalaAgachado : escalaNormal, Time.deltaTime / tiempoAgachado);
    }

    void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.right * moveHorizontal + transform.forward * moveVertical;

        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        Vector3 velocity = moveDirection * speed;
        velocity.y = rb.velocity.y;  // Mantiene la velocidad vertical existente (gravedad)
        rb.velocity = velocity;
    }

    void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotar el jugador sobre el eje Y (horizontal, izquierda/derecha)
        transform.Rotate(Vector3.up * mouseX);

        // Rotar la cámara sobre el eje X (vertical, arriba/abajo)
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -80f, 80f);  // Limitar el ángulo de la cámara para no mirar completamente arriba o abajo
        cameraTransform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);  // Aseguramos que solo la cámara se incline verticalmente
    }

    void Jump()
    {
        if (IsGrounded())
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);  // Resetea la velocidad en el eje Y para evitar saltos acumulados
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);  // Añadir la fuerza de salto
        }
    }

    bool IsGrounded()
    {
        // Usamos el CapsuleCollider para verificar si está tocando el suelo
        float distanceToGround = capsuleCollider.bounds.extents.y;
        return Physics.Raycast(transform.position, Vector3.down, distanceToGround + 0.1f);
    }
}