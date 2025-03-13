using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CojerObjetoRaycast : MonoBehaviour
{
    public Transform cameraTransform;
    public float raycastDistance = 5f;  // Distancia del raycast
    public Transform HandPoint;  // Punto donde se sostendrá el objeto
    private GameObject pickedObject = null;

    void Update()
    {
        // Verificar si el jugador está tratando de recoger o soltar el objeto
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (pickedObject != null)
            {
                SoltarObjeto();
            }
            else
            {
                RaycastParaCogerObjeto();
            }
        }
    }

    void RaycastParaCogerObjeto()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            // Verificar si el objeto tiene el tag "objeto"
            if (hit.collider.CompareTag("objeto"))
            {
                CogerObjeto(hit.collider.gameObject);
            }
        }
    }

    void CogerObjeto(GameObject objeto)
    {
        Rigidbody objetoRb = objeto.GetComponent<Rigidbody>();
        if (objetoRb != null)
        {
            objetoRb.useGravity = false;
            objetoRb.isKinematic = true;

            objeto.transform.position = HandPoint.position;
            objeto.transform.SetParent(HandPoint);
            pickedObject = objeto;
        }
    }

    void SoltarObjeto()
    {
        if (pickedObject != null)
        {
            Rigidbody objetoRb = pickedObject.GetComponent<Rigidbody>();
            if (objetoRb != null)
            {
                objetoRb.useGravity = true;
                objetoRb.isKinematic = false;
            }

            pickedObject.transform.SetParent(null);
            pickedObject = null;
        }
    }
}
