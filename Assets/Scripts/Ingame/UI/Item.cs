using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item : MonoBehaviour
{
    //  �ڽǷ翧�� ���� OnDrag���� �Ƿ翧���� ��ü�Ǿ����
    public Sprite Icon;
    public Sprite Silhouette;

    public GameObject ItemObject;

    public PlayerCharacter ItemCharacter;

    void Start()
    {
        ItemCharacter = ItemObject.GetComponent<PlayerCharacter>();
    }

}
