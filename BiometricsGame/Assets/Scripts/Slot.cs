using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public bool empty;
    public Sprite icon;
    public string tipo;

    public Image slotIcon;

    private void Start()
    {
        slotIcon = GetComponent<Image>();
    }

    public void UpdateSlot(Sprite newIcon, string newTipo)
    {
        icon = newIcon;
        tipo = newTipo;
        slotIcon.sprite = icon;
        empty = false;
    }

    public void ClearSlot()
    {
        icon = null;
        tipo = null;
        slotIcon.sprite = null;
        empty = true;
    }
}

    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    /*public GameObject item;
    public int ID;
    public string tipo;
    public string descripcion;

    public bool empty;
    public Sprite icon;


    public Transform slotIconGameObject;


    private void Start()
    {
        slotIconGameObject = transform.GetChild(0);
    }


    public void UpdateSlot()
    {
        slotIconGameObject.GetComponent<Image>().sprite = icon;


    }*/


