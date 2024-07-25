using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    public string tipoRecoleccion;
    public Inventario inventario;

    public void OnDrop(PointerEventData eventData)
    {
        DraggableItem item = eventData.pointerDrag.GetComponent<DraggableItem>();
        if (item != null)
        {
            OnItemDropped(item);
        }
    }

    public void OnItemDropped(DraggableItem item)
    {
        string tipoItem = item.GetComponent<Slot>().tipo;
        if (tipoItem == tipoRecoleccion)
        {
            inventario.puntaje += inventario.bonificacion;
            item.GetComponent<Slot>().ClearSlot();
        }
        else
        {
            inventario.puntaje -= inventario.penalizacion;
        }
    }
}
