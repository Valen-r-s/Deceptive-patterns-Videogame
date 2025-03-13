using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjetosMercado : MonoBehaviour
{
    public GameObject indicadorInteraccion;  // Objeto que contiene el indicador de interacción (puede ser un texto, imagen, etc.)
    public Transform cameraTransform;  // Transform de la cámara del jugador
    public float raycastDistance = 5f;  // Distancia del raycast
    public Text precioText;  // Texto del precio del objeto

    private void Start()
    {
        // Asegurarse de que el indicador y el precio estén desactivados al inicio
        if (indicadorInteraccion != null)
        {
            indicadorInteraccion.SetActive(false);
        }

        if (precioText != null)
        {
            precioText.gameObject.SetActive(false);
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
            // Verificar si el objeto tiene el tag "ObjetoMercado"
            if (hit.collider.CompareTag("objeto"))
            {
                // Mostrar el indicador de interacción y el precio del objeto
                if (indicadorInteraccion != null)
                {
                    indicadorInteraccion.SetActive(true);
                }

                ObjetosMercado item = hit.collider.GetComponent<ObjetosMercado>();
                if (item != null && precioText != null)
                {
                    precioText.text = "$" + item.precio.ToString();
                    precioText.gameObject.SetActive(true);
                }
            }
            else
            {
                // Ocultar el indicador y el precio si no se apunta al objeto
                if (indicadorInteraccion != null)
                {
                    indicadorInteraccion.SetActive(false);
                }

                if (precioText != null)
                {
                    precioText.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            // Ocultar el indicador y el precio si no se apunta a ningún objeto
            if (indicadorInteraccion != null)
            {
                indicadorInteraccion.SetActive(false);
            }

            if (precioText != null)
            {
                precioText.gameObject.SetActive(false);
            }
        }
    }

    public int precio; // Precio del objeto asignado desde el Inspector
}
