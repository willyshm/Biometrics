using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuntoReciclaje : MonoBehaviour
{
    public GameObject recycleUI;
    private bool isPlayerInZone = false;

    private void Start()
    {
        recycleUI.SetActive(false);
    }
 

        private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInZone = true;
            Inventario.instance.SetNearRecyclePoint(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
            //recycleUI.SetActive(false);
            Inventario.instance.SetNearRecyclePoint(false);
        }
    }

    void Update()
    {
        if (isPlayerInZone && Input.GetKeyDown(KeyCode.R))
        {
            recycleUI.SetActive(true);
        }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                recycleUI.SetActive(false);
            }
        }

    }

