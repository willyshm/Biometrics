using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*Este script es para el manejo del inventario general*/

public class Inventario : MonoBehaviour
{
    public static Inventario instance;
    public GameObject inventoryUI;
    private bool isNearRecyclePoint = false;


    private void Start()
    {
        inventoryUI.SetActive(false);
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isNearRecyclePoint)
        {
            ToggleInventory();
        }
    }

    void ToggleInventory()
    {
        inventoryUI.SetActive(!inventoryUI.activeSelf);
    }

    public void SetNearRecyclePoint(bool value)
    {
        isNearRecyclePoint = value;
        if (isNearRecyclePoint)
        {
            inventoryUI.SetActive(false);
        }
    }



}


