using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueTrigger : MonoBehaviour
{
    public GameManager gameManager;
    public float delayBeforeDialogue = 2.0f;
    public float delayBetweenDialogues = 1.0f;
    public float durationOfDialogue = 5.0f;

    [System.Serializable]
    public struct DialogueEntry
    {
        public string texto; // Texto principal del di�logo
        public bool mostrarMision; 
        public string textoMision; // Texto de misi�n espec�fico para este di�logo
        public AudioClip audioClip; // Clip de audio asociado con este di�logo
    }

    public DialogueEntry[] dialogos; 
    public GameObject dialoguePanel; 
    public TMP_Text dialogueText; 
    public TMP_Text missionText; 

    public AudioSource musicaFondo;

    public int triggerSequence; // Secuencia necesaria para activar este trigger
    public GameManager.GameState nextState; // Estado a cambiar despu�s de este di�logo
    public bool changeState; // Indica si este trigger cambia el estado del juego

    private bool isDialogueActive = false; // Para evitar colisiones durante el di�logo
    private int dialoguesShownCount = 0; // Contador de di�logos mostrados


    // Variables para la verificaci�n de triggers completados
   
    private const int totalTriggersToComplete = 14; 

    private float originalMusicVolume; // Para almacenar el volumen original de la m�sica

    // Crear un evento para notificar que se ha activado un trigger
    public static event System.Action<DialogueTrigger> OnTriggerActivated;

    private void Start()
    {
        // Guardar el volumen original de la m�sica de fondo
        if (musicaFondo != null)
        {
            originalMusicVolume = musicaFondo.volume;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verificamos si el di�logo est� activo para no activar m�ltiples di�logos simult�neamente
        if (other.CompareTag("Player") && gameManager.dialogueSequence == triggerSequence && !isDialogueActive)
        {
            StartCoroutine(HandleDialogue());
            // Invocar el evento cuando el trigger se active
            OnTriggerActivated?.Invoke(this);
        }
    }

    private IEnumerator HandleDialogue()
    {
        isDialogueActive = true; // Marcamos que un di�logo est� en progreso

        // Reducir el volumen de la m�sica de fondo
        if (musicaFondo != null)
        {
            musicaFondo.volume = originalMusicVolume * 0.2f; // Reducir  volumen de la m�sica
        }

        // Retraso opcional antes de que comience el di�logo
        yield return new WaitForSeconds(delayBeforeDialogue);

        // Mostramos el panel de di�logo
        dialoguePanel.SetActive(true);

        // Recorremos todos los di�logos del array
        foreach (DialogueEntry dialogo in dialogos)
        {
            dialogueText.text = dialogo.texto; // Asignar el texto del di�logo

            // Reproducir el audio correspondiente si existe
            if (dialogo.audioClip != null)
            {
                PlayDialogueAudio(dialogo.audioClip);
            }

            dialoguesShownCount++; // Aumentamos el contador de di�logos mostrados

            // Controlar si mostramos o actualizamos el texto de misi�n
            if (dialogo.mostrarMision)
            {
                missionText.text = dialogo.textoMision;
                missionText.gameObject.SetActive(true); // Aseguramos que el texto de misi�n est� visible
            }

            // Mantenemos el di�logo principal visible por el tiempo especificado
            yield return new WaitForSeconds(durationOfDialogue);

            // Retraso entre di�logos consecutivos (si est� configurado)
            yield return new WaitForSeconds(delayBetweenDialogues);
        }

        // Restaurar el volumen de la m�sica de fondo
        if (musicaFondo != null)
        {
            musicaFondo.volume = originalMusicVolume;
        }

        // Desactivamos el panel de di�logo despu�s de que todos los di�logos hayan terminado
        dialoguePanel.SetActive(false);

        // Condicional para mantener o desactivar el texto de misi�n seg�n el �ltimo di�logo
        DialogueEntry ultimoDialogo = dialogos[dialogos.Length - 1];
        if (!ultimoDialogo.mostrarMision)
        {
            missionText.gameObject.SetActive(false); // Solo ocultamos si el �ltimo di�logo no necesita mostrar misi�n
        }

        // Avanzamos en la secuencia de di�logos
        gameManager.AdvanceDialogueSequence();

        // Reiniciamos el contador de di�logos para la pr�xima secuencia
        dialoguesShownCount = 0;
        isDialogueActive = false; // Marcamos que el di�logo ha finalizado
    }

    private void PlayDialogueAudio(AudioClip clip)
    {
        // Crear un GameObject temporal para reproducir el audio
        GameObject audioObject = new GameObject("DialogueAudio");
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();

        // Configurar el AudioSource
        audioSource.clip = clip;
        audioSource.volume = 1.0f; // Aumentar un poco el volumen del di�logo para destacarlo
        audioSource.Play();

        // Destruir el GameObject despu�s de que el audio termine
        Destroy(audioObject, clip.length);
    }
}
