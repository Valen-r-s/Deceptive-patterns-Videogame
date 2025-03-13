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

        // Si el campo est� vac�o, mostrar error
        if (string.IsNullOrEmpty(correo))
        {
            MostrarError("El campo de correo est� vac�o.");
            return;
        }

        // Validar que el correo tiene un formato v�lido
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

    // M�todo para validar el formato del correo electr�nico
    private bool EsCorreoValido(string correo)
    {
        string patronCorreo = @"^[^@\s]+@[^@\s]+\.(com|net|org|edu|gov|mil|co|es|mx|cl|gmail|hotmail|yahoo|outlook)$";
        return Regex.IsMatch(correo, patronCorreo, RegexOptions.IgnoreCase);
    }

    // M�todo para mostrar mensajes de error
    private void MostrarError(string mensaje)
    {
        TxtErrorCorreo.text = mensaje;
        TxtErrorCorreo.gameObject.SetActive(true); // Activar el mensaje de error
    }

    // M�todo para obtener el correo almacenado (puede usarse en la escena final)
    public static string ObtenerCorreo()
    {
        return correoGuardado;
    }
}
