using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IBeginDragHandler, IDragHandler, IDropHandler
{
    public Item ItemOnSlot;
    public Image Icon;  //  ★실루엣도 포함해야함

    void Start()
    {
        Icon = GetComponent<Image>();
    }

    public bool HasItem()
    {
        if (ItemOnSlot)
            return true;

        return false;
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if (HasItem())
            DragHandler.Instance.BeginDrag(this, ItemOnSlot);
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        if (DragHandler.Instance.isDragging())
            DragHandler.Instance.Drag();
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        DragHandler.Instance.Drop();

        //AudioManager.instance.Play("Unclick");
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        if (!DragHandler.Instance.isDragging())
            return;

        DragHandler.Instance.Drop(this);
    }
}
