using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item : MonoBehaviour
{
    //  ★실루엣이 들어가서 OnDrag에서 실루엣으로 대체되어야함
    public Sprite Icon;
    public Sprite Silhouette;

    public GameObject ItemObject;

    public PlayerCharacter ItemCharacter;

    void Start()
    {
        ItemCharacter = ItemObject.GetComponent<PlayerCharacter>();
    }

}
