using System.Collections;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions; // Importa el espacio de nombres para Regex

public class GuardarCorreo : MonoBehaviour
{
    public Servidor servidor; // Referencia al objeto Servidor para enviar datos al servidor
    public TMP_InputField InputCorreo; // InputField para ingresar el correo electrónico
    public TMP_Text TxtErrorCorreo; // Texto para mostrar mensajes de error si el correo no es válido

    public void GuardarCorreoEnBD()
    {
        string correo = InputCorreo.text.Trim();

        // Validar que el correo electrónico tiene un formato válido
        if (EsCorreoValido(correo))
        {
            TxtErrorCorreo.gameObject.SetActive(false); // Oculta el mensaje de error si el correo es válido
            StartCoroutine(EnviarCorreoAlServidor(correo)); // Enviar el correo al servidor
        }
        else
        {
            TxtErrorCorreo.text = "Correo inválido. Asegúrate de usar un formato válido.";
            TxtErrorCorreo.gameObject.SetActive(true); // Mostrar mensaje de error si el correo es inválido
        }
    }

    // Método para validar el formato del correo electrónico
    private bool EsCorreoValido(string correo)
    {
        string patronCorreo = @"^[^@\s]+@[^@\s]+\.(com|net|org|edu|gov|mil|co|es|mx|cl|gmail|hotmail|yahoo|outlook)$";
        return Regex.IsMatch(correo, patronCorreo, RegexOptions.IgnoreCase);
    }

    // Coroutine para enviar el correo al servidor
    IEnumerator EnviarCorreoAlServidor(string correo)
    {
        string[] datos = new string[2];
        datos[0] = login.nombreRollActual; // El nombre del usuario actual (nombreroll)
        datos[1] = correo; // Correo electrónico

        // Llama al servicio "guardar_correo" en el servidor
        StartCoroutine(servidor.ConsumirServicio("guardar_correo", datos, ConfirmarCorreoGuardado));
        yield return new WaitUntil(() => !servidor.ocupado);
    }

    // Método para confirmar que el correo fue guardado en el servidor
    private void ConfirmarCorreoGuardado()
    {
        if (servidor.respuesta.codigo == 200) // Código 200 para éxito
        {
            Debug.Log("Correo guardado correctamente en la base de datos.");
        }
        else
        {
            Debug.LogError("Error al guardar el correo en el servidor: " + servidor.respuesta.mensaje);
        }
    }
}
