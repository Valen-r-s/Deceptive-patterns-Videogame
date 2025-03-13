using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public ContadorLlaves contadorLlaves; // Referencia al script ContadorLlaves
    public FinalSequence finalSequence;   // Referencia al script FinalSequence
    public GameObject interactionCanvas;  // Canvas para mostrar el texto de interacci�n
    public GameObject player;             // Referencia al jugador (capsula)
    public GameObject backgroundMusic;    // Objeto que controla la m�sica de fondo
    public MonoBehaviour playerMovementScript; // Script de movimiento del jugador (como MonoBehaviour)

    private bool playerInTrigger = false; // Booleano para verificar si el jugador est� dentro del trigger

    void Start()
    {
        interactionCanvas.SetActive(false); // Desactivar el canvas de interacci�n al inicio
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && contadorLlaves.llavesActuales >= 4)
        {
            playerInTrigger = true; // El jugador est� dentro del trigger
            interactionCanvas.SetActive(true); // Mostrar el canvas de interacci�n
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false; // El jugador ha salido del trigger
            interactionCanvas.SetActive(false); // Ocultar el canvas de interacci�n
        }
    }

    void Update()
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            // El jugador presion� "E" una vez dentro del trigger y tiene 4 llaves
            ActivateCinematic();
        }
    }

    void ActivateCinematic()
    {
        // Activar la cinem�tica y ejecutar toda la l�gica relacionada
        finalSequence.PlayPrologueVideo(); // Llama al m�todo para reproducir el video de pr�logo
        gameObject.SetActive(false); // Desactiva la puerta si es necesario
        interactionCanvas.SetActive(false); // Ocultar el canvas de interacci�n

        // Detener la m�sica de fondo
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
