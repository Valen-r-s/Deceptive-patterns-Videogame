using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Safe : MonoBehaviour
{
    public GameObject interactionText;  // Texto que muestra "Presiona E para interactuar"
    public string correctCode = "1234";  // El c�digo correcto para este objeto

    public GameObject objectToAnimate;  // El objeto que contiene el Animator
    public AnimationClip animationClip;  // La animaci�n que se ejecutar� al verificar el c�digo correcto
    public GameObject objectToShow;  // El objeto que aparecer� cuando el c�digo sea correcto
    public GameObject dialogPanel;  // Panel de di�logo que se mostrar� al ingresar el c�digo correcto
    public TMP_Text dialogText;  // Texto del panel de di�logo que se podr� editar
    public string dialogMessage = "C�digo correcto, caja desbloqueada.";  // Mensaje del di�logo a mostrar
    public float dialogDelay = 1.0f;  // Tiempo de retraso antes de mostrar el di�logo
    public float dialogDuration = 3.0f;  // Duraci�n del di�logo
    public Transform cameraTransform;  // Transform de la c�mara del jugador
    public float raycastDistance = 5f;  // Distancia del raycast

    public AudioSource musicaGeneral;  // AudioSource para la m�sica general del ambiente
    public AudioClip dialogVoiceClip;  // Clip de audio de la voz del di�logo

    private float originalMusicVolume;  // Volumen original de la m�sica general
    private bool isNear = false;  // Si el jugador est� apuntando al objeto
    private Animator objectAnimator;  // Animator del objeto
    private Animation objectAnimation;  // Si prefieres usar el componente Animation en vez de Animator
    private bool hasBeenUnlocked = false;  // Verifica si el c�digo ya ha sido ingresado correctamente

    void Start()
    {
        if (interactionText != null)
        {
            interactionText.SetActive(false);  // Ocultar el texto de interacci�n al inicio
        }

        if (objectToAnimate != null)
        {
            objectAnimator = objectToAnimate.GetComponent<Animator>();  // Obtener el componente Animator del objeto a animar
            objectAnimation = objectToAnimate.GetComponent<Animation>();  // Para usar Animation si lo prefieres
        }
        else
        {
            Debug.LogWarning("No se asign� un objeto para animar en " + gameObject.name);
        }

        // Asegurarse de que el objeto a mostrar est� desactivado al inicio
        if (objectToShow != null)
        {
            objectToShow.SetActive(false);  // El objeto permanece oculto hasta que se introduce el c�digo correcto
        }

        // Asegurarse de que el panel de di�logo est� desactivado al inicio
        if (dialogPanel != null)
        {
            dialogPanel.SetActive(false);
        }

        // Guardar el volumen original de la m�sica general
        if (musicaGeneral != null)
        {
            originalMusicVolume = musicaGeneral.volume;
        }
    }

    void Update()
    {
        RaycastParaMostrarIndicador();

        // Mostrar el mensaje solo si el jugador est� apuntando al objeto y la caja no ha sido desbloqueada a�n
        if (isNear && !hasBeenUnlocked && Input.GetKeyDown(KeyCode.E))
        {
            KeypadManager.instance.SetCurrentCode(correctCode);  // Pasar el c�digo correcto al KeypadManager
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
                        interactionText.SetActive(true);  // Mostrar el texto de interacci�n
                    }
                }
            }
            else if (isNear)
            {
                isNear = false;
                if (interactionText != null)
                {
                    interactionText.SetActive(false);  // Ocultar el texto de interacci�n
                }
            }
        }
        else if (isNear)
        {
            isNear = false;
            if (interactionText != null)
            {
                interactionText.SetActive(false);  // Ocultar el texto de interacci�n
            }
        }
    }

    // Funci�n para ejecutar la animaci�n del objeto cuando el c�digo es correcto
    public void PlayCorrectCodeAnimation()
    {
        if (objectAnimation != null && animationClip != null)
        {
            objectAnimation.clip = animationClip;
            objectAnimation.Play();  // Ejecutar la animaci�n usando AnimationClip
        }
        else if (objectAnimator != null && animationClip != null)
        {
            objectAnimator.Play(animationClip.name);  // Si prefieres usar Animator con el nombre del AnimationClip
        }
        else
        {
            Debug.LogWarning("No se encontr� Animator o Animation en el objeto " + objectToAnimate.name);
        }

        // Marcar que la caja ha sido desbloqueada
        hasBeenUnlocked = true;

        // Mostrar el objeto oculto al introducir el c�digo correcto
        if (objectToShow != null)
        {
            objectToShow.SetActive(true);  // El objeto aparecer� al introducir el c�digo correcto
        }

        // Iniciar la rutina para mostrar el panel de di�logo con un retraso
        StartCoroutine(ShowDialogWithDelay());

        // Desactivar el panel del Keypad
        KeypadManager.instance.ToggleKeypadPanel();  // Cerrar el panel del Keypad
    }

    // Corrutina para mostrar el di�logo con un retraso y duraci�n espec�fica
    private IEnumerator ShowDialogWithDelay()
    {
        yield return new WaitForSeconds(dialogDelay);  // Esperar el retraso antes de mostrar el di�logo

        if (dialogPanel != null && dialogText != null)
        {
            // Reducir el volumen de la m�sica de fondo antes de reproducir el di�logo
            if (musicaGeneral != null)
            {
                musicaGeneral.volume = originalMusicVolume * 0.2f;  // Reducir el volumen de la m�sica al 30%
            }

            dialogText.text = dialogMessage;  // Establecer el mensaje del di�logo
            dialogPanel.SetActive(true);

            // Reproducir la voz del di�logo si hay un audio asignado
            if (dialogVoiceClip != null)
            {
                PlayDialogueAudio(dialogVoiceClip);
            }

            yield return new WaitForSeconds(dialogDuration);  // Mantener el di�logo visible durante un tiempo
            dialogPanel.SetActive(false);  // Ocultar el panel de di�logo

            // Restaurar el volumen original de la m�sica general
            if (musicaGeneral != null)
            {
                musicaGeneral.volume = originalMusicVolume;
            }
        }
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
