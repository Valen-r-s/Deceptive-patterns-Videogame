using UnityEngine;

public class codigoIncorrecto : MonoBehaviour
{
    public Safe safeToMonitor;  
    public string correctCode = "290";  
    public string incorrectCode = "330"; 

    private static bool codigoErroneoIngresado = false;

    private void OnEnable()
    {
        KeypadManager.OnCodeChecked += OnCodeEntered;
    }

    private void OnDisable()
    {
        KeypadManager.OnCodeChecked -= OnCodeEntered;
    }

    private void OnCodeEntered(string enteredCode)
    {
        if (enteredCode == incorrectCode)
        {
            codigoErroneoIngresado = true; 
            Debug.LogWarning("El jugador ha ingresado el código incorrecto.");
        }
        else if (enteredCode == correctCode)
        {
            Debug.Log("El jugador ha ingresado el código correcto.");
        }
    }

    public static bool SeIngresoCodigoIncorrecto()
    {
        return codigoErroneoIngresado;
    }
}
