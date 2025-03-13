using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SumaValores : MonoBehaviour
{
    public Text totalText;  // Texto del UI donde se mostrará la suma total de los precios
    public GameObject warningPanel; // Panel para mostrar el mensaje de advertencia
    public TMP_Text warningText; // Texto dentro del panel de advertencia
    public string mensajeAdvertencia = "¡Un objeto de sabotaje ha salido del área!"; // Mensaje personalizable
    public float mensajeDuracion = 3f; // Duración del mensaje de advertencia en segundos

    private int totalSum = 0;
    public GameObject objetoSabotajePrefab; // Prefab del objeto que se añadirá automáticamente
    public List<Transform> spawnPoints; // Lista de puntos donde se generarán los objetos de sabotaje
    public float tiempoEntreSabotajes = 5f; // Tiempo entre la aparición de nuevos objetos de sabotaje

    private int objetosGenerados = 0;
    private int maxObjetosSabotaje = 2; // Máximo número de objetos de sabotaje a generar
    private bool primerObjetoColocado = false;
    private bool mensajeMostrado = false; // Bandera para controlar si el mensaje de advertencia ya se mostró

    private List<GameObject> objetosSabotajeClonados = new List<GameObject>(); // Lista para almacenar los objetos de sabotaje clonados

    public AudioSource musicaGeneral;  // AudioSource para la música de fondo
    public AudioClip warningSound;  // Sonido que se reproduce cuando se muestra la advertencia

    private float originalMusicVolume; // Para almacenar el volumen original de la música

    private void Start()
    {
        if (totalText != null)
        {
            totalText.text = "Total: $0";
        }

        if (warningPanel != null)
        {
            warningPanel.SetActive(false); // Inicialmente ocultamos el panel de advertencia
        }

        // Guardar el volumen original de la música de fondo
        if (musicaGeneral != null)
        {
            originalMusicVolume = musicaGeneral.volume;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verificar si el objeto tiene el script 'ObjetosMercado'
        ObjetosMercado item = other.GetComponent<ObjetosMercado>();
        if (item != null)
        {
            // Sumamos el precio del objeto
            totalSum += item.precio;
            // Actualizamos el texto en el UI
            UpdateTotalText();

            // Añadir objetos de sabotaje cuando se activa el trigger por primera vez
            if (!primerObjetoColocado)
            {
                primerObjetoColocado = true;
                StartCoroutine(AgregarObjetosSabotajeConRetraso());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Verificar si el objeto tiene el script 'ObjetosMercado'
        ObjetosMercado item = other.GetComponent<ObjetosMercado>();
        if (item != null)
        {
            // Restar el precio del objeto cuando se retira del trigger
            totalSum -= item.precio;
            // Actualizamos el texto en el UI
            UpdateTotalText();
        }

        // Verificar si el objeto que sale es un objeto de sabotaje clonado y si el mensaje no ha sido mostrado aún
        if (objetosSabotajeClonados.Contains(other.gameObject) && !mensajeMostrado)
        {
            mensajeMostrado = true; // Marcamos que el mensaje ya fue mostrado
            // Mostrar el mensaje de advertencia con retraso
            StartCoroutine(ShowWarningMessageWithDelay(1f)); // 1 segundo de retraso antes de mostrar el mensaje

            // Opcional: borrar el mensaje después de unos segundos
            StartCoroutine(ClearWarningMessageAfterDelay(mensajeDuracion + 1f)); // Agregar el retraso al tiempo total
        }
    }

    private void UpdateTotalText()
    {
        if (totalText != null)
        {
            totalText.text = "Total: $" + totalSum.ToString();
        }
    }

    private IEnumerator AgregarObjetosSabotajeConRetraso()
    {
        while (objetosGenerados < maxObjetosSabotaje)
        {
            yield return new WaitForSeconds(tiempoEntreSabotajes);

            // Instanciar el objeto de sabotaje en un punto de spawn según el número de objetos generados
            if (objetoSabotajePrefab != null && spawnPoints.Count > objetosGenerados)
            {
                Transform spawnPoint = spawnPoints[objetosGenerados];
                GameObject nuevoObjeto = Instantiate(objetoSabotajePrefab, spawnPoint.position, spawnPoint.rotation);
                nuevoObjeto.tag = "objeto"; // Asegurarse de que el objeto tenga el tag correcto

                // Agregar el objeto clonado a la lista de sabotajes
                objetosSabotajeClonados.Add(nuevoObjeto);

                // Copiar todas las propiedades del prefab
                ObjetosMercado nuevoItem = nuevoObjeto.GetComponent<ObjetosMercado>();
                if (nuevoItem != null)
                {
                    nuevoItem.precio = objetoSabotajePrefab.GetComponent<ObjetosMercado>().precio;
                }

                // Asegurarse de que el objeto tenga un Rigidbody para ser interactuado
                if (nuevoObjeto.GetComponent<Rigidbody>() == null)
                {
                    nuevoObjeto.AddComponent<Rigidbody>();
                }

                objetosGenerados++;
            }
        }
    }

    private IEnumerator ShowWarningMessageWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Espera antes de mostrar el mensaje

        if (warningPanel != null && warningText != null)
        {
            // Reducir el volumen de la música de fondo antes de mostrar el mensaje de advertencia
            if (musicaGeneral != null)
            {
                musicaGeneral.volume = originalMusicVolume * 0.3f; // Reducir el volumen de la música al 30%
            }

            warningText.text = mensajeAdvertencia; // Mostrar el mensaje personalizado
            warningPanel.SetActive(true); // Activar el panel de advertencia

            // Reproducir el sonido de advertencia
            if (warningSound != null)
            {
                PlayWarningAudio(warningSound);
            }
        }
    }

    private IEnumerator ClearWarningMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (warningPanel != null)
        {
            warningPanel.SetActive(false); // Ocultar el panel de advertencia después del retraso
        }

        // Restaurar el volumen original de la música de fondo después de ocultar el mensaje de advertencia
        if (musicaGeneral != null)
        {
            musicaGeneral.volume = originalMusicVolume;
        }
    }

    // Función para reproducir el audio del mensaje de advertencia con un GameObject temporal
    private void PlayWarningAudio(AudioClip clip)
    {
        // Crear un GameObject temporal para reproducir el audio
        GameObject audioObject = new GameObject("WarningAudio");
        AudioSource tempAudioSource = audioObject.AddComponent<AudioSource>();

        // Configurar el AudioSource
        tempAudioSource.clip = clip;
        tempAudioSource.volume = 1.0f;  // Aumentar el volumen del sonido de advertencia para destacarlo
        tempAudioSource.Play();

        // Destruir el GameObject después de que el audio termine
        Destroy(audioObject, clip.length);
    }
}
