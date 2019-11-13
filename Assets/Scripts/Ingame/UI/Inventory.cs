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

    //  ��ȯ
    public void AddItem()
    {
        //  �ڿ�ȥ�� ������� üũ

        //  ������ ������� üũ
        foreach (Slot i in Slots)
        {
            //  ������ ���
            if (!i.ItemOnSlot)
            {
                GameObject itemObj = Instantiate(CharacterPrefabs[Random.Range(0, CharacterPrefabs.Length -1)], Vector3.zero, Quaternion.identity);
                Item item = itemObj.GetComponent<Item>();

                i.ItemOnSlot = item;
                i.Icon.sprite = item.Icon;

                break;
            }
        }

        //  �ڿ������ �Ѿ���� ������ ������
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
