using System.Collections;
using UnityEngine;

public class codigoIncorrecto : MonoBehaviour
{
    public Safe safeToMonitor;  // Referencia al objeto Safe que se va a monitorear
    public string correctCode = "290";  // C�digo correcto que debe introducir el jugador
    public string incorrectCode = "330";  // C�digo incorrecto para detectar el patr�n enga�oso
    public Servidor servidor;  // Referencia al servidor

    private void OnEnable()
    {
        KeypadManager.OnCodeChecked += OnCodeEntered;
    }

    private void OnDisable()
    {
        KeypadManager.OnCodeChecked -= OnCodeEntered;
    }

    private void OnCodeEntered(string enteredCode)
    {
        // Verificar si el c�digo ingresado es incorrecto
        if (enteredCode == incorrectCode)
        {
            Debug.LogWarning("El jugador ha ingresado el c�digo incorrecto.");
            StartCoroutine(ActualizarClave(1)); // Cambiar la clave a 1
        }
        else if (enteredCode == correctCode)
        {
            Debug.Log("El jugador ha ingresado el c�digo correcto.");
        }
    }

    private IEnumerator ActualizarClave(int valorClave)
    {
        if (servidor == null)
        {
            Debug.LogError("El servidor no est� asignado. Verifica en el Inspector.");
            yield break;
        }

        // Usa login.nombreRollActual para obtener el nombre del usuario
        if (string.IsNullOrEmpty(login.nombreRollActual))
        {
            Debug.LogError("El nombre del usuario (nombreroll) no est� definido. Aseg�rate de que el usuario haya iniciado sesi�n.");
            yield break;
        }

        string[] datos = new string[2];
        datos[0] = login.nombreRollActual; // Asigna el nombre del usuario actual
        datos[1] = valorClave.ToString(); // Valor de la clave a actualizar

        Debug.Log($"Enviando datos: nombreroll={login.nombreRollActual}, clave={valorClave}");
        StartCoroutine(servidor.ConsumirServicio("actualizarClave", datos, OnClaveActualizada));
        yield return new WaitUntil(() => !servidor.ocupado);
    }

    private void OnClaveActualizada()
    {
        if (servidor.respuesta.codigo == 200)
        {
            Debug.Log("Clave actualizada correctamente en la base de datos.");
        }
        else
        {
            Debug.LogError($"Error al actualizar la clave: {servidor.respuesta.mensaje}");
        }
    }
}