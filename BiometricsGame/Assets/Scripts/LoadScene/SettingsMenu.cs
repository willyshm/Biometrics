using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public GameObject settingsPanel;  // Asigna el panel de configuraci�n desde el Inspector

    // M�todo para mostrar/ocultar el men� de configuraci�n
    public void ToggleSettingsMenu()
    {
        // Cambia el estado activo del panel
        settingsPanel.SetActive(!settingsPanel.activeSelf);
    }

    // M�todo para cerrar el men� de configuraci�n 
    public void CloseSettingsMenu()
    {
        settingsPanel.SetActive(false);
    }
}
