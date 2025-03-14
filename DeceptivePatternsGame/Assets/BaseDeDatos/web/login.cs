using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class login : MonoBehaviour
{
    public TMP_InputField InputUsuario;
    public TMP_Text TxtIncorrecto;
    public TMP_Text TxtCampoVacio;
    public GameObject CrearUsuario;
    public GameObject PanelInicioSesionCorrecto;

    // Variable para almacenar el nombre del usuario logueado
    public static string nombreRollActual;

    void Start()
    {
        // Asegurar que los mensajes de error estén ocultos al inicio
        OcultarMensajesAdvertencia();
    }

    public void IniciarSesion()
    {
        OcultarMensajesAdvertencia();

        if (string.IsNullOrEmpty(InputUsuario.text))
        {
            TxtCampoVacio.gameObject.SetActive(true); // Mostrar mensaje de campo vacío
            return;
        }

        // Validar si el nombre ingresado coincide con el nombre registrado en la sesión
        if (InputUsuario.text == RegistrarUsuario.ObtenerNombreRoll()) // Llamar método estático
        {
            // Inicio de sesión correcto
            nombreRollActual = InputUsuario.text;
            PanelInicioSesionCorrecto.SetActive(true);

            // Iniciar la espera de 6 segundos antes de cambiar de escena
            StartCoroutine(EsperarYCambiarEscena());
        }
        else
        {
            // Usuario incorrecto
            TxtIncorrecto.gameObject.SetActive(true);
        }
    }

    IEnumerator EsperarYCambiarEscena()
    {
        yield return new WaitForSeconds(6f);
        SceneManager.LoadScene("CasaVer3Prueba");
    }

    public void ActivarCrearUsuarioCanvas()
    {
        OcultarMensajesAdvertencia();
        CrearUsuario.SetActive(true);
    }

    private void OcultarMensajesAdvertencia()
    {
        TxtCampoVacio.gameObject.SetActive(false);
        TxtIncorrecto.gameObject.SetActive(false);
    }
}
