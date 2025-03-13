using System.Collections;
using UnityEngine;

public class Metrica1 : MonoBehaviour
{
    public int triggerToWatch; // El ID del trigger que queremos monitorear
    public Servidor servidor; // Referencia al servidor

    private void OnEnable()
    {
        DialogueTrigger.OnTriggerActivated += OnTriggerActivated;
    }

    private void OnDisable()
    {
        DialogueTrigger.OnTriggerActivated -= OnTriggerActivated;
    }

    private void OnTriggerActivated(DialogueTrigger activatedTrigger)
    {
        if (activatedTrigger.triggerSequence == triggerToWatch)
        {
            Debug.Log($"El jugador ha activado el trigger con la secuencia {triggerToWatch}.");
            StartCoroutine(ActualizarPatron(1)); // Guardar 1 en la base de datos si el trigger se activa
        }
    }

    private IEnumerator ActualizarPatron(int valorPatron)
    {
        if (servidor == null)
        {
            Debug.LogError("El servidor no está asignado. Verifica en el Inspector.");
            yield break;
        }

        // Usa login.nombreRollActual para obtener el nombre del usuario
        if (string.IsNullOrEmpty(login.nombreRollActual))
        {
            yield break;
        }

        // Preparar los datos para enviar al servidor
        string[] datos = new string[2];
        datos[0] = login.nombreRollActual; // Asigna el nombre del usuario actual
        datos[1] = valorPatron.ToString(); // Valor del patrón a actualizar

        // Consumir el servicio para actualizar el patrón
        StartCoroutine(servidor.ConsumirServicio("actualizarPatron", datos, OnPatronActualizado));
        yield return new WaitUntil(() => !servidor.ocupado);
    }

    private void OnPatronActualizado()
    {
        if (servidor.respuesta.codigo == 200)
        {
            Debug.Log("Patrón actualizado correctamente en la base de datos.");
        }
        else
        {
            Debug.LogError($"Error al actualizar el patrón: {servidor.respuesta.mensaje}");
        }
    }
}
