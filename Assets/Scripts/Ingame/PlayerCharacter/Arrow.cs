using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

    private Dealer Parent;
    private IDamageable Target;

    public float Damage = 30.0f;
    public float Speed = 60.0f;

    public GameObject Effect;

    //  화살 초기화
    public void Init(Dealer parent, IDamageable target)
    {
        Parent = parent;
        Target = target;

        Damage = parent.AttackDamage;
        Speed = parent.AttackSpeed * 50.0f;
    }

    float GetAngle(Vector2 start, Vector2 end)
    {
        Vector2 v2 = start - end;
        return -Mathf.Atan2(v2.x, v2.y) * Mathf.Rad2Deg;
    }

    void OnEnable()
    {
        transform.position = Parent.GetTransform().position;
        transform.rotation = Parent.GetTransform().rotation;
        //transform.Rotate(0, 0, GetAngle(transform.position, Dest.position));

        //  날아가서 닿으면 타격
        iTween.MoveTo(gameObject, 
            iTween.Hash("position", Target.GetTransform().position, 
            "speed", Speed, "easetype", "linear", "oncomplete", "Hit"));
    }

    //  Trigger로 충돌
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Enemy"))
    //    {
    //        Hit();
    //    }
    //}

    void Hit()
    {
        //  이펙트
        Instantiate(Effect, transform.position, transform.rotation);

        //  데미징
        Target.Damage(Damage);

        if (!Target.isAlive())
        {
            print("화살 아이들");
            Parent.SetIdle();
        }

        //  화살 사라짐
        gameObject.SetActive(false);

        //  꽃아버리기
        //transform.SetParent(Dest);
    }
}
