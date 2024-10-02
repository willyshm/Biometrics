using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public GameObject settingsPanel;  // Asigna el panel de configuración desde el Inspector

    // Método para mostrar/ocultar el menú de configuración
    public void ToggleSettingsMenu()
    {
        // Cambia el estado activo del panel
        settingsPanel.SetActive(!settingsPanel.activeSelf);
    }

    // Método para cerrar el menú de configuración 
    public void CloseSettingsMenu()
    {
        settingsPanel.SetActive(false);
    }
}
