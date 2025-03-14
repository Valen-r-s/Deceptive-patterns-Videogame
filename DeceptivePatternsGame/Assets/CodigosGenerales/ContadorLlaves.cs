using UnityEngine;
using TMPro;

public class ContadorLlaves : MonoBehaviour
{
    public TextMeshProUGUI textoLlaves; 
    public int llavesActuales = 0;      

    void Start()
    {
        textoLlaves.text = llavesActuales.ToString();
        Debug.Log("Juego iniciado, llaves locales: " + llavesActuales);
    }

    public void ActualizarContador()
    {
        Debug.Log("Actualizando contador en la UI, llaves actuales: " + llavesActuales);
        textoLlaves.text = llavesActuales.ToString();
    }

    public void RecolectarLlave()
    {
        llavesActuales++; 
        Debug.Log("Llave recolectada, nuevas llaves: " + llavesActuales);
        ActualizarContador();   
    }

    public int ObtenerNumeroDeLlaves()
    {
        return llavesActuales;
    }
}
