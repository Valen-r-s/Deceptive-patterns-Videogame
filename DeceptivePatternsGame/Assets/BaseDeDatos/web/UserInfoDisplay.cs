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
        // Obtener los datos desde las variables estáticas
        string nombreCompleto = RegistrarUsuario.ObtenerNombreCompleto();
        string correo = A.ObtenerCorreo();

        if (!string.IsNullOrEmpty(nombreCompleto))
        {
            userNameTMP.text = $"{nombreCompleto}";
        }
        else
        {
            userNameTMP.text = "Nombre no registrado";
        }

        if (!string.IsNullOrEmpty(correo))
        {
            userEmailTMP.text = $"{correo}";
        }
        else
        {
            userEmailTMP.text = "Correo no registrado";
        }

        userInfoCanvas.SetActive(true);
    }
}
