using UnityEngine;

public class QuitButton : MonoBehaviour
{
    public void QuitGame()
    {
        // Cierra el juego
        Application.Quit();

        // En el Editor de Unity, esto no funciona,
        // así que usa Debug.Log para confirmar que el método fue llamado.
        Debug.Log("El juego se cerraría si estuviera en una compilación.");
    }
}
