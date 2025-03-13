using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class ChestInteraction : MonoBehaviour
{
    public Animator chestAnimator;              // Referencia al Animator del cofre
    public Canvas interactionHintCanvas;        // Canvas que muestra "Presiona E para interactuar"
    public GameObject keyObject;                // La llave dentro del cofre
    public GameObject firstDialogPanel;         // Panel del di�logo para el primer intento fallido y mensaje de �xito
    public TMP_Text firstDialogText;            // Texto para el primer intento fallido y mensaje de �xito
    public GameObject multiAttemptDialogPanel;  // Panel del di�logo para intentos m�ltiples (5to, 8vo, etc.)
    public TMP_Text multiAttemptDialogText;     // Texto para los intentos m�ltiples

    // Variables para los mensajes de di�logo, configurables en el Inspector
    [TextArea] public string firstAttemptMessage = "La llave es incorrecta. Prueba con otra llave.";
    [TextArea] public string successMessage = "�Cofre abierto exitosamente!";
    [TextArea] public string multiAttemptMessage = "A�n no tienes la llave correcta. Sigue intentando.";

    public float dialogDisplayDuration = 2f;    // Duraci�n del di�logo de intentos fallidos, ajustable desde el Inspector
    public float successDialogDuration = 3f;    // Duraci�n del di�logo de �xito, ajustable desde el Inspector
    public float openAnimationDuration = 2f;    // Duraci�n de la animaci�n de apertura del cofre
    private bool playerInRange = false;         // Verifica si el jugador est� en rango
    private int failedAttempts = 0;             // Contador de intentos fallidos
    private Collider triggerCollider;           // Referencia al Collider del propio Trigger

    public AudioSource musicaGeneral;           // AudioSource para la m�sica de fondo
    public AudioClip firstAttemptSound;         // Sonido para el primer intento fallido
    public AudioClip successSound;              // Sonido para el mensaje de �xito
    public AudioClip multiAttemptSound;         // Sonido para los intentos m�ltiples

    private float originalMusicVolume;          // Para almacenar el volumen original de la m�sica

    private void Start()
    {
        interactionHintCanvas.gameObject.SetActive(false);
        keyObject.SetActive(false); // Desactiva la llave al inicio del juego
        firstDialogPanel.SetActive(false); // Desactiva el primer panel de di�logo al inicio
        multiAttemptDialogPanel.SetActive(false); // Desactiva el panel de intentos m�ltiples al inicio

        // Obtiene el Collider del propio objeto en el que est� el script
        triggerCollider = GetComponent<Collider>();

        // Guardar el volumen original de la m�sica de fondo
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
        // Solo intenta interactuar si el jugador est� en rango y el trigger sigue habilitado
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
            chestAnimator.SetTrigger("OpenCajon"); // Activa la animaci�n de apertura del cofre
            triggerCollider.enabled = false; // Desactiva el propio trigger para evitar m�s interacciones
            firstDialogText.text = successMessage; // Muestra el mensaje de �xito
            firstDialogPanel.SetActive(true);

            // Reducir el volumen de la m�sica general y reproducir el sonido del mensaje de �xito
            if (musicaGeneral != null)
            {
                musicaGeneral.volume = originalMusicVolume * 0.3f; // Reducir el volumen de la m�sica al 30%
            }

            if (successSound != null)
            {
                PlayDialogueAudio(successSound);
            }

            StartCoroutine(HideDialogAfterDelay(firstDialogPanel, successDialogDuration)); // Oculta el panel despu�s de un tiempo espec�fico para �xito
            StartCoroutine(ActivateKeyAfterAnimation()); // Activa la llave despu�s de la animaci�n
        }
        else
        {
            HandleIncorrectKeyAttempt();
        }
    }

    private void HandleIncorrectKeyAttempt()
    {
        failedAttempts++;

        // Reducir el volumen de la m�sica de fondo antes de reproducir el di�logo
        if (musicaGeneral != null)
        {
            musicaGeneral.volume = originalMusicVolume * 0.3f; // Reducir el volumen de la m�sica al 30%
        }

        // Mostrar el primer di�logo solo en el primer intento fallido
        if (failedAttempts == 1)
        {
            firstDialogText.text = firstAttemptMessage;
            firstDialogPanel.SetActive(true);

            // Reproduce el sonido del primer intento fallido
            if (firstAttemptSound != null)
            {
                PlayDialogueAudio(firstAttemptSound);
            }

            StartCoroutine(HideDialogAfterDelay(firstDialogPanel, dialogDisplayDuration)); // Oculta el primer di�logo despu�s de un tiempo espec�fico para fallos
        }
        // Mostrar el di�logo para intentos m�ltiples en el 5�, 8�, 11� intento, etc.
        else if (failedAttempts == 5 || failedAttempts == 8 || (failedAttempts >= 11 && (failedAttempts - 5) % 3 == 0))
        {
            multiAttemptDialogText.text = multiAttemptMessage;
            multiAttemptDialogPanel.SetActive(true);

            // Reproduce el sonido para intentos m�ltiples
            if (multiAttemptSound != null)
            {
                PlayDialogueAudio(multiAttemptSound);
            }

            StartCoroutine(HideDialogAfterDelay(multiAttemptDialogPanel, dialogDisplayDuration)); // Oculta el panel de intentos m�ltiples despu�s de un tiempo
        }
        else
        {
            Debug.Log("Llave incorrecta, pero sin mostrar di�logo.");
        }
    }

    private IEnumerator HideDialogAfterDelay(GameObject dialogPanel, float duration)
    {
        yield return new WaitForSeconds(duration); // Espera el tiempo especificado en el Inspector
        dialogPanel.SetActive(false); // Oculta el panel de di�logo

        // Restaurar el volumen original de la m�sica de fondo despu�s de ocultar el di�logo
        if (musicaGeneral != null)
        {
            musicaGeneral.volume = originalMusicVolume;
        }
    }

    private IEnumerator ActivateKeyAfterAnimation()
    {
        yield return new WaitForSeconds(openAnimationDuration); // Espera hasta que la animaci�n termine
        keyObject.SetActive(true); // Activa la llave cuando termina la animaci�n de apertura
        Debug.Log("Llave activada dentro del cofre.");
    }

    // Funci�n para reproducir el audio del di�logo con un GameObject temporal
    private void PlayDialogueAudio(AudioClip clip)
    {
        // Crear un GameObject temporal para reproducir el audio
        GameObject audioObject = new GameObject("DialogueAudio");
        AudioSource tempAudioSource = audioObject.AddComponent<AudioSource>();

        // Configurar el AudioSource
        tempAudioSource.clip = clip;
        tempAudioSource.volume = 1.0f;  // Aumentar un poco el volumen del di�logo para destacarlo
        tempAudioSource.Play();

        // Destruir el GameObject despu�s de que el audio termine
        Destroy(audioObject, clip.length);
    }
}
