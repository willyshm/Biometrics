using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image itemIcon;
    private Transform originalParent;
    private Vector3 originalPosition;

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        originalPosition = transform.position;
        transform.SetParent(transform.root);
        itemIcon.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(originalParent);
        transform.position = originalPosition;
        itemIcon.raycastTarget = true;

        GameObject dropZone = eventData.pointerCurrentRaycast.gameObject;
        if (dropZone != null && dropZone.GetComponent<DropZone>() != null)
        {
            dropZone.GetComponent<DropZone>().OnItemDropped(this);
        }
    }
}
