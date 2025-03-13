using UnityEngine;

public class QuitButton : MonoBehaviour
{
    public void QuitGame()
    {
        // Cierra el juego
        Application.Quit();

        // En el Editor de Unity, esto no funciona,
        // as� que usa Debug.Log para confirmar que el m�todo fue llamado.
        Debug.Log("El juego se cerrar�a si estuviera en una compilaci�n.");
    }
}
