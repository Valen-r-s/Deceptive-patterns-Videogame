using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RelAspect : MonoBehaviour
{
    public TMP_Dropdown aspectRatioDropdown; 
    // Resoluciones recomendadas para cada relaci�n de aspecto
    private readonly Dictionary<string, Vector2Int> aspectRatios = new Dictionary<string, Vector2Int>
    {
        {"16:9", new Vector2Int(1920, 1080)},
        {"16:10", new Vector2Int(1920, 1200)}, //Widescreen
        //{"21:9", new Vector2Int(2560, 1080)}, //Ultrawide
        {"3:2", new Vector2Int(2160, 1440)}//Relaci�n usada en algunos port�tiles y tablets modernos
    };

    void Start()
    {

        // Inicializar el dropdown de relaci�n de aspecto
        aspectRatioDropdown.ClearOptions();
        List<string> options = new List<string>(aspectRatios.Keys); // A�adir las opciones
        aspectRatioDropdown.AddOptions(options);

        // Escuchar cambios en el Dropdown
        aspectRatioDropdown.onValueChanged.AddListener(delegate { CambiarRelacionDeAspecto(aspectRatioDropdown); });
    }

    public void ActivarPantallaCompleta(bool pantallaCompleta)
    {
        Screen.fullScreen = pantallaCompleta;
    }

    // M�todo para cambiar la relaci�n de aspecto seg�n la selecci�n del Dropdown
    public void CambiarRelacionDeAspecto(TMP_Dropdown dropdown)
    {
        string selectedAspectRatio = dropdown.options[dropdown.value].text;

        if (aspectRatios.ContainsKey(selectedAspectRatio))
        {
            Vector2Int resolution = aspectRatios[selectedAspectRatio];
            Screen.SetResolution(resolution.x, resolution.y, Screen.fullScreen);
        }
    }
}
