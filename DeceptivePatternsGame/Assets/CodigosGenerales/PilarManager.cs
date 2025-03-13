using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PilarKeyInteraction : MonoBehaviour
{
    public Canvas interactionHintCanvas;
    public Canvas descriptionCanvas;
    [TextArea]
    public string description;
    public TextMeshProUGUI descriptionText;
    public Button closeButton;
    public GameObject keyModel;
    public MonoBehaviour playerMovementScript;
    public bool isCorrectKey = false;
    private bool playerInRange = false;
    private bool descriptionOpen = false;

    // Referencia estática para controlar una sola llave activa a la vez
    private static GameObject lastKeyModel = null;

    // Variable estática para verificar si el jugador tiene la llave correcta
    public static bool hasCorrectKey = false;

    private void Start()
    {
        interactionHintCanvas.gameObject.SetActive(false);
        descriptionCanvas.gameObject.SetActive(false);

        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(CloseDescription);
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
            interactionHintCanvas.gameObject.SetActive(false);
            if (descriptionOpen)
            {
                CloseDescription(); // Cierra el canvas si el jugador se aleja
            }
            playerInRange = false;
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!descriptionOpen)
            {
                ShowDescription(); // Abre el canvas
            }
            else
            {
                TakeKey(); // Toma la llave si el canvas ya está abierto
            }
        }
    }

    private void ShowDescription()
    {
        interactionHintCanvas.gameObject.SetActive(false);
        descriptionCanvas.gameObject.SetActive(true);
        descriptionText.text = description;
        descriptionOpen = true;
        DisablePlayerMovement();
    }

    private void CloseDescription()
    {
        descriptionCanvas.gameObject.SetActive(false);
        descriptionOpen = false;
        EnablePlayerMovement();
    }

    private void TakeKey()
    {
        if (lastKeyModel != null && lastKeyModel != keyModel)
        {
            lastKeyModel.SetActive(true);
        }

        if (keyModel != null && keyModel.activeSelf)
        {
            keyModel.SetActive(false);
            lastKeyModel = keyModel;

            if (isCorrectKey)
            {
                Debug.Log($"[{gameObject.name}] - Has tomado la llave correcta.");
                hasCorrectKey = true; // Marca que el jugador tiene la llave correcta
            }
            else
            {
                Debug.Log($"[{gameObject.name}] - Has tomado una llave incorrecta.");
                hasCorrectKey = false; // Marca que el jugador no tiene la llave correcta
            }
        }
    }

    private void DisablePlayerMovement()
    {
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = false;
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void EnablePlayerMovement()
    {
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = true;
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
