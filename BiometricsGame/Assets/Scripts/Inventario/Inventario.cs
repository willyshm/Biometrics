using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

/*Este script es para el manejo del inventario general*/

public class Inventario : MonoBehaviour
{
    public static Inventario instance;
    public GameObject inventoryUI;
    public GameObject recycleInventoryUI;
    public List<InventorySlot> inventorySlots = new List<InventorySlot>();
    public List<InventorySlot> recycleInventorySlots = new List<InventorySlot>();
    private List<item> items = new List<item>();
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

    public void AddItem(item Item)
    {
        items.Add(Item);
        UpdateUI();
    }

    public void RemoveItem(item Item)
    {
        items.Remove(Item);
        UpdateUI();
    }

    void UpdateUI()
    {
        // Actualizar el inventario principal
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (i < items.Count)
            {
                inventorySlots[i].AddItem(items[i]);
            }
            else
            {
                inventorySlots[i].ClearSlot();
            }
        }

        // Actualizar el inventario de reciclaje si estamos en un punto de reciclaje
        if (isNearRecyclePoint)
        {
            for (int i = 0; i < recycleInventorySlots.Count; i++)
            {
                if (i < items.Count)
                {
                    recycleInventorySlots[i].AddItem(items[i]);
                }
                else
                {
                    recycleInventorySlots[i].ClearSlot();
                }
            }
        }
    }

    public void SetNearRecyclePoint(bool value)
    {
        isNearRecyclePoint = value;
        if (isNearRecyclePoint)
        {
            inventoryUI.SetActive(false);
            recycleInventoryUI.SetActive(true);
        }
        else
        {
            recycleInventoryUI.SetActive(false);
        }
        UpdateUI();
    }


}


