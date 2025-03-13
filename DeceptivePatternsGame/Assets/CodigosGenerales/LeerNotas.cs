using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class LeerNotas : MonoBehaviour
{
    public GameObject indicadorInteraccion;  // Objeto que contiene el indicador de interacci�n (texto de "Presiona 'E' para interactuar")
    public RawImage rawImagenNota;  // Imagen que se mostrar� al interactuar
    public Transform cameraTransform;  // Transform de la c�mara del jugador
    public float raycastDistance = 5f;  // Distancia del raycast

    private bool isNear = false;  // Si el jugador est� cerca del objeto
    private bool isViewingNote = false;  // Si el jugador est� viendo la nota

    void Start()
    {
        // Asegurarse de que el indicador y la imagen est�n desactivados al inicio
        if (indicadorInteraccion != null)
        {
            indicadorInteraccion.SetActive(false);
        }
        if (rawImagenNota != null)
        {
            rawImagenNota.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        RaycastParaMostrarIndicador();

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isNear && !isViewingNote)
            {
                MostrarNota();
            }
            else if (isViewingNote)
            {
                OcultarNota();
            }
        }
    }

    void RaycastParaMostrarIndicador()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            // Verificar si el objeto tiene el tag "Interactuable"
            if (hit.collider.gameObject == gameObject)
            {
                if (!isNear)
                {
                    isNear = true;
                    if (indicadorInteraccion != null && !isViewingNote)
                    {
                        indicadorInteraccion.SetActive(true);  // Mostrar el texto de interacci�n
                    }
                }
            }
            else if (isNear)
            {
                isNear = false;
                if (indicadorInteraccion != null)
                {
                    indicadorInteraccion.SetActive(false);  // Ocultar el texto de interacci�n
                }
            }
        }
        else if (isNear)
        {
            isNear = false;
            if (indicadorInteraccion != null)
            {
                indicadorInteraccion.SetActive(false);  // Ocultar el texto de interacci�n
            }
        }
    }

    void MostrarNota()
    {
        if (rawImagenNota != null)
        {
            rawImagenNota.gameObject.SetActive(true);
            isViewingNote = true;
        }
        if (indicadorInteraccion != null)
        {
            indicadorInteraccion.SetActive(false);
        }
    }

    void OcultarNota()
    {
        if (rawImagenNota != null)
        {
            rawImagenNota.gameObject.SetActive(false);
            isViewingNote = false;
        }

        // No necesitamos verificar si el jugador sigue mirando al objeto para mostrar el indicador,
        // simplemente queremos que se cierre la nota y el indicador no vuelva a mostrarse hasta que el jugador mire nuevamente.
    }
}
