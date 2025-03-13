using UnityEngine;

public class CanvasActivator : MonoBehaviour
{
    public GameObject canvasToActivate; // Canvas que deseas activar

    // M�todo que se vincular� al bot�n
    public void ActivateCanvas()
    {
        if (canvasToActivate != null)
        {
            canvasToActivate.SetActive(true); // Activar el Canvas
        }
        else
        {
            Debug.LogWarning("No se ha asignado un Canvas al script GenericCanvasActivator.");
        }
    }
}
