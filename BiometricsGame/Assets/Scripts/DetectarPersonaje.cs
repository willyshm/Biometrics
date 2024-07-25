using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectarPersonaje : MonoBehaviour
{
    public string tipoRecoleccion;
    private bool dentroDeZonaReciclaje;
    public GameObject zonaDeReciclajeUI;

    private void Start()
    {
        zonaDeReciclajeUI.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            dentroDeZonaReciclaje = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            dentroDeZonaReciclaje = false;
            zonaDeReciclajeUI.SetActive(false); 
        }
    }

    private void Update()
    {
        if (dentroDeZonaReciclaje && Input.GetKeyDown(KeyCode.R))
        {
            zonaDeReciclajeUI.SetActive(true);
        }
    }
}
