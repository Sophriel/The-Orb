//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    //  나중에 사망한 상태인지를 받아오게 수정
    bool isAlive();

    Transform GetTransform();

    void Damage(float damageTaken);
}