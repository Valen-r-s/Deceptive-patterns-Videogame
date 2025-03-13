using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Encuestas : MonoBehaviour
{
    public Servidor servidor;
    public Toggle[] togglesSi; // Lista de toggles "Sí"
    public Toggle[] togglesNo; // Lista de toggles "No"
    public Button botonEnviar; // Botón para enviar las respuestas

    void Start()
    {
        // Agregar funcionalidad al botón
        botonEnviar.onClick.AddListener(EnviarTodasLasRespuestas);
    }
    
    public void EnviarTodasLasRespuestas()
    {
        // Validar si hay un usuario logueado
        if (string.IsNullOrEmpty(login.nombreRollActual))
        {
            Debug.LogError("No hay usuario logueado. No se pueden enviar respuestas.");
            return;
        }

        // Capturar las respuestas de las 12 preguntas (1 para "Sí", 0 para "No")
        string[] respuestas = new string[12];
        for (int i = 0; i < togglesSi.Length; i++)
        {
            respuestas[i] = togglesSi[i].isOn ? "1" : "0"; // 1 si "Sí", 0 si "No"
        }

        // Iniciar la solicitud para enviar las respuestas
        StartCoroutine(EnviarRespuestasAlServidor(respuestas));
    }

    IEnumerator EnviarRespuestasAlServidor(string[] respuestas)
    {
        WWWForm formulario = new WWWForm();
        formulario.AddField("nombreroll", login.nombreRollActual); // Nombre del usuario

        // Agregar respuestas al formulario
        for (int i = 0; i < respuestas.Length; i++)
        {
            formulario.AddField($"p{i + 1}", respuestas[i]);
        }

        // Enviar la solicitud al servidor
        UnityWebRequest request = UnityWebRequest.Post(servidor.servidor + "/encuestas.php", formulario);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log($"Respuesta del servidor: {request.downloadHandler.text}");
            var respuesta = JsonUtility.FromJson<Respuesta>(request.downloadHandler.text);

            if (respuesta.codigo == 201)
            {
                Debug.Log("Respuestas enviadas correctamente.");
            }
            else
            {
                Debug.LogError($"Error enviando respuestas: {respuesta.mensaje}");
            }
        }
        else
        {
            Debug.LogError($"Error en la solicitud: {request.error}");
        }
    }
}
