using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public ContadorLlaves contadorLlaves; // Referencia al script ContadorLlaves
    public FinalSequence finalSequence;   // Referencia al script FinalSequence
    public GameObject interactionCanvas;  // Canvas para mostrar el texto de interacción
    public GameObject player;             // Referencia al jugador (capsula)
    public GameObject backgroundMusic;    // Objeto que controla la música de fondo
    public MonoBehaviour playerMovementScript; // Script de movimiento del jugador (como MonoBehaviour)

    private bool playerInTrigger = false; // Booleano para verificar si el jugador está dentro del trigger

    void Start()
    {
        interactionCanvas.SetActive(false); // Desactivar el canvas de interacción al inicio
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && contadorLlaves.llavesActuales >= 4)
        {
            playerInTrigger = true; // El jugador está dentro del trigger
            interactionCanvas.SetActive(true); // Mostrar el canvas de interacción
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false; // El jugador ha salido del trigger
            interactionCanvas.SetActive(false); // Ocultar el canvas de interacción
        }
    }

    void Update()
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            // El jugador presionó "E" una vez dentro del trigger y tiene 4 llaves
            ActivateCinematic();
        }
    }

    void ActivateCinematic()
    {
        // Activar la cinemática y ejecutar toda la lógica relacionada
        finalSequence.PlayPrologueVideo(); // Llama al método para reproducir el video de prólogo
        gameObject.SetActive(false); // Desactiva la puerta si es necesario
        interactionCanvas.SetActive(false); // Ocultar el canvas de interacción

        // Detener la música de fondo
        if (backgroundMusic != null)
        {
            backgroundMusic.SetActive(false);
        }

        // Desactivar el movimiento del jugador
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = false;
        }
    }
}
