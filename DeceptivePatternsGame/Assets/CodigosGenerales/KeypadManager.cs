using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeypadManager : MonoBehaviour
{
    public static event System.Action<string> OnCodeChecked;

    public static KeypadManager instance;  // Singleton para acceder desde cualquier objeto
    public GameObject keypadPanel;  // Panel del teclado
    public GameObject otherPanel;
    public GameObject otherPanel1;
    public TMP_Text codeDisplay;  // Donde se muestra el c�digo ingresado
    private string enteredCode = "";  // C�digo que el usuario ingresa
    private string currentCorrectCode;  // El c�digo correcto actual para verificar
    private Safe currentObject;  // Referencia al objeto actual que estamos interactuando

    public PruebaControlador playerController;  // Referencia al controlador del jugador

    private void Awake()
    {
        // Implementaci�n del patr�n Singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        keypadPanel.SetActive(false);  // Ocultar el panel inicialmente
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        otherPanel.SetActive(true);
        otherPanel1.SetActive(true);
    }

    public void ToggleKeypadPanel()
    {
        bool isActive = keypadPanel.activeSelf;
        keypadPanel.SetActive(!isActive);

        if (isActive)
        {
            // Cuando se cierra el Keypad Panel
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            ClearCode();
            otherPanel.SetActive(true);
            otherPanel1.SetActive(true);
            playerController.enabled = true;  // Desbloquear la c�mara cuando se cierra el panel
        }
        else
        {
            // Cuando se abre el Keypad Panel
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            otherPanel.SetActive(false);
            otherPanel1.SetActive(false);
            playerController.enabled = false;  // Bloquear la c�mara cuando el panel est� activo
        }
    }

    public void SetCurrentCode(string code)
    {
        currentCorrectCode = code;  // Guardar el c�digo correcto del objeto con el que estamos interactuando
    }

    public void SetCurrentObject(Safe obj)
    {
        currentObject = obj;  // Guardar referencia del objeto con el que estamos interactuando
    }

    public void AddDigit(string digit)
    {
        if (enteredCode.Length == 0 && codeDisplay.text == "Error")
        {
            // Si se ingresa un nuevo d�gito despu�s de un error, limpiar el texto
            codeDisplay.text = "";
        }

        if (enteredCode.Length < 6)  // Limitar el c�digo a 6 d�gitos
        {
            enteredCode += digit;
            codeDisplay.text = enteredCode;
        }
    }

    public void CheckCode()
    {
        OnCodeChecked?.Invoke(enteredCode);

        if (enteredCode == currentCorrectCode)
        {
            Debug.Log("C�digo correcto: Acci�n ejecutada");
            if (currentObject != null)
            {
                currentObject.PlayCorrectCodeAnimation();  // Ejecutar la animaci�n del objeto cuando el c�digo es correcto
                playerController.enabled = true;  // Reactivar la c�mara al terminar la animaci�n
            }

            // Limpiar el mensaje en caso de �xito
            ClearCode();
        }
        else
        {
            //Debug.Log("C�digo incorrecto");
            // Mostrar mensaje de error en la pantalla de c�digo
            codeDisplay.text = "Error";
        }
    }

    public void ClearCode()
    {
        enteredCode = "";
        codeDisplay.text = "";
    }
}
