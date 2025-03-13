using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

[CreateAssetMenu(fileName = "Servidor", menuName = "Game/Servidor", order = 1)]
public class Servidor : ScriptableObject
{
    public string servidor; // URL base del servidor
    public Servicio[] servicios; // Lista de servicios disponibles

    public bool ocupado = false; // Indicador de si el servidor está ocupado
    public Respuesta respuesta; // Respuesta genérica del servidor

    // Método para consumir un servicio
    public IEnumerator ConsumirServicio(string nombre, string[] datos, UnityAction callback)
    {
        ocupado = true;

        WWWForm formulario = new WWWForm();
        Servicio s = null;

        // Buscar el servicio correspondiente
        foreach (var servicio in servicios)
        {
            if (servicio.nombre.Equals(nombre))
            {
                s = servicio;
                break;
            }
        }

        // Verificar si el servicio existe
        if (s == null)
        {
            Debug.LogError("Servicio no encontrado: " + nombre);
            ocupado = false;
            yield break;
        }

        // Añadir los parámetros al formulario
        for (int i = 0; i < s.parametros.Length; i++)
        {
            formulario.AddField(s.parametros[i], datos[i]);
        }

        // Realizar la petición POST al servidor
        UnityWebRequest www = UnityWebRequest.Post(servidor + "/" + s.URL, formulario);
        Debug.Log("URL solicitada: " + servidor + "/" + s.URL);
        yield return www.SendWebRequest();

        // Manejar errores de la solicitud
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error en la conexión: " + www.error);
            respuesta = new Respuesta(); // Código por defecto (404)
        }
        else
        {
            string jsonResponse = www.downloadHandler.text;
            Debug.Log("Respuesta JSON recibida: " + jsonResponse);

            try
            {
                respuesta = JsonUtility.FromJson<Respuesta>(jsonResponse); // Intentar analizar el JSON
            }
            catch (System.ArgumentException e)
            {
                Debug.LogError("Error al parsear JSON: " + e.Message);
                Debug.LogError("Contenido de JSON recibido: " + jsonResponse);
                respuesta = new Respuesta(); // Usar respuesta por defecto en caso de error
            }
        }

        ocupado = false;
        callback?.Invoke(); 
    }
}

[System.Serializable]
public class Servicio
{
    public string nombre;  // Nombre del servicio (ej. "registro", "login", "actualizar_llaves")
    public string URL;     // URL del archivo PHP en el servidor (ej. "reg_usuario.php", "login.php", "actualizar_llaves.php")
    public string[] parametros;  // Lista de parámetros que requiere el servicio
}

[System.Serializable]
public class Respuesta
{
    public int codigo;        // Código de la respuesta (ej. 201, 403, 404)
    public string mensaje;     // Mensaje de la respuesta del servidor (ej. "Correo guardado correctamente")
    public int llaves;
    public string nombrecompleto;
    public string correo;

    public Respuesta()
    {
        codigo = 404;         // Código por defecto en caso de error
        mensaje = "";         // Mensaje por defecto vacío
        llaves = 0;
        nombrecompleto = "";  // Valores por defecto
        correo = "";
    }
}
