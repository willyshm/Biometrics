using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*Este script es para la recoleccion de objetos*/

public class Inventario : MonoBehaviour
{
    private bool dentroDelInventario;
    public GameObject inventario;

    private int cantidadSlots;
    private int slotsDisponibles;
    private GameObject[] slots;
    public GameObject slotholder;


    private void Start()
    {
        cantidadSlots = slotholder.transform.childCount;

        slots = new GameObject[cantidadSlots];

        for (int i = 0; i < cantidadSlots; i++)
        {
            slots[i] = slotholder.transform.GetChild(i).gameObject;

            if (slots[i].GetComponent<Slot>().item == null)
            {
                slots[i].GetComponent<Slot>().empty = true;
            }
        }
    }

    private void Update()
    {
        /*validar para abrir el inventario*/

        if (Input.GetKeyDown(KeyCode.I))
        {
            dentroDelInventario = !dentroDelInventario;
        }
        if (dentroDelInventario)
        {
            inventario.SetActive(true);
        }
        else 
        { 
            inventario.SetActive(false); 
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag=="Item")
        {
            GameObject itemPickedUp = collision.gameObject;

            Items item = itemPickedUp.GetComponent<Items>();

            AddItem(itemPickedUp, item.ID, item.tipo, item.descripcion, item.icon);
        }

    }

    public void AddItem(GameObject itemObject,int itemID, string tipoObjeto, string itemDescripcion, Sprite iconoItem)
    {
        for (int i = 0; i < cantidadSlots; i++)
        {
            if (slots[i].GetComponent<Slot>().empty)
            {
                itemObject.GetComponent<Items>().cogerObjeto = true;

                slots[i].GetComponent<Slot>().item = itemObject;
                slots[i].GetComponent<Slot>().ID = itemID;
                slots[i].GetComponent<Slot>().tipo = tipoObjeto;
                slots[i].GetComponent<Slot>().descripcion = itemDescripcion;
                slots[i].GetComponent<Slot>().icon = iconoItem;

                itemObject.transform.parent = slots[i].transform;
                itemObject.SetActive(false);

                slots[i].GetComponent<Slot>().UpdateSlot();
                
                slots[i].GetComponent<Slot>().empty = false;


                return;
            }
            
        }
    }
}
