using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndicadorInterac : MonoBehaviour
{
    public GameObject indicadorInteraccion;  // Objeto que contiene el indicador de interacción (puede ser un texto, imagen, etc.)
    public Transform cameraTransform;  // Transform de la cámara del jugador
    public float raycastDistance = 5f;  // Distancia del raycast

    private void Start()
    {
        // Asegurarse de que el indicador esté desactivado al inicio
        if (indicadorInteraccion != null)
        {
            indicadorInteraccion.SetActive(false);
        }
    }

    private void Update()
    {
        RaycastParaMostrarIndicador();
    }

    void RaycastParaMostrarIndicador()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            // Verificar si el objeto tiene el tag "Interactuable"
            if (hit.collider.CompareTag("objeto"))
            {
                // Mostrar el indicador de interacción
                if (indicadorInteraccion != null)
                {
                    indicadorInteraccion.SetActive(true);
                }
            }
            else
            {
                // Ocultar el indicador si no se apunta al objeto
                if (indicadorInteraccion != null)
                {
                    indicadorInteraccion.SetActive(false);
                }
            }
        }
        else
        {
            // Ocultar el indicador si no se apunta a ningún objeto
            if (indicadorInteraccion != null)
            {
                indicadorInteraccion.SetActive(false);
            }
        }
    }
}
