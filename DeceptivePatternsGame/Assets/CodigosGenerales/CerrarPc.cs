using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera mainCamera;  // C�mara principal del juego
    public Camera pcCamera;    // C�mara que muestra la vista del PC
    public Canvas pcCanvas;    // Canvas que se activa cuando interact�as con el PC
    public MonoBehaviour playerMovementScript;  // Script de movimiento del jugador
    public GameObject cursorImage;  // Imagen del puntero

    void Start()
    {
        // Asegurarse de que solo la c�mara principal est� activa al iniciar
        mainCamera.enabled = true;
        pcCamera.enabled = false;
        pcCanvas.gameObject.SetActive(false);
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = true;
        }
        if (cursorImage != null)
        {
            cursorImage.SetActive(true);
        }
    }

    public void SwitchToMainCamera()
    {
        // Cambia a la c�mara principal, desactiva el canvas del PC, habilita el movimiento del jugador y muestra el puntero
        mainCamera.enabled = true;
        pcCamera.enabled = false;
        pcCanvas.gameObject.SetActive(false);
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = true;
        }
        if (cursorImage != null)
        {
            cursorImage.SetActive(true);
        }
    }

    // Este m�todo debe ser llamado para activar la c�mara del PC y el canvas
    public void SwitchToPCCamera()
    {
        mainCamera.enabled = false;
        pcCamera.enabled = true;
        pcCanvas.gameObject.SetActive(true);
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = false;
        }
        if (cursorImage != null)
        {
            cursorImage.SetActive(false);
        }
    }
}

// Nota: Este script debe estar asignado a un GameObject en la escena.
// Puedes llamar a 'SwitchToMainCamera()' desde el bot�n de salir del Canvas del PC, 
// y a 'SwitchToPCCamera()' cuando interact�es con el PC.
