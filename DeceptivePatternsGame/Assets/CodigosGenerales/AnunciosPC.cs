using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;
using System.Collections;

public class A : MonoBehaviour
{
    public Servidor servidor;              // Referencia al objeto Servidor para enviar datos al servidor
    public GameObject[] panels;            // Paneles de anuncios en secuencia
    public GameObject[] normalPanels;      // Paneles normales
    public GameObject panelAjustes;        // Panel de ajustes
    public GameObject panelCorreo;         // Panel de correo para ingresar el correo electrónico
    public TMP_InputField correoInputField; // InputField para ingresar el correo electrónico
    public Button[] BotonesInicio;
    public Button BotónSí;
    public Button BotónNo;
    public Button enviarCorreoButton;
    public CanvasGroup boton4CanvasGroup;
    public GameObject pcTrigger;

    private int panelIndex = 0;
    private bool panelsEnabled = true;
    private bool reactivarTrigger = false;
    private bool isCorreoIngresado = false;

    void Start()
    {
        // Inicialización: desactiva todos los paneles
        foreach (var panel in panels)
            panel.SetActive(false);
        foreach (var normalPanel in normalPanels)
            normalPanel.SetActive(false);

        panelAjustes.SetActive(false);
        panelCorreo.SetActive(false);

        BotónSí.gameObject.SetActive(false);
        BotónNo.gameObject.SetActive(false);

        // Asigna listeners a los botones principales para abrir paneles
        for (int i = 0; i < BotonesInicio.Length; i++)
        {
            int index = i;
            BotonesInicio[i].onClick.AddListener(() => OnMainButtonClick(index));
        }

        SetButton4Interactable(false);
        BotónSí.onClick.AddListener(DeactivatePanels);
        BotónNo.onClick.AddListener(ResetPanels);
        enviarCorreoButton.onClick.AddListener(ValidarYGuardarCorreo); // Asigna el listener para el botón de enviar correo
    }

    void ValidarYGuardarCorreo()
    {
        string email = correoInputField.text;
        string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$"; // Patrón para validar el correo

        if (Regex.IsMatch(email, emailPattern))
        {
            isCorreoIngresado = true; // Marca que el correo ha sido ingresado
            SetButton4Interactable(true);
            panelCorreo.SetActive(false);
            Debug.Log("Correo válido y guardado: " + email);

            // Inicia la coroutine para enviar el correo al servidor
            StartCoroutine(EnviarCorreoAlServidor(email));

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
            Debug.Log("Por favor ingrese un correo válido.");
        }
    }

    IEnumerator EnviarCorreoAlServidor(string correo)
    {
        string[] datos = new string[2];
        datos[0] = login.nombreRollActual; // Nombre del usuario (nombreroll)
        datos[1] = correo; // Correo a guardar

        // Llama al servicio "guardar_correo" en el servidor
        StartCoroutine(servidor.ConsumirServicio("guardar_correo", datos, ConfirmarCorreoGuardado));
        yield return new WaitUntil(() => !servidor.ocupado);
    }

    private void ConfirmarCorreoGuardado()
    {
        if (servidor.respuesta.codigo == 200) // Si el servidor respondió correctamente
        {
            Debug.Log("Correo guardado correctamente en la base de datos.");
        }
        else
        {
            Debug.LogError("Error al guardar el correo en el servidor: " + servidor.respuesta.mensaje);
        }
    }

    void OnMainButtonClick(int buttonIndex)
    {
        Debug.Log("OnMainButtonClick called with buttonIndex: " + buttonIndex);

        if (IsAnyPanelOpen())
        {
            Debug.Log("Cannot open panel. Another panel is already open.");
            return;
        }

        if (panelsEnabled)
        {
            if (panelIndex < panels.Length)
            {
                panels[panelIndex].SetActive(true);
                Debug.Log("Panel " + panelIndex + " opened.");
                panelIndex++;
            }

            if (panelIndex == panels.Length)
            {
                BotónSí.gameObject.SetActive(true);
                BotónNo.gameObject.SetActive(true);
                Debug.Log("All panels shown. Displaying options to deactivate panels.");
            }
        }
        else
        {
            if (buttonIndex < normalPanels.Length)
            {
                normalPanels[buttonIndex].SetActive(true);
                Debug.Log("Normal panel " + buttonIndex + " opened.");
            }
        }
    }

    void DeactivatePanels()
    {
        Debug.Log("DeactivatePanels called.");
        if (IsAnyPanelOpen())
        {
            Debug.Log("Cannot deactivate panels. Another panel is already open.");
            return;
        }

        panelsEnabled = false;
        panelIndex = 0;

        BotónSí.gameObject.SetActive(false);
        BotónNo.gameObject.SetActive(false);
        Debug.Log("Panels deactivated.");

        if (!isCorreoIngresado)
        {
            panelCorreo.SetActive(true);
            pcTrigger.SetActive(false);
            reactivarTrigger = true;
            Debug.Log("Correo panel opened.");
        }
    }

    void ResetPanels()
    {
        Debug.Log("ResetPanels called.");
        CerrarPaneles();

        panelsEnabled = true;
        panelIndex = 0;

        Debug.Log("Panels reset. Starting over the ad sequence.");
    }

    void SetButton4Interactable(bool interactable)
    {
        Debug.Log("SetButton4Interactable called. Interactable: " + interactable);
        boton4CanvasGroup.interactable = interactable;
        boton4CanvasGroup.blocksRaycasts = interactable;
        boton4CanvasGroup.alpha = interactable ? 1f : 0.5f;

        if (interactable)
        {
            BotonesInicio[3].onClick.AddListener(MostrarPanelAjustes);
        }
        else
        {
            BotonesInicio[3].onClick.RemoveAllListeners();
        }
    }

    void MostrarPanelAjustes()
    {
        Debug.Log("MostrarPanelAjustes called.");
        if (IsAnyPanelOpen())
        {
            Debug.Log("Cannot open ajustes panel. Another panel is already open.");
            return;
        }

        panelAjustes.SetActive(true);
        BotónSí.gameObject.SetActive(true);
        BotónNo.gameObject.SetActive(true);
        Debug.Log("Ajustes panel opened.");
    }

    public void CerrarPaneles()
    {
        Debug.Log("CerrarPaneles called.");
        foreach (var panel in panels)
        {
            if (panel.activeSelf)
            {
                panel.SetActive(false);
                Debug.Log("Panel closed.");
            }
        }

        foreach (var normalPanel in normalPanels)
        {
            if (normalPanel.activeSelf)
            {
                normalPanel.SetActive(false);
                Debug.Log("Normal panel closed.");
            }
        }

        if (panelAjustes.activeSelf)
        {
            panelAjustes.SetActive(false);
            Debug.Log("Ajustes panel closed.");
        }

        if (panelCorreo.activeSelf)
        {
            panelCorreo.SetActive(false);
            Debug.Log("Correo panel closed.");
        }

        BotónSí.gameObject.SetActive(false);
        BotónNo.gameObject.SetActive(false);
    }

    bool IsAnyPanelOpen()
    {
        foreach (var panel in panels)
        {
            if (panel.activeSelf)
            {
                Debug.Log("A panel is currently open.");
                return true;
            }
        }
        foreach (var normalPanel in normalPanels)
        {
            if (normalPanel.activeSelf)
            {
                Debug.Log("A normal panel is currently open.");
                return true;
            }
        }
        if (panelAjustes.activeSelf || panelCorreo.activeSelf)
        {
            Debug.Log("Ajustes or Correo panel is currently open.");
            return true;
        }

        Debug.Log("No panels are open.");
        return false;
    }
}
