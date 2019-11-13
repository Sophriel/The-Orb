using System.Collections;
using UnityEngine;
using Spine.Unity;

public class Tanker : PlayerCharacter
{

    void OnEnable()
    {
        cld = GetComponent<CapsuleCollider2D>();
        anm = GetComponent<SkeletonAnimation>();

        //  SetPosition(transform);  //  필드매니저에서 해줘야함
        SetClass(Class.Tanker);
        SetTier(Tier.Low);
        SetNumber(6);
        Init();
        SetIdle();
    }

    //  대기상태로 전환
    public override void SetIdle()
    {
        print("Set Idle");

        cStatus = Status.Idle;
        TargetEnemy = null;
        isAttacked = false;

        anm.AnimationName = "idle";

        iTween.RotateTo(gameObject,
            iTween.Hash("rotation", new Vector3(0, 0, 0), "time", 0.3f));
        StartCoroutine(Rader());

        //StopCoroutine(AttackCoroutine());
    }


    void Update()
    {
        switch (cStatus)
        {
            case Status.None:
                break;
            case Status.Idle:
                break;
            case Status.Attack:
                iTween.RotateUpdate(gameObject,
                    iTween.Hash("rotation", new Vector3(0, 0, GetAngle(transform.position, TargetEnemy.GetTransform().position)), "time", 0.5f));
                anm.AnimationState.Event += HandleEvent;
                break;
            case Status.Skill:
                break;
            case Status.Dead:
                anm.AnimationName = "death";
                anm.loop = false;
                break;
            default:
                break;
        }
    }


    //  콜백함수. 이벤트 발생시 호출됨
    void HandleEvent(Spine.TrackEntry trackEntry, Spine.Event e)
    {
        //  정확히 타격 판정이 발생하는 부분
        if (e.Data.Name == "hit" && !isAttacked)
        {
            print("event hitttttt");
            Attack();
            isAttacked = true;
        }

        //  공격 후
        if (e.Data.Name == "end attack" && isAttacked)
        {
            print("end Attack");

            //  적이 아직 사거리 내에 있는지 확인
            if (TargetEnemy.isAlive())
                CheckDistance(transform.position, TargetEnemy.GetTransform().position, Range);

            isAttacked = false;
        }
    }


    //  탐색 레이더. 나중에 부모클래스로 빼줘야하고 애니메이션 이름 통일
    public IEnumerator Rader()
    {
        AngleZ = 0.0f;

        while (true)
        {
            //  레이 쏘는 방향 뱅글뱅글
            AngleZ += 1440.0f * Time.deltaTime;
            Detector = (Quaternion.Euler(0.0f, 0.0f, AngleZ) * Vector3.down * Range)
                + transform.position;
            Debug.DrawLine(transform.position, Detector, Color.red, 0.1f);

            hit = Physics2D.Linecast(transform.position, Detector, 1 << LayerMask.NameToLayer("Enemy"));

            if (hit)
            {
                TargetEnemy = (IDamageable)hit.transform.GetComponent(typeof(IDamageable));

                //  유효한 타겟인가?
                if (TargetEnemy.isAlive())
                {
                    cStatus = Status.Attack;
                    anm.AnimationName = "attack 2";
                    yield break;
                }

                else
                    SetIdle();
            }

            else
            {
                if (cStatus == Status.Attack)
                {
                    SetIdle();
                }
            }

            yield return null;
        }
    }


    //  공격
    public override void Attack()
    {
        TargetEnemy.Damage(AttackDamage);

        if (!TargetEnemy.isAlive())
            SetIdle();


        //  스킬 카운트
        Count++;
    }


    //  피격
    public override void Damage(float damageTaken)
    {
        //  피격 당한상태가 아닌 경우, 살아있을때
        if (!isDamaged && cStatus != Status.Dead)
        {
            isDamaged = true;
            StartCoroutine(DamageCoroutine());

            //  데미지 가하기
            HP -= damageTaken;

        }

        if (HP <= 0)
        {
            //  상태 교체
            cStatus = Status.Dead;

            //  레이어 교체
            cld.enabled = false;
            gameObject.layer = 1 << LayerMask.NameToLayer("Default");

            //  애니메이션 실행
            anm.AnimationName = "death 2";
            iTween.FadeTo(gameObject, iTween.Hash("time", 1.0f, "alpha", 0.0f, "oncomplete", "Die"));

        }
    }


    //  사망
    public void Die()
    {
        gameObject.SetActive(false);
    }
}