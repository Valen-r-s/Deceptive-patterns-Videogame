using UnityEngine;

public class CCambioCamara : MonoBehaviour
{
    public Camera playerCamera;  // C�mara del jugador (primera persona)
    public Camera computerCamera; // C�mara del PC

    void Start()
    {
        // Asegurar que solo la c�mara del jugador est� activa al inicio
        playerCamera.enabled = true;
        computerCamera.enabled = false;
    }

    void Update()
{
    if (Input.GetKeyDown(KeyCode.E))
    {
        if (playerCamera.enabled)
        {
            Debug.Log("Cambiando a c�mara del PC");
            playerCamera.enabled = false;
            computerCamera.enabled = true;
        }
        else
        {
            Debug.Log("Cambiando a c�mara del jugador");
            computerCamera.enabled = false;
            playerCamera.enabled = true;
        }
    }
}
}
