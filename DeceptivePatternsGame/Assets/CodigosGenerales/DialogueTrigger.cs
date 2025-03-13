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
        public string texto; // Texto principal del diálogo
        public bool mostrarMision; 
        public string textoMision; // Texto de misión específico para este diálogo
        public AudioClip audioClip; // Clip de audio asociado con este diálogo
    }

    public DialogueEntry[] dialogos; 
    public GameObject dialoguePanel; 
    public TMP_Text dialogueText; 
    public TMP_Text missionText; 

    public AudioSource musicaFondo;

    public int triggerSequence; // Secuencia necesaria para activar este trigger
    public GameManager.GameState nextState; // Estado a cambiar después de este diálogo
    public bool changeState; // Indica si este trigger cambia el estado del juego

    private bool isDialogueActive = false; // Para evitar colisiones durante el diálogo
    private int dialoguesShownCount = 0; // Contador de diálogos mostrados


    // Variables para la verificación de triggers completados
   
    private const int totalTriggersToComplete = 14; 

    private float originalMusicVolume; // Para almacenar el volumen original de la música

    // Crear un evento para notificar que se ha activado un trigger
    public static event System.Action<DialogueTrigger> OnTriggerActivated;

    private void Start()
    {
        // Guardar el volumen original de la música de fondo
        if (musicaFondo != null)
        {
            originalMusicVolume = musicaFondo.volume;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verificamos si el diálogo está activo para no activar múltiples diálogos simultáneamente
        if (other.CompareTag("Player") && gameManager.dialogueSequence == triggerSequence && !isDialogueActive)
        {
            StartCoroutine(HandleDialogue());
            // Invocar el evento cuando el trigger se active
            OnTriggerActivated?.Invoke(this);
        }
    }

    private IEnumerator HandleDialogue()
    {
        isDialogueActive = true; // Marcamos que un diálogo está en progreso

        // Reducir el volumen de la música de fondo
        if (musicaFondo != null)
        {
            musicaFondo.volume = originalMusicVolume * 0.2f; // Reducir  volumen de la música
        }

        // Retraso opcional antes de que comience el diálogo
        yield return new WaitForSeconds(delayBeforeDialogue);

        // Mostramos el panel de diálogo
        dialoguePanel.SetActive(true);

        // Recorremos todos los diálogos del array
        foreach (DialogueEntry dialogo in dialogos)
        {
            dialogueText.text = dialogo.texto; // Asignar el texto del diálogo

            // Reproducir el audio correspondiente si existe
            if (dialogo.audioClip != null)
            {
                PlayDialogueAudio(dialogo.audioClip);
            }

            dialoguesShownCount++; // Aumentamos el contador de diálogos mostrados

            // Controlar si mostramos o actualizamos el texto de misión
            if (dialogo.mostrarMision)
            {
                missionText.text = dialogo.textoMision;
                missionText.gameObject.SetActive(true); // Aseguramos que el texto de misión esté visible
            }

            // Mantenemos el diálogo principal visible por el tiempo especificado
            yield return new WaitForSeconds(durationOfDialogue);

            // Retraso entre diálogos consecutivos (si está configurado)
            yield return new WaitForSeconds(delayBetweenDialogues);
        }

        // Restaurar el volumen de la música de fondo
        if (musicaFondo != null)
        {
            musicaFondo.volume = originalMusicVolume;
        }

        // Desactivamos el panel de diálogo después de que todos los diálogos hayan terminado
        dialoguePanel.SetActive(false);

        // Condicional para mantener o desactivar el texto de misión según el último diálogo
        DialogueEntry ultimoDialogo = dialogos[dialogos.Length - 1];
        if (!ultimoDialogo.mostrarMision)
        {
            missionText.gameObject.SetActive(false); // Solo ocultamos si el último diálogo no necesita mostrar misión
        }

        // Avanzamos en la secuencia de diálogos
        gameManager.AdvanceDialogueSequence();

        // Reiniciamos el contador de diálogos para la próxima secuencia
        dialoguesShownCount = 0;
        isDialogueActive = false; // Marcamos que el diálogo ha finalizado
    }

    private void PlayDialogueAudio(AudioClip clip)
    {
        // Crear un GameObject temporal para reproducir el audio
        GameObject audioObject = new GameObject("DialogueAudio");
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();

        // Configurar el AudioSource
        audioSource.clip = clip;
        audioSource.volume = 1.0f; // Aumentar un poco el volumen del diálogo para destacarlo
        audioSource.Play();

        // Destruir el GameObject después de que el audio termine
        Destroy(audioObject, clip.length);
    }
}
