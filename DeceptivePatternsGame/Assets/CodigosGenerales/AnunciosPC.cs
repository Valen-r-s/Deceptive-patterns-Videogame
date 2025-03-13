using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;

public class A : MonoBehaviour
{
    public GameObject[] panels;
    public GameObject[] normalPanels;
    public GameObject panelAjustes;
    public GameObject panelCorreo;
    public TMP_InputField correoInputField;
    public TMP_Text TxtErrorCorreo; 
    public Button[] BotonesInicio;
    public Button Bot�nS�;
    public Button Bot�nNo;
    public Button enviarCorreoButton;
    public CanvasGroup boton4CanvasGroup;
    public GameObject pcTrigger;

    private int panelIndex = 0;
    private bool panelsEnabled = true;
    private bool reactivarTrigger = false;
    private bool isCorreoIngresado = false;

    // Variable para almacenar el correo temporalmente
    private static string correoGuardado;

    void Start()
    {
        // Inicializaci�n: desactiva todos los paneles
        foreach (var panel in panels)
            panel.SetActive(false);
        foreach (var normalPanel in normalPanels)
            normalPanel.SetActive(false);

        panelAjustes.SetActive(false);
        panelCorreo.SetActive(false);
        Bot�nS�.gameObject.SetActive(false);
        Bot�nNo.gameObject.SetActive(false);

        // Asigna listeners a los botones principales para abrir paneles
        for (int i = 0; i < BotonesInicio.Length; i++)
        {
            int index = i;
            BotonesInicio[i].onClick.AddListener(() => OnMainButtonClick(index));
        }

        SetButton4Interactable(false);
        Bot�nS�.onClick.AddListener(DeactivatePanels);
        Bot�nNo.onClick.AddListener(ResetPanels);
        enviarCorreoButton.onClick.AddListener(ValidarYGuardarCorreo);
    }

    void ValidarYGuardarCorreo()
    {
        string email = correoInputField.text.Trim();
        string emailPattern = @"^[^@\s]+@[^@\s]+\.(com|net|org|edu|gov|mil|co|es|mx|cl|gmail|hotmail|yahoo|outlook)$"; // Nuevo patr�n de validaci�n

        if (string.IsNullOrEmpty(email))
        {
            MostrarError("El campo de correo est� vac�o.");
            return;
        }

        if (Regex.IsMatch(email, emailPattern))
        {
            isCorreoIngresado = true;
            SetButton4Interactable(true);
            panelCorreo.SetActive(false);
            correoGuardado = email; // Guardar temporalmente
            TxtErrorCorreo.gameObject.SetActive(false); // Ocultar mensaje de error
            Debug.Log("Correo v�lido y guardado: " + correoGuardado);

            // Reactiva el trigger si es necesario
            if (reactivarTrigger)
            {
                pcTrigger.SetActive(true);
                reactivarTrigger = false;
                Debug.Log("pcTrigger reactivated.");
            }
        }
        else
        {
            MostrarError("Ingresa un correo valido.");
        }
    }

    // M�todo para mostrar errores
    void MostrarError(string mensaje)
    {
        TxtErrorCorreo.text = mensaje;
        TxtErrorCorreo.gameObject.SetActive(true); // Mostrar el mensaje de error
    }

    // M�todo para obtener el correo en otras escenas
    public static string ObtenerCorreo()
    {
        return correoGuardado;
    }

    void OnMainButtonClick(int buttonIndex)
    {
        if (IsAnyPanelOpen())
            return;

        if (panelsEnabled)
        {
            if (panelIndex < panels.Length)
            {
                panels[panelIndex].SetActive(true);
                panelIndex++;
            }

            if (panelIndex == panels.Length)
            {
                Bot�nS�.gameObject.SetActive(true);
                Bot�nNo.gameObject.SetActive(true);
            }
        }
        else
        {
            if (buttonIndex < normalPanels.Length)
            {
                normalPanels[buttonIndex].SetActive(true);
            }
        }
    }

    void DeactivatePanels()
    {
        if (IsAnyPanelOpen())
            return;

        panelsEnabled = false;
        panelIndex = 0;

        Bot�nS�.gameObject.SetActive(false);
        Bot�nNo.gameObject.SetActive(false);

        if (!isCorreoIngresado)
        {
            panelCorreo.SetActive(true);
            pcTrigger.SetActive(false);
            reactivarTrigger = true;
        }
    }

    void ResetPanels()
    {
        CerrarPaneles();
        panelsEnabled = true;
        panelIndex = 0;
    }

    void SetButton4Interactable(bool interactable)
    {
        boton4CanvasGroup.interactable = interactable;
        boton4CanvasGroup.blocksRaycasts = interactable;
        boton4CanvasGroup.alpha = interactable ? 1f : 0.5f;

        if (interactable)
            BotonesInicio[3].onClick.AddListener(MostrarPanelAjustes);
        else
            BotonesInicio[3].onClick.RemoveAllListeners();
    }

    void MostrarPanelAjustes()
    {
        if (IsAnyPanelOpen())
            return;

        panelAjustes.SetActive(true);
        Bot�nS�.gameObject.SetActive(true);
        Bot�nNo.gameObject.SetActive(true);
    }

    public void CerrarPaneles()
    {
        foreach (var panel in panels)
            panel.SetActive(false);

        foreach (var normalPanel in normalPanels)
            normalPanel.SetActive(false);

        panelAjustes.SetActive(false);
        panelCorreo.SetActive(false);

        Bot�nS�.gameObject.SetActive(false);
        Bot�nNo.gameObject.SetActive(false);
    }

    bool IsAnyPanelOpen()
    {
        foreach (var panel in panels)
            if (panel.activeSelf)
                return true;
        foreach (var normalPanel in normalPanels)
            if (normalPanel.activeSelf)
                return true;
        return panelAjustes.activeSelf || panelCorreo.activeSelf;
    }
}
