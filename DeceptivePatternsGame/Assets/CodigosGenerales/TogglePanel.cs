using UnityEngine;

public class TogglePanel : MonoBehaviour
{
    public GameObject panel;  // El panel que quieres mostrar/ocultar

    public void TogglePanelVisibility()
    {
        if (panel != null)
        {
            panel.SetActive(!panel.activeSelf);  // Cambia el estado activo del panel
        }
    }
}
