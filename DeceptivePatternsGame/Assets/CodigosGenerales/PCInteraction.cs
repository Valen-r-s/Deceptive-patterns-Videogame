using UnityEngine;
using UnityEngine.UI;

public class PCInteraction : MonoBehaviour
{
    public Camera playerCamera;
    public Camera computerCamera;
    public GameObject interactionText;
    public GameObject panelToDisable;
    public GameObject panelToDisable2;
    public GameObject computerCanvas;
    public MonoBehaviour playerMovementScript;
    public GameObject pointerImage; // Imagen del puntero en el canvas

    private bool isPlayerInTrigger = false;
    private RectTransform canvasRectTransform;

    void Start()
    {
        playerCamera.enabled = true;
        computerCamera.enabled = false;
        interactionText.SetActive(false);
        panelToDisable.SetActive(true);
        panelToDisable2.SetActive(true);
        computerCanvas.SetActive(false);

        // Asegurar que el puntero esté activo al inicio
        if (pointerImage != null)
        {
            pointerImage.SetActive(true);
        }

        canvasRectTransform = computerCanvas.GetComponent<RectTransform>();
    }

    void Update()
    {
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            if (playerCamera.enabled)
            {
                // Cambiar a la cámara del PC
                playerCamera.enabled = false;
                computerCamera.enabled = true;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                panelToDisable.SetActive(false); 
                panelToDisable2.SetActive(false);
                computerCanvas.SetActive(true);
                computerCanvas.GetComponent<Canvas>().worldCamera = computerCamera;

                interactionText.SetActive(false);

                if (playerMovementScript != null)
                {
                    playerMovementScript.enabled = false;
                }

                // Desactivar el puntero cuando se usa la cámara del PC
                if (pointerImage != null)
                {
                    pointerImage.SetActive(false);
                }
            }
            else
            {
                // Volver a la cámara del jugador
                computerCamera.enabled = false;
                playerCamera.enabled = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                panelToDisable.SetActive(true);
                panelToDisable2.SetActive(true);
                computerCanvas.SetActive(false);

                if (isPlayerInTrigger)
                {
                    interactionText.SetActive(true);
                }

                if (playerMovementScript != null)
                {
                    playerMovementScript.enabled = true;
                }

                // Reactivar el puntero al volver a la cámara del jugador
                if (pointerImage != null)
                {
                    pointerImage.SetActive(true);
                }
            }
        }

        if (computerCamera.enabled)
        {
            UpdateCursorVisibility();
        }
    }

    void UpdateCursorVisibility()
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector2 localPoint;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, mousePosition, computerCamera, out localPoint);

        if (canvasRectTransform.rect.Contains(localPoint))
        {
            Cursor.visible = true;
        }
        else
        {
            Cursor.visible = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = true;

            if (!computerCamera.enabled)
            {
                interactionText.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
            interactionText.SetActive(false);
        }
    }
}
