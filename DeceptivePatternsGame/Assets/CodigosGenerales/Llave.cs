using UnityEngine;
using TMPro;

public class Llave : MonoBehaviour
{
    public Puerta doorToUnlock;
    public GameManager gameManager;

    public AudioSource audioSource;  
    public TextMeshProUGUI mensajeUI;  
    private Volumen volumenManager;  
    private bool jugadorEnRango = false; 

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();  
        }

        volumenManager = GameObject.FindObjectOfType<Volumen>();
        if (volumenManager != null)
        {
            audioSource.volume = volumenManager.sliderEfectosValue;  
        }

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
            if (mensajeUI != null)
            {
                mensajeUI.gameObject.SetActive(false);
            }
        }
    }

    void Update()
    {
        if (jugadorEnRango && Input.GetKeyDown(KeyCode.E))
        {
            RecolectarLlave();
        }
    }

    public void RecolectarLlave()
    {
        doorToUnlock.UnlockDoor();
        gameManager.LlaveRecolectada(gameObject.name);

        GameObject.FindObjectOfType<ContadorLlaves>().RecolectarLlave();

        PlaySound();

        if (mensajeUI != null)
        {
            mensajeUI.gameObject.SetActive(false);
        }

        Destroy(gameObject, audioSource.clip.length);
    }

    void PlaySound()
    {
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.volume = volumenManager.sliderEfectosValue;  
            audioSource.Play(); 
        }
    }
}
