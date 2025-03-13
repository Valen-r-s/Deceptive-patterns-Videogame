using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class GuardarCorreo : MonoBehaviour
{
    public TMP_InputField InputCorreo; 
    public TMP_Text TxtErrorCorreo;

    // Variable para almacenar el correo temporalmente
    private static string correoGuardado;

    void Awake()
    {
        DontDestroyOnLoad(gameObject); // Evitar que se destruya al cambiar de escena
    }

    public void GuardarCorreoEnMemoria()
    {
        string correo = InputCorreo.text.Trim();

        // Si el campo está vacío, mostrar error
        if (string.IsNullOrEmpty(correo))
        {
            MostrarError("El campo de correo está vacío.");
            return;
        }

        // Validar que el correo tiene un formato válido
        if (EsCorreoValido(correo))
        {
            TxtErrorCorreo.gameObject.SetActive(false); // Ocultar mensaje de error
            correoGuardado = correo; // Guardar en memoria temporal
            Debug.Log("Correo guardado: " + correoGuardado);
        }
        else
        {
            MostrarError("Ingresa un correo valido.");
        }
    }

    // Método para validar el formato del correo electrónico
    private bool EsCorreoValido(string correo)
    {
        string patronCorreo = @"^[^@\s]+@[^@\s]+\.(com|net|org|edu|gov|mil|co|es|mx|cl|gmail|hotmail|yahoo|outlook)$";
        return Regex.IsMatch(correo, patronCorreo, RegexOptions.IgnoreCase);
    }

    // Método para mostrar mensajes de error
    private void MostrarError(string mensaje)
    {
        TxtErrorCorreo.text = mensaje;
        TxtErrorCorreo.gameObject.SetActive(true); // Activar el mensaje de error
    }

    // Método para obtener el correo almacenado (puede usarse en la escena final)
    public static string ObtenerCorreo()
    {
        return correoGuardado;
    }
}
