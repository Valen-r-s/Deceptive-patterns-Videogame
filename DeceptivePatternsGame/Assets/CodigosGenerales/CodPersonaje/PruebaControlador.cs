using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PruebaControlador : MonoBehaviour
{
    public float walkSpeed = 10f;
    public float runSpeed = 20f;
    public float crouchHeight = 0.2f;
    public float mouseSensitivity = 600f;
    public Transform cameraTransform;
    public float raycastDistance = 5f;  // Distancia del raycast

    private float originalHeight;
    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;

    private float rotationX = 0f;

    // Variables para agacharse de forma suave
    private Vector3 escalaNormal;
    private Vector3 escalaAgachado;
    public float tiempoAgachado = 0.1f;
    //private bool agachado = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        Cursor.lockState = CursorLockMode.Locked;
        originalHeight = transform.localScale.y;

        rotationX = 0f;
        cameraTransform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);

        escalaNormal = transform.localScale;
        escalaAgachado = new Vector3(transform.localScale.x, crouchHeight, transform.localScale.z);
    }

    void Update()
    {
        // Movimiento
        Move();

        // Rotación de la cámara y el jugador
        Look();

        // Agacharse
        /*
        if (Input.GetKey(KeyCode.C))
        {
            agachado = true;
        }
        else
        {
            agachado = false;
        }

        // Interpolación suave para el agachado
        transform.localScale = Vector3.Lerp(transform.localScale, agachado ? escalaAgachado : escalaNormal, Time.deltaTime / tiempoAgachado);
        */

        rb.AddForce(Vector3.down * 10f, ForceMode.Acceleration);
    

    // Raycast para apuntar
    Raycast();
    }

    void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.right * moveHorizontal + transform.forward * moveVertical;

        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        Vector3 velocity = moveDirection * speed;
        velocity.y = rb.velocity.y;
        rb.velocity = velocity;
    }

    void Look()
    {
        if (!PanelManager.isSettingsActive)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            transform.Rotate(Vector3.up * mouseX);

            rotationX -= mouseY;
            rotationX = Mathf.Clamp(rotationX, -80f, 80f);
            cameraTransform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        }
    }

    // Eliminado el método Jump ya que el salto no es necesario
    /*
    void Jump()
    {
        if (IsGrounded())
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    */

    bool IsGrounded()
    {
        float distanceToGround = capsuleCollider.bounds.extents.y;
        return Physics.Raycast(transform.position, Vector3.down, distanceToGround + 0.1f);
    }

    void Raycast()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            // Opcional: Dibujar el rayo en la escena para visualización
            Debug.DrawRay(cameraTransform.position, cameraTransform.forward * raycastDistance, Color.red);

            // Opcional: Interacción con el objeto apuntado
            //if (hit.collider != null)
            //{
            //    //Debug.Log("Apuntando a: " + hit.collider.gameObject.name);
            //}
        }
    }
}
