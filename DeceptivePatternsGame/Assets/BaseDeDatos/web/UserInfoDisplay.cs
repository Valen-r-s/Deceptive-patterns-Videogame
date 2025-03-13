using UnityEngine;
using TMPro;

public class UserInfoDisplay : MonoBehaviour
{
    public GameObject userInfoCanvas; 
    public TMP_Text userNameTMP;     
    public TMP_Text userEmailTMP;    

    private void Start()
    {
        userInfoCanvas.SetActive(false); 
    }

    public void MostrarDatosUsuario()
    {
        RegistrarUsuario registrarUsuario = FindObjectOfType<RegistrarUsuario>();
        A guardarCorreo = FindObjectOfType<A>();

        if (registrarUsuario == null || guardarCorreo == null)
        {
            Debug.LogError("No se encontró la referencia a los scripts necesarios.");
            return;
        }

        // Obtener los datos del usuario
        string nombreCompleto = registrarUsuario.ObtenerNombreCompleto();
        string correo = A.ObtenerCorreo(); // Este método es estático, así que se puede llamar directamente

        if (!string.IsNullOrEmpty(nombreCompleto) && !string.IsNullOrEmpty(correo))
        {
            userNameTMP.text = nombreCompleto;
            userEmailTMP.text = correo;
            userInfoCanvas.SetActive(true);
        }
        else
        {
            Debug.LogError("No se encontraron datos del usuario. Verifica que los ingresó correctamente.");
        }
    }
}
