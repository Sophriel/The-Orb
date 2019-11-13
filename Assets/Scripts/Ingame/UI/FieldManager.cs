using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : MonoBehaviour
{
    public GameObject[] list;
    public Sector[] slist;

    void Start()
    {
        list = new GameObject[transform.childCount];
        slist = new Sector[list.Length];

        for (int i = 0; i < transform.childCount; i++)
        {
            list[i] = transform.GetChild(i).gameObject;
            slist[i] = list[i].GetComponent<Sector>();
        }
    }

    void Update()
    {

    }
}
