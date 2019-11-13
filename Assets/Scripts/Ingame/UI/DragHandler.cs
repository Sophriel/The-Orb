using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragHandler : MonoBehaviour
{
    #region Singleton

    private static volatile DragHandler instance;
    private static object _lock = new System.Object();

    public static DragHandler Instance
    {
        get
        {
            if (instance == null)
                //  하나의 스레드로만 접근 가능하도록 DragHandler를 lock
                lock (_lock)
                    instance = new DragHandler();


            return instance;
        }
    }

    private void Awake()
    {
        ItemImage = GetComponent<Image>();
        canvasGroup = GetComponent<CanvasGroup>();

        instance = this;
        gameObject.SetActive(false);
    }

    #endregion

    public Item ItemBeingDragged;
    public Image ItemImage;  //  반투명 실루엣

    public Slot BeginSlot, DropSlot;
    public Vector3 StartPosition;

    public CanvasGroup canvasGroup;

    public bool isDragging()
    {
        if (ItemBeingDragged != null)
            return true;

        return false;
    }

    public void BeginDrag(Slot slot, Item item)
    {
        gameObject.SetActive(true);

        BeginSlot = slot;
        ItemBeingDragged = item;
        ItemImage.sprite = ItemBeingDragged.Silhouette;
        StartPosition = slot.transform.position;

        canvasGroup.blocksRaycasts = false;
    }

    public void Drag()
    {
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.0f));
    }

    public void Drop()
    {
        //  불가능한 위치일때
        iTween.MoveTo(gameObject, iTween.Hash("position", StartPosition, "time", 0.5f, "easetype", "easeInCubic", "oncomplete", "EndDrag"));

    }

    public void Drop(Slot slot)
    {
        //  가능한 위치일때
        DropSlot = slot;

        //  드랍 슬롯에 아이템이 있으면
        if (DropSlot.HasItem())
        {
            BeginSlot.ItemOnSlot = DropSlot.ItemOnSlot;
            BeginSlot.Icon.sprite = DropSlot.Icon.sprite;
        }

        //  없으면
        else
        {
            BeginSlot.ItemOnSlot = null;
            BeginSlot.Icon.sprite = null;
        }

        DropSlot.ItemOnSlot = ItemBeingDragged;
        DropSlot.Icon.sprite = ItemBeingDragged.Icon;


        //  공통
        ItemBeingDragged = null;
        BeginSlot = null;
        DropSlot = null;

        canvasGroup.blocksRaycasts = true;

        EndDrag();
    }

    public void EndDrag()
    {
        gameObject.SetActive(false);

    }
}
