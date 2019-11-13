using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Slot[] Slots;
    public GameObject[] CharacterPrefabs;

    void Start()
    {
        Slots = GetComponentsInChildren<Slot>();
    }

    //  소환
    public void AddItem()
    {
        //  ★영혼이 충분한지 체크

        //  슬롯이 충분한지 체크
        foreach (Slot i in Slots)
        {
            //  슬롯이 비면
            if (!i.ItemOnSlot)
            {
                GameObject itemObj = Instantiate(CharacterPrefabs[Random.Range(0, CharacterPrefabs.Length -1)], Vector3.zero, Quaternion.identity);
                Item item = itemObj.GetComponent<Item>();

                i.ItemOnSlot = item;
                i.Icon.sprite = item.Icon;

                break;
            }
        }

        //  ★여기까지 넘어오면 아이템 가득참
    }

    public void RemoveItem(Item item)
    {
        Debug.Log("TODO: Remove item. " + item.name);
        //items.Remove(item);
    }

    //public bool HasItem(Item item)
    //{
    //    //return items.Contains(item);

    //    return true;
    //}

    //public int GetAmount()
    //{
    //    //return items.Count;

    //    return 0;
    //}
}
