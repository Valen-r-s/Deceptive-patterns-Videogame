using UnityEngine;
using UnityEngine.UI;

public class Encuestas : MonoBehaviour
{
    public Toggle[] togglesSi; // Lista de toggles "Sí"
    public Toggle[] togglesNo; // Lista de toggles "No"
    public Button botonEnviar; // Botón para enviar las respuestas

    // Variable estática para almacenar las respuestas temporalmente
    private static string[] respuestasGuardadas = new string[12];

    void Start()
    {
        // Agregar funcionalidad al botón
        botonEnviar.onClick.AddListener(GuardarRespuestasLocalmente);

        // Asegurar que al marcar un toggle, el otro se desactive
        for (int i = 0; i < togglesSi.Length; i++)
        {
            int index = i; // Necesario para evitar problemas con closures en C#
            togglesSi[i].onValueChanged.AddListener((isOn) => OnToggleChanged(index, true, isOn));
            togglesNo[i].onValueChanged.AddListener((isOn) => OnToggleChanged(index, false, isOn));
        }
    }

    // Método para asegurar que solo un toggle pueda estar activo por pregunta
    private void OnToggleChanged(int index, bool esSi, bool isOn)
    {
        if (isOn) // Si se activó uno, desactivar el otro
        {
            if (esSi)
                togglesNo[index].isOn = false;
            else
                togglesSi[index].isOn = false;
        }
    }

    public void GuardarRespuestasLocalmente()
    {
        // Validar si hay un usuario logueado
        if (string.IsNullOrEmpty(login.nombreRollActual))
        {
            Debug.LogError("No hay usuario logueado. No se pueden guardar respuestas.");
            return;
        }

        // Capturar las respuestas de las 12 preguntas (1 para "Sí", 0 para "No")
        for (int i = 0; i < togglesSi.Length; i++)
        {
            respuestasGuardadas[i] = togglesSi[i].isOn ? "1" : "0"; // 1 si "Sí", 0 si "No"
        }

        Debug.Log($"Respuestas guardadas localmente para {login.nombreRollActual}: {string.Join(", ", respuestasGuardadas)}");
    }

    // Método para obtener las respuestas en otra parte del juego si es necesario
    public static string[] ObtenerRespuestas()
    {
        return respuestasGuardadas;
    }
}
