using UnityEngine;

public class PanelManager : MonoBehaviour
{
    // Paneles que se activarán/desactivarán
    public GameObject settingsPanel;
    public GameObject pantallaPanel;
    public GameObject sonidoPanel;
    public GameObject controlesPanel;
    public GameObject cuentaPanel;

    public static bool isSettingsActive = false;  // Bandera pública para saber si el panel de configuración está activo

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        settingsPanel.SetActive(false);
        DeactivateAllPanels(); // Asegúrate de que todos los paneles estén desactivados al inicio
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleSettingsPanel();
        }
    }

    // Alternar la visibilidad del panel de configuración
    public void ToggleSettingsPanel()
    {
        if (settingsPanel.activeSelf)
        {
            settingsPanel.SetActive(false);
            isSettingsActive = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            settingsPanel.SetActive(true);
            DeactivateAllPanels();  // Desactivar todos los paneles antes de activar el default
            pantallaPanel.SetActive(true);  // Activar el panel por defecto
            isSettingsActive = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    // Método para activar un panel basado en el nombre usando un switch
    public void ActivatePanel(string panelName)
    {
        DeactivateAllPanels(); // Desactiva todos los paneles antes de activar uno

        switch (panelName)
        {
            case "pantalla":
                pantallaPanel.SetActive(true);
                break;
            case "sonido":
                sonidoPanel.SetActive(true);
                break;
            case "controles":
                controlesPanel.SetActive(true);
                break;
            case "cuenta":
                cuentaPanel.SetActive(true);
                break;
            default:
                pantallaPanel.SetActive(true);  // Por defecto, activar pantalla si el nombre no coincide
                break;
        }
    }

    // Método para desactivar todos los paneles
    private void DeactivateAllPanels()
    {
        pantallaPanel.SetActive(false);
        sonidoPanel.SetActive(false);
        controlesPanel.SetActive(false);
        cuentaPanel.SetActive(false);
    }
}
