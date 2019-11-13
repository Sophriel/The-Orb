using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Sector : Slot
{
    //public int SectorNumber;
    //public GameObject CharacterOnSector;

    //void Start()
    //{
    //    icon = GetComponent<Image>();
    //    CharacterOnSector = null;
    //}

    //public override void OnBeginDrag(PointerEventData eventData)
    //{
    //    DragHandler.Instance.BeginSlot = this;

    //    if (item)
    //    {
    //        DragHandler.Instance.ItemImage.sprite = item.icon;

    //        DragHandler.Instance.ItemBeingDragged = item;
    //        DragHandler.Instance.startPosition = transform.position;
    //    }

    //    if (CharacterOnSector)
    //    {
    //        CharacterOnSector.SetActive(false);
    //    }

    //    DragHandler.Instance.canvasGroup.blocksRaycasts = false;
    //}

    //public override void OnDrag(PointerEventData eventData)
    //{
    //    if (DragHandler.Instance.ItemBeingDragged)
    //    {
    //        Vector3 screenpoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.0f);
    //        DragHandler.Instance.transform.position = Camera.main.ScreenToWorldPoint(screenpoint);
    //    }
    //}

    //public override void OnEndDrag(PointerEventData eventData)
    //{
    //    if (!DragHandler.Instance.EndSlot)
    //    {
    //        DragHandler.Instance.ItemBeingDragged = null;
    //        DragHandler.Instance.canvasGroup.blocksRaycasts = true;
    //        DragHandler.Instance.BeginSlot.item.transform.position = DragHandler.Instance.startPosition;
    //    }
    //    //AudioManager.instance.Play("Unclick");
    //}

    //public override void OnDrop(PointerEventData eventData)
    //{
    //    if (DragHandler.Instance.ItemBeingDragged == null)
    //        return;

    //    DragHandler.Instance.EndSlot = this;

    //    //  아이템이 이미 슬롯에 있으면
    //    if (item)
    //    {
    //        DragHandler.Instance.BeginSlot.item = item;
    //        DragHandler.Instance.BeginSlot.icon.sprite = item.icon;

    //        item = DragHandler.Instance.ItemBeingDragged;
    //    }

    //    //  없으면
    //    else
    //    {
    //        DragHandler.Instance.BeginSlot.item = null;
    //        DragHandler.Instance.BeginSlot.icon.sprite = null;

    //        item = DragHandler.Instance.ItemBeingDragged;
    //    }

    //    icon.color = new Color(0, 0, 0, 0);
    //    CharacterOnSector = Instantiate(item.ItemObject);

    //    DragHandler.Instance.ItemBeingDragged = null;
    //    DragHandler.Instance.BeginSlot = null;
    //    DragHandler.Instance.EndSlot = null;
    //}


    //public void PlaceCharacter(GameObject chr)
    //{
    //    CharacterOnSector = chr;
    //}
}
