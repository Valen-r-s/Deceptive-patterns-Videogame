using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RegistrarUsuario : MonoBehaviour
{
    public Servidor servidor;

    public TMP_InputField inputNombreCompleto;
    public TMP_InputField inputNombreRoll;
    public Toggle toggleAceptar;
    public Toggle toggleNoAceptar;
    public TMP_Text txtErrorDB;
    public TMP_Text txtCorrecto;
    public TMP_Text txtUsado;
    public TMP_Text txtCampos;
    public TMP_Text txtTerminos;
    public TMP_Text txtNoAceptar;
    public TMP_Text txtNombreInvalido; // Nuevo texto para nombre inválido
    public GameObject crearUsuarioCanvas;
    public Button btnCerrarCanvas;
    public Button btnCerrarCanvasTyC;
    public GameObject TyCCanvas;

    void Start()
    {
        // Asegurarse de que solo un toggle puede estar activo a la vez
        toggleAceptar.onValueChanged.AddListener(OnToggleAceptarChanged);
        toggleNoAceptar.onValueChanged.AddListener(OnToggleNoAceptarChanged);

        btnCerrarCanvas.gameObject.SetActive(false);

        btnCerrarCanvas.onClick.AddListener(CerrarCanvasCrearUsuario);

        btnCerrarCanvasTyC.onClick.AddListener(CerrarCanvasTyC);

        // Ocultar todos los mensajes al inicio
        OcultarTodosLosMensajes();
    }

    // Si se selecciona el toggle "Aceptar", desactiva el toggle "NoAceptar"
    private void OnToggleAceptarChanged(bool isOn)
    {
        if (isOn)
        {
            toggleNoAceptar.isOn = false;
        }
    }

    // Si se selecciona el toggle "NoAceptar", desactiva el toggle "Aceptar"
    private void OnToggleNoAceptarChanged(bool isOn)
    {
        if (isOn)
        {
            toggleAceptar.isOn = false;
        }
    }

    public void RegistrarNuevoUsuario()
    {
        // Ocultar mensajes previos antes de validar
        OcultarTodosLosMensajes();

        // Validar que los campos no estén vacíos
        string nombreCompleto = inputNombreCompleto.text;
        string nombreRoll = inputNombreRoll.text;

        if (string.IsNullOrEmpty(nombreCompleto) || string.IsNullOrEmpty(nombreRoll))
        {
            txtCampos.gameObject.SetActive(true);
            return; // No continuar si los campos están vacíos
        }

        // Validar que el nombre completo contenga al menos dos palabras
        if (nombreCompleto.Trim().Split(' ').Length < 2)
        {
            txtNombreInvalido.gameObject.SetActive(true); // Mostrar mensaje de error
            txtNombreInvalido.text = "Ingresa tu nombre y apellido";
            return; // No continuar con el registro si no cumple con el requisito
        }

        // Si el usuario ha seleccionado "No Aceptar", mostrar advertencia y no continuar
        if (toggleNoAceptar.isOn)
        {
            TyCCanvas.SetActive(true);
            return; // No continuar con el registro
        }

        // Si no ha seleccionado ningún toggle, mostrar mensaje de advertencia de términos
        if (!toggleAceptar.isOn && !toggleNoAceptar.isOn)
        {
            txtTerminos.gameObject.SetActive(true);
            return; // No continuar si no se ha seleccionado ningún toggle
        }

        // Si ha seleccionado "Aceptar", continuar con el registro
        StartCoroutine(RegistrarUsuarioCoroutine());
    }

    IEnumerator RegistrarUsuarioCoroutine()
    {
        // Preparar los datos para enviar al servidor
        string nombreCompleto = inputNombreCompleto.text;
        string nombreRoll = inputNombreRoll.text;
        int terminos = toggleAceptar.isOn ? 1 : 2;

        string[] datos = new string[3];
        datos[0] = terminos.ToString();
        datos[1] = nombreRoll;
        datos[2] = nombreCompleto;

        // Consumir el servicio de registro
        StartCoroutine(servidor.ConsumirServicio("registro", datos, PosRegistro));
        yield return new WaitUntil(() => !servidor.ocupado);
    }

    // Método llamado después de recibir la respuesta del servidor
    public void PosRegistro()
    {
        // Asegurarse de que todos los mensajes anteriores estén desactivados
        OcultarTodosLosMensajes();

        switch (servidor.respuesta.codigo)
        {
            case 201: // Usuario creado correctamente
                txtCorrecto.gameObject.SetActive(true);
                btnCerrarCanvas.gameObject.SetActive(true);  // Mostrar el botón cuando el registro es exitoso
                break;

            case 403: // Usuario ya existe
                txtUsado.gameObject.SetActive(true);
                break;

            case 404: // Error de base de datos
                txtErrorDB.gameObject.SetActive(true);
                break;

            default:
                // En caso de una respuesta no esperada
                txtErrorDB.text = "Error desconocido.";
                txtErrorDB.gameObject.SetActive(true);
                break;
        }
    }

    // Ocultar todos los mensajes de error/exito
    private void OcultarTodosLosMensajes()
    {
        txtUsado.gameObject.SetActive(false);
        txtCorrecto.gameObject.SetActive(false);
        txtErrorDB.gameObject.SetActive(false);
        txtCampos.gameObject.SetActive(false);
        txtTerminos.gameObject.SetActive(false);
        txtNoAceptar.gameObject.SetActive(false);
        txtNombreInvalido.gameObject.SetActive(false); // Ocultar el mensaje de nombre inválido
    }

    public void CerrarCanvasCrearUsuario()
    {
        crearUsuarioCanvas.SetActive(false);
        btnCerrarCanvas.gameObject.SetActive(false);
        OcultarTodosLosMensajes();
    }    
    public void CerrarCanvasTyC()
    {
        TyCCanvas.SetActive(false);
        btnCerrarCanvas.gameObject.SetActive(false);
    }
}
