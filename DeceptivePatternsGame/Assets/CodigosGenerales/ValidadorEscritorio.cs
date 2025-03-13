using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class ChestInteraction : MonoBehaviour
{
    public Animator chestAnimator;              // Referencia al Animator del cofre
    public Canvas interactionHintCanvas;        // Canvas que muestra "Presiona E para interactuar"
    public GameObject keyObject;                // La llave dentro del cofre
    public GameObject firstDialogPanel;         // Panel del diálogo para el primer intento fallido y mensaje de éxito
    public TMP_Text firstDialogText;            // Texto para el primer intento fallido y mensaje de éxito
    public GameObject multiAttemptDialogPanel;  // Panel del diálogo para intentos múltiples (5to, 8vo, etc.)
    public TMP_Text multiAttemptDialogText;     // Texto para los intentos múltiples

    // Variables para los mensajes de diálogo, configurables en el Inspector
    [TextArea] public string firstAttemptMessage = "La llave es incorrecta. Prueba con otra llave.";
    [TextArea] public string successMessage = "¡Cofre abierto exitosamente!";
    [TextArea] public string multiAttemptMessage = "Aún no tienes la llave correcta. Sigue intentando.";

    public float dialogDisplayDuration = 2f;    // Duración del diálogo de intentos fallidos, ajustable desde el Inspector
    public float successDialogDuration = 3f;    // Duración del diálogo de éxito, ajustable desde el Inspector
    public float openAnimationDuration = 2f;    // Duración de la animación de apertura del cofre
    private bool playerInRange = false;         // Verifica si el jugador está en rango
    private int failedAttempts = 0;             // Contador de intentos fallidos
    private Collider triggerCollider;           // Referencia al Collider del propio Trigger

    public AudioSource musicaGeneral;           // AudioSource para la música de fondo
    public AudioClip firstAttemptSound;         // Sonido para el primer intento fallido
    public AudioClip successSound;              // Sonido para el mensaje de éxito
    public AudioClip multiAttemptSound;         // Sonido para los intentos múltiples

    private float originalMusicVolume;          // Para almacenar el volumen original de la música

    private void Start()
    {
        interactionHintCanvas.gameObject.SetActive(false);
        keyObject.SetActive(false); // Desactiva la llave al inicio del juego
        firstDialogPanel.SetActive(false); // Desactiva el primer panel de diálogo al inicio
        multiAttemptDialogPanel.SetActive(false); // Desactiva el panel de intentos múltiples al inicio

        // Obtiene el Collider del propio objeto en el que está el script
        triggerCollider = GetComponent<Collider>();

        // Guardar el volumen original de la música de fondo
        if (musicaGeneral != null)
        {
            originalMusicVolume = musicaGeneral.volume;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactionHintCanvas.gameObject.SetActive(true);
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactionHintCanvas.gameObject.SetActive(false); // Oculta el mensaje al salir del trigger
            playerInRange = false;
        }
    }

    private void Update()
    {
        // Solo intenta interactuar si el jugador está en rango y el trigger sigue habilitado
        if (playerInRange && triggerCollider != null && triggerCollider.enabled && Input.GetKeyDown(KeyCode.E))
        {
            interactionHintCanvas.gameObject.SetActive(false); // Oculta el mensaje al presionar E
            TryOpenChest();
        }
    }

    private void TryOpenChest()
    {
        if (PilarKeyInteraction.hasCorrectKey) // El jugador tiene la llave correcta
        {
            Debug.Log("Cofre abierto correctamente.");
            chestAnimator.SetTrigger("OpenCajon"); // Activa la animación de apertura del cofre
            triggerCollider.enabled = false; // Desactiva el propio trigger para evitar más interacciones
            firstDialogText.text = successMessage; // Muestra el mensaje de éxito
            firstDialogPanel.SetActive(true);

            // Reducir el volumen de la música general y reproducir el sonido del mensaje de éxito
            if (musicaGeneral != null)
            {
                musicaGeneral.volume = originalMusicVolume * 0.3f; // Reducir el volumen de la música al 30%
            }

            if (successSound != null)
            {
                PlayDialogueAudio(successSound);
            }

            StartCoroutine(HideDialogAfterDelay(firstDialogPanel, successDialogDuration)); // Oculta el panel después de un tiempo específico para éxito
            StartCoroutine(ActivateKeyAfterAnimation()); // Activa la llave después de la animación
        }
        else
        {
            HandleIncorrectKeyAttempt();
        }
    }

    private void HandleIncorrectKeyAttempt()
    {
        failedAttempts++;

        // Reducir el volumen de la música de fondo antes de reproducir el diálogo
        if (musicaGeneral != null)
        {
            musicaGeneral.volume = originalMusicVolume * 0.3f; // Reducir el volumen de la música al 30%
        }

        // Mostrar el primer diálogo solo en el primer intento fallido
        if (failedAttempts == 1)
        {
            firstDialogText.text = firstAttemptMessage;
            firstDialogPanel.SetActive(true);

            // Reproduce el sonido del primer intento fallido
            if (firstAttemptSound != null)
            {
                PlayDialogueAudio(firstAttemptSound);
            }

            StartCoroutine(HideDialogAfterDelay(firstDialogPanel, dialogDisplayDuration)); // Oculta el primer diálogo después de un tiempo específico para fallos
        }
        // Mostrar el diálogo para intentos múltiples en el 5º, 8º, 11º intento, etc.
        else if (failedAttempts == 5 || failedAttempts == 8 || (failedAttempts >= 11 && (failedAttempts - 5) % 3 == 0))
        {
            multiAttemptDialogText.text = multiAttemptMessage;
            multiAttemptDialogPanel.SetActive(true);

            // Reproduce el sonido para intentos múltiples
            if (multiAttemptSound != null)
            {
                PlayDialogueAudio(multiAttemptSound);
            }

            StartCoroutine(HideDialogAfterDelay(multiAttemptDialogPanel, dialogDisplayDuration)); // Oculta el panel de intentos múltiples después de un tiempo
        }
        else
        {
            Debug.Log("Llave incorrecta, pero sin mostrar diálogo.");
        }
    }

    private IEnumerator HideDialogAfterDelay(GameObject dialogPanel, float duration)
    {
        yield return new WaitForSeconds(duration); // Espera el tiempo especificado en el Inspector
        dialogPanel.SetActive(false); // Oculta el panel de diálogo

        // Restaurar el volumen original de la música de fondo después de ocultar el diálogo
        if (musicaGeneral != null)
        {
            musicaGeneral.volume = originalMusicVolume;
        }
    }

    private IEnumerator ActivateKeyAfterAnimation()
    {
        yield return new WaitForSeconds(openAnimationDuration); // Espera hasta que la animación termine
        keyObject.SetActive(true); // Activa la llave cuando termina la animación de apertura
        Debug.Log("Llave activada dentro del cofre.");
    }

    // Función para reproducir el audio del diálogo con un GameObject temporal
    private void PlayDialogueAudio(AudioClip clip)
    {
        // Crear un GameObject temporal para reproducir el audio
        GameObject audioObject = new GameObject("DialogueAudio");
        AudioSource tempAudioSource = audioObject.AddComponent<AudioSource>();

        // Configurar el AudioSource
        tempAudioSource.clip = clip;
        tempAudioSource.volume = 1.0f;  // Aumentar un poco el volumen del diálogo para destacarlo
        tempAudioSource.Play();

        // Destruir el GameObject después de que el audio termine
        Destroy(audioObject, clip.length);
    }
}
