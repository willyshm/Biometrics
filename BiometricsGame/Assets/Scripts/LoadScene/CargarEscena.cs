using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CargarEscena : MonoBehaviour
{
    public void CambiarNivel(int indice)
    {
        SceneManager.LoadScene(indice);
    }
}
