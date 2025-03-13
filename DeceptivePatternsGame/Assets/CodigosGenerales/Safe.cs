using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Safe : MonoBehaviour
{
    public GameObject interactionText;  // Texto que muestra "Presiona E para interactuar"
    public string correctCode = "1234";  // El código correcto para este objeto

    public GameObject objectToAnimate;  // El objeto que contiene el Animator
    public AnimationClip animationClip;  // La animación que se ejecutará al verificar el código correcto
    public GameObject objectToShow;  // El objeto que aparecerá cuando el código sea correcto
    public GameObject dialogPanel;  // Panel de diálogo que se mostrará al ingresar el código correcto
    public TMP_Text dialogText;  // Texto del panel de diálogo que se podrá editar
    public string dialogMessage = "Código correcto, caja desbloqueada.";  // Mensaje del diálogo a mostrar
    public float dialogDelay = 1.0f;  // Tiempo de retraso antes de mostrar el diálogo
    public float dialogDuration = 3.0f;  // Duración del diálogo
    public Transform cameraTransform;  // Transform de la cámara del jugador
    public float raycastDistance = 5f;  // Distancia del raycast

    public AudioSource musicaGeneral;  // AudioSource para la música general del ambiente
    public AudioClip dialogVoiceClip;  // Clip de audio de la voz del diálogo

    private float originalMusicVolume;  // Volumen original de la música general
    private bool isNear = false;  // Si el jugador está apuntando al objeto
    private Animator objectAnimator;  // Animator del objeto
    private Animation objectAnimation;  // Si prefieres usar el componente Animation en vez de Animator
    private bool hasBeenUnlocked = false;  // Verifica si el código ya ha sido ingresado correctamente

    void Start()
    {
        if (interactionText != null)
        {
            interactionText.SetActive(false);  // Ocultar el texto de interacción al inicio
        }

        if (objectToAnimate != null)
        {
            objectAnimator = objectToAnimate.GetComponent<Animator>();  // Obtener el componente Animator del objeto a animar
            objectAnimation = objectToAnimate.GetComponent<Animation>();  // Para usar Animation si lo prefieres
        }
        else
        {
            Debug.LogWarning("No se asignó un objeto para animar en " + gameObject.name);
        }

        // Asegurarse de que el objeto a mostrar esté desactivado al inicio
        if (objectToShow != null)
        {
            objectToShow.SetActive(false);  // El objeto permanece oculto hasta que se introduce el código correcto
        }

        // Asegurarse de que el panel de diálogo esté desactivado al inicio
        if (dialogPanel != null)
        {
            dialogPanel.SetActive(false);
        }

        // Guardar el volumen original de la música general
        if (musicaGeneral != null)
        {
            originalMusicVolume = musicaGeneral.volume;
        }
    }

    void Update()
    {
        RaycastParaMostrarIndicador();

        // Mostrar el mensaje solo si el jugador está apuntando al objeto y la caja no ha sido desbloqueada aún
        if (isNear && !hasBeenUnlocked && Input.GetKeyDown(KeyCode.E))
        {
            KeypadManager.instance.SetCurrentCode(correctCode);  // Pasar el código correcto al KeypadManager
            KeypadManager.instance.SetCurrentObject(this);  // Pasar el objeto actual al KeypadManager
            KeypadManager.instance.ToggleKeypadPanel();  // Mostrar el panel del Keypad
        }
    }

    void RaycastParaMostrarIndicador()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            // Verificar si el objeto tiene el tag "Safe"
            if (hit.collider.gameObject == gameObject && !hasBeenUnlocked)
            {
                if (!isNear)
                {
                    isNear = true;
                    if (interactionText != null)
                    {
                        interactionText.SetActive(true);  // Mostrar el texto de interacción
                    }
                }
            }
            else if (isNear)
            {
                isNear = false;
                if (interactionText != null)
                {
                    interactionText.SetActive(false);  // Ocultar el texto de interacción
                }
            }
        }
        else if (isNear)
        {
            isNear = false;
            if (interactionText != null)
            {
                interactionText.SetActive(false);  // Ocultar el texto de interacción
            }
        }
    }

    // Función para ejecutar la animación del objeto cuando el código es correcto
    public void PlayCorrectCodeAnimation()
    {
        if (objectAnimation != null && animationClip != null)
        {
            objectAnimation.clip = animationClip;
            objectAnimation.Play();  // Ejecutar la animación usando AnimationClip
        }
        else if (objectAnimator != null && animationClip != null)
        {
            objectAnimator.Play(animationClip.name);  // Si prefieres usar Animator con el nombre del AnimationClip
        }
        else
        {
            Debug.LogWarning("No se encontró Animator o Animation en el objeto " + objectToAnimate.name);
        }

        // Marcar que la caja ha sido desbloqueada
        hasBeenUnlocked = true;

        // Mostrar el objeto oculto al introducir el código correcto
        if (objectToShow != null)
        {
            objectToShow.SetActive(true);  // El objeto aparecerá al introducir el código correcto
        }

        // Iniciar la rutina para mostrar el panel de diálogo con un retraso
        StartCoroutine(ShowDialogWithDelay());

        // Desactivar el panel del Keypad
        KeypadManager.instance.ToggleKeypadPanel();  // Cerrar el panel del Keypad
    }

    // Corrutina para mostrar el diálogo con un retraso y duración específica
    private IEnumerator ShowDialogWithDelay()
    {
        yield return new WaitForSeconds(dialogDelay);  // Esperar el retraso antes de mostrar el diálogo

        if (dialogPanel != null && dialogText != null)
        {
            // Reducir el volumen de la música de fondo antes de reproducir el diálogo
            if (musicaGeneral != null)
            {
                musicaGeneral.volume = originalMusicVolume * 0.2f;  // Reducir el volumen de la música al 30%
            }

            dialogText.text = dialogMessage;  // Establecer el mensaje del diálogo
            dialogPanel.SetActive(true);

            // Reproducir la voz del diálogo si hay un audio asignado
            if (dialogVoiceClip != null)
            {
                PlayDialogueAudio(dialogVoiceClip);
            }

            yield return new WaitForSeconds(dialogDuration);  // Mantener el diálogo visible durante un tiempo
            dialogPanel.SetActive(false);  // Ocultar el panel de diálogo

            // Restaurar el volumen original de la música general
            if (musicaGeneral != null)
            {
                musicaGeneral.volume = originalMusicVolume;
            }
        }
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
