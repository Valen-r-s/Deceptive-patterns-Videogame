using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RegistrarUsuario : MonoBehaviour
{
    public TMP_InputField inputNombreCompleto;
    public TMP_InputField inputNombreRoll;
    public Toggle toggleAceptar;
    public Toggle toggleNoAceptar;
    public TMP_Text txtCorrecto;
    public TMP_Text txtUsado;
    public TMP_Text txtCampos;
    public TMP_Text txtTerminos;
    public TMP_Text txtNoAceptar;
    public TMP_Text txtNombreInvalido;
    public GameObject crearUsuarioCanvas;
    public Button btnCerrarCanvas;
    public Button btnCerrarCanvasTyC;
    public GameObject TyCCanvas;

    // Variables para almacenar los datos temporalmente
    private string nombreCompleto;
    private string nombreRoll;
    private bool aceptoTerminos;

    void Start()
    {
        // Evitar que este objeto se destruya al cambiar de escena
        DontDestroyOnLoad(this.gameObject);

        toggleAceptar.onValueChanged.AddListener(OnToggleAceptarChanged);
        toggleNoAceptar.onValueChanged.AddListener(OnToggleNoAceptarChanged);

        btnCerrarCanvas.gameObject.SetActive(false);
        btnCerrarCanvas.onClick.AddListener(CerrarCanvasCrearUsuario);
        btnCerrarCanvasTyC.onClick.AddListener(CerrarCanvasTyC);

        OcultarTodosLosMensajes();
    }


    private void OnToggleAceptarChanged(bool isOn)
    {
        if (isOn)
        {
            toggleNoAceptar.isOn = false;
        }
    }

    private void OnToggleNoAceptarChanged(bool isOn)
    {
        if (isOn)
        {
            toggleAceptar.isOn = false;
        }
    }

    public void RegistrarNuevoUsuario()
    {
        OcultarTodosLosMensajes();

        nombreCompleto = inputNombreCompleto.text;
        nombreRoll = inputNombreRoll.text;

        if (string.IsNullOrEmpty(nombreCompleto) || string.IsNullOrEmpty(nombreRoll))
        {
            txtCampos.gameObject.SetActive(true);
            return;
        }

        if (nombreCompleto.Trim().Split(' ').Length < 2)
        {
            txtNombreInvalido.gameObject.SetActive(true);
            txtNombreInvalido.text = "Ingresa tu nombre y apellido";
            return;
        }

        if (toggleNoAceptar.isOn)
        {
            TyCCanvas.SetActive(true);
            return;
        }

        if (!toggleAceptar.isOn && !toggleNoAceptar.isOn)
        {
            txtTerminos.gameObject.SetActive(true);
            return;
        }

        aceptoTerminos = toggleAceptar.isOn;

        // Simulamos que el usuario ha sido registrado correctamente
        txtCorrecto.gameObject.SetActive(true);
        btnCerrarCanvas.gameObject.SetActive(true);
    }

    private void OcultarTodosLosMensajes()
    {
        txtCorrecto.gameObject.SetActive(false);
        txtUsado.gameObject.SetActive(false);
        txtCampos.gameObject.SetActive(false);
        txtTerminos.gameObject.SetActive(false);
        txtNoAceptar.gameObject.SetActive(false);
        txtNombreInvalido.gameObject.SetActive(false);
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

    public string ObtenerNombreCompleto()
    {
        return nombreCompleto;
    }

    public string ObtenerNombreRoll()
    {
        return nombreRoll;
    }

    public bool AceptoTerminos()
    {
        return aceptoTerminos;
    }
}
