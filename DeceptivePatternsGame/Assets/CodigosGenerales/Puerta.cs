using UnityEngine;

public class Puerta : MonoBehaviour
{
    public bool isLocked = true; // La puerta est� inicialmente bloqueada
    private bool isNear = false; // Si el jugador est� cerca
    private bool isOpen = false; // Estado de la puerta (cerrada inicialmente)
    public GameObject interactMessagePanel; // Panel de interacci�n
    public GameObject lockedMessagePanel; // Panel de puerta bloqueada
    public bool unlockAfterDelay = false; // Desbloquear despu�s de un tiempo
    public float unlockDelay = 5f; // Tiempo de espera para desbloquear la puerta
    public float interactionDistance = 5f; // Distancia m�xima de interacci�n

    // Referencias de audio
    public AudioClip openSound; // Sonido para abrir la puerta
    public AudioClip closeSound; // Sonido para cerrar la puerta
    private AudioSource audioSource; // Componente AudioSource para reproducir sonidos

    private Camera playerCamera; // Referencia a la c�mara del jugador

    void Start()
    {
        // Aseg�rate de desactivar ambos paneles al inicio
        if (interactMessagePanel != null) interactMessagePanel.SetActive(false);
        if (lockedMessagePanel != null) lockedMessagePanel.SetActive(false);

        // Desbloquea la puerta despu�s de unos segundos si est� habilitado
        if (unlockAfterDelay)
        {
            Invoke("UnlockDoor", unlockDelay);
        }

        // Inicializa el componente AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();

        // Obtiene la c�mara del jugador
        playerCamera = Camera.main;
    }

    void Update()
    {
        CheckPlayerInteraction();

        // Solo permite la interacci�n si la puerta no est� bloqueada, el jugador est� cerca y presiona la tecla E
        if (!isLocked && Input.GetKeyDown(KeyCode.E) && isNear)
        {
            ToggleDoor(); // Alterna entre abrir y cerrar la puerta
        }
    }

    private void CheckPlayerInteraction()
    {
        // Realiza un Raycast desde la c�mara del jugador hacia adelante
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            if (hit.collider != null && hit.transform == transform)
            {
                if (!isNear)
                {
                    isNear = true;
                    UpdateMessagePanel();
                }
                return;
            }
        }

        if (isNear)
        {
            isNear = false;
            UpdateMessagePanel();
        }
    }

    public void UnlockDoor()
    {
        isLocked = false;
        UpdateMessagePanel();
    }

    void UpdateMessagePanel()
    {
        if (isNear)
        {
            if (isLocked)
            {
                if (lockedMessagePanel != null) lockedMessagePanel.SetActive(true);
                if (interactMessagePanel != null) interactMessagePanel.SetActive(false);
            }
            else
            {
                if (lockedMessagePanel != null) lockedMessagePanel.SetActive(false);
                if (interactMessagePanel != null) interactMessagePanel.SetActive(true);
            }
        }
        else
        {
            if (lockedMessagePanel != null) lockedMessagePanel.SetActive(false);
            if (interactMessagePanel != null) interactMessagePanel.SetActive(false);
        }
    }

    private void ToggleDoor()
    {
        if (isOpen)
        {
            // Si la puerta est� abierta, la cerramos (rotamos -90 grados)
            transform.Rotate(0, 90f, 0);
            //Debug.Log("Puerta cerrada");
            PlaySound(closeSound); // Reproducimos el sonido de cierre
        }
        else
        {
            // Si la puerta est� cerrada, la abrimos (rotamos 90 grados)
            transform.Rotate(0, -90f, 0);
            //Debug.Log("Puerta abierta");
            PlaySound(openSound); // Reproducimos el sonido de apertura
        }

        isOpen = !isOpen; // Cambiamos el estado de la puerta
        UpdateMessagePanel(); // Actualizamos el panel despu�s de cambiar el estado de la puerta
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}
