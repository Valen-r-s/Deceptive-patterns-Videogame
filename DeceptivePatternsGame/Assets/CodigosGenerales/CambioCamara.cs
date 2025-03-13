using UnityEngine;

public class CCambioCamara : MonoBehaviour
{
    public Camera playerCamera;  // Cámara del jugador (primera persona)
    public Camera computerCamera; // Cámara del PC

    void Start()
    {
        // Asegurar que solo la cámara del jugador esté activa al inicio
        playerCamera.enabled = true;
        computerCamera.enabled = false;
    }

    void Update()
{
    if (Input.GetKeyDown(KeyCode.E))
    {
        if (playerCamera.enabled)
        {
            Debug.Log("Cambiando a cámara del PC");
            playerCamera.enabled = false;
            computerCamera.enabled = true;
        }
        else
        {
            Debug.Log("Cambiando a cámara del jugador");
            computerCamera.enabled = false;
            playerCamera.enabled = true;
        }
    }
}
}
