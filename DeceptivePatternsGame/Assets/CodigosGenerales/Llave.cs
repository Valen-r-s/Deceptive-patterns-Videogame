using System.Collections;
using UnityEngine;
using TMPro;  // Usamos TextMeshPro para el texto en pantalla

public class Llave : MonoBehaviour
{
    public Puerta doorToUnlock;
    public GameManager gameManager;
    public Servidor servidor;

    public AudioSource audioSource;  // AudioSource de la llave
    public TextMeshProUGUI mensajeUI;  // El mensaje de UI para mostrar al jugador
    private Volumen volumenManager;  // Referencia al script de Volumen para obtener el volumen de efectos
    private bool jugadorEnRango = false;  // Para verificar si el jugador está en rango para recoger la llave

    void Start()
    {
        // Asegurarse de que el AudioSource esté asignado
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();  // Obtener el AudioSource si no está asignado manualmente
        }

        // Obtener una referencia al Volumen Manager para acceder al volumen de efectos
        volumenManager = GameObject.FindObjectOfType<Volumen>();
        if (volumenManager != null)
        {
            audioSource.volume = volumenManager.sliderEfectosValue;  // Asignar el volumen de efectos de sonido inicial
        }

        // Ocultar el mensaje de interacción al inicio del juego
        if (mensajeUI != null)
        {
            mensajeUI.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnRango = true;
            // Mostrar el mensaje de "Pulsa E para recoger" cuando el jugador está en rango
            if (mensajeUI != null)
            {
                mensajeUI.text = "Pulsa E para recoger";
                mensajeUI.gameObject.SetActive(true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnRango = false;
            // Ocultar el mensaje cuando el jugador sale del rango
            if (mensajeUI != null)
            {
                mensajeUI.gameObject.SetActive(false);
            }
        }
    }

    void Update()
    {
        // Verificar si el jugador está en rango y presiona la tecla E para recoger la llave
        if (jugadorEnRango && Input.GetKeyDown(KeyCode.E))
        {
            RecolectarLlave();
        }
    }

    public void RecolectarLlave()
    {
        doorToUnlock.UnlockDoor();
        gameManager.LlaveRecolectada(gameObject.name);

        // Actualiza el contador de llaves
        GameObject.FindObjectOfType<ContadorLlaves>().RecolectarLlave();

        // Reproducir sonido de recolección de la llave con el volumen correcto de efectos
        PlaySound();

        // Actualiza las llaves en la base de datos
        StartCoroutine(ActualizarLlavesEnBD());

        // Ocultar el mensaje después de recoger la llave
        if (mensajeUI != null)
        {
            mensajeUI.gameObject.SetActive(false);
        }

        // Destruir la llave después de un breve retraso para permitir que el sonido se reproduzca completamente
        Destroy(gameObject, audioSource.clip.length);
    }

    // Método para reproducir el sonido de recolección
    void PlaySound()
    {
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.volume = volumenManager.sliderEfectosValue;  // Ajustar el volumen según el slider de efectos
            audioSource.Play();  // Reproducir el sonido
        }
    }

    IEnumerator ActualizarLlavesEnBD()
    {
        string[] datos = new string[2];
        datos[0] = login.nombreRollActual;  // Usar el nombre del usuario logueado
        datos[1] = GameObject.FindObjectOfType<ContadorLlaves>().llavesActuales.ToString();  // Usar el número actual de llaves

        // Consumir el servicio "actualizar_llaves"
        StartCoroutine(servidor.ConsumirServicio("actualizar_llaves", datos, null));
        yield return new WaitUntil(() => !servidor.ocupado);
    }

}
