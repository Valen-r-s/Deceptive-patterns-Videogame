using UnityEngine;
using UnityEngine.UI;

public class Interactuar: MonoBehaviour
{
    public GameObject interactionPanel; // Panel que contiene el texto de interacción
    public Text interactionText;        // Texto que indica que se puede interactuar
    public string interactKey = "E";    // Tecla para interactuar
    public string interactMessage = "Presiona 'E' para interactuar"; // Mensaje de interacción

    private bool isPlayerInRange = false; // Controla si el jugador está en rango

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(interactKey))
        {
            Interact();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            interactionPanel.SetActive(true);
            interactionText.text = interactMessage;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            interactionPanel.SetActive(false);
        }
    }

    private void Interact()
    {
        // Acciones a realizar cuando se interactúa con el objeto
        Debug.Log("Interactuando con el objeto!");
        // Puedes agregar aquí cualquier lógica adicional que necesites tras la interacción.
    }
}
