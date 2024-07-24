using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public GameObject item;
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


    }



}
