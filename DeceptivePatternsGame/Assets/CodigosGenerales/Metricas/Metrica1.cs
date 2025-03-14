using UnityEngine;

public class Metrica1 : MonoBehaviour
{
    public int triggerToWatch; // El ID del trigger que queremos monitorear

    // Variable est�tica para almacenar si el trigger ha sido activado
    private static bool triggerActivado = false;

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
            triggerActivado = true; // Guardar el estado temporalmente
            Debug.Log($"El jugador ha activado el trigger con la secuencia {triggerToWatch}.");
        }
    }

    // M�todo para obtener el estado del trigger en otra parte del c�digo si es necesario
    public static bool ObtenerEstadoTrigger()
    {
        return triggerActivado;
    }
}
