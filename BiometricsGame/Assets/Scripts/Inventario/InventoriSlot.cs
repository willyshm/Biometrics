using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
   public Image icon;  // Componente Image para mostrar el ícono

    item Item;  // El objeto actualmente en este slot

    public void AddItem(item newItem)
    {
        Item = newItem;

        icon.sprite = Item.itemIcon;  // Establece el ícono del objeto
        icon.enabled = true;
    }

    public void ClearSlot()
    {
        Item = null;
        icon.sprite = null;
        icon.enabled = false;
    }
}
