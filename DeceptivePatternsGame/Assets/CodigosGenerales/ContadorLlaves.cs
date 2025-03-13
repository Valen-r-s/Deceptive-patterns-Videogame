using System.Collections;
using UnityEngine;
using TMPro;

public class ContadorLlaves : MonoBehaviour
{
    public TextMeshProUGUI textoLlaves;  // El campo de texto en la UI donde se mostrará el contador
    public int llavesActuales = 0;       // Contador local de llaves
    public Servidor servidor;            // Referencia al servidor para obtener llaves de la base de datos

    void Start()
    {
        // Mostrar el número de llaves locales en la UI al inicio
        textoLlaves.text = llavesActuales.ToString();
        Debug.Log("Juego iniciado, llaves locales: " + llavesActuales);

        // Cargar las llaves actuales del jugador desde la base de datos
        StartCoroutine(CargarLlavesDesdeBD());
    }

    // Método para actualizar el texto del contador en la UI
    public void ActualizarContador()
    {
        Debug.Log("Actualizando contador en la UI, llaves actuales: " + llavesActuales);
        textoLlaves.text = llavesActuales.ToString();
    }

    // Método para cargar el número de llaves desde la base de datos
    IEnumerator CargarLlavesDesdeBD()
    {
        string[] datos = new string[1];
        datos[0] = login.nombreRollActual;  // Usar el nombre del usuario logueado
        Debug.Log("Consultando llaves para el usuario: " + datos[0]);

        // Consumir el servicio para obtener las llaves del servidor
        StartCoroutine(servidor.ConsumirServicio("obtener_llaves", datos, CallbackCargarLlaves));
        yield return new WaitUntil(() => !servidor.ocupado);
    }

    // Callback para actualizar el número de llaves desde el servidor 
    void CallbackCargarLlaves()
    {
        Debug.Log("Respuesta completa del servidor: " + JsonUtility.ToJson(servidor.respuesta));

        // Ahora Unity espera que el valor de las llaves esté directamente en "llaves"
        llavesActuales = servidor.respuesta.llaves;
        Debug.Log("Llaves actualizadas desde el servidor, nuevas llaves: " + llavesActuales);

        // Actualiza el contador de llaves en la UI
        ActualizarContador();
    }

    // Llamar a este método cuando el jugador recoja una llave
    public void RecolectarLlave()
    {
        llavesActuales++;  // Incrementar el número de llaves localmente 
        Debug.Log("Llave recolectada, nuevas llaves: " + llavesActuales);
        ActualizarContador();  // Actualizar la UI 

        // Actualizar las llaves en la base de datos
        StartCoroutine(ActualizarLlavesEnBD());
    }

    // Método para actualizar las llaves en la base de datos
    IEnumerator ActualizarLlavesEnBD()
    {
        string[] datos = new string[2];
        datos[0] = login.nombreRollActual;  // Usar el nombre del usuario logueado
        datos[1] = llavesActuales.ToString();  // Enviar la cantidad de llaves actual

        // Consumir el servicio para actualizar las llaves en la base de datos
        StartCoroutine(servidor.ConsumirServicio("actualizar_llaves", datos, null));
        yield return new WaitUntil(() => !servidor.ocupado);
    }
}
