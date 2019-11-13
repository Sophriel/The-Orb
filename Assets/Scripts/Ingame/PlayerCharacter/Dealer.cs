using System.Collections;
using UnityEngine;
using Spine.Unity;

public class Dealer : PlayerCharacter
{
    public GameObject cArrow;
    private Arrow arr;

    void OnEnable()
    {
        cld = GetComponent<CapsuleCollider2D>();
        anm = GetComponent<SkeletonAnimation>();
        hpbarMask = GetComponentInChildren<SpriteMask>().gameObject;
        arr = cArrow.GetComponent<Arrow>();

        //  SetPosition(transform);  //  필드매니저에서 해줘야함
        SetClass(Class.Dealer);
        SetTier(Tier.Low);
        SetNumber(6);
        Init();
        SetIdle();
    }

    //  대기상태로 전환
    public override void SetIdle()
    {
        cStatus = Status.Idle;
        TargetEnemy = null;
        isAttacked = false;

        anm.AnimationName = "animation";
        anm.timeScale = 1.0f;

        iTween.RotateTo(gameObject,
            iTween.Hash("rotation", new Vector3(0, 0, 0), "speed", AttackSpeed * 300.0f));

        StartCoroutine(Rader());
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
                    iTween.Hash("rotation", new Vector3(0, 0, GetAngle(transform.position, TargetEnemy.GetTransform().position)),
                    "speed", AttackSpeed * 300.0f));
                anm.timeScale = AttackSpeed;
                anm.AnimationState.Event += HandleEvent;
                CheckDistance(transform.position, TargetEnemy.GetTransform().position, Range);
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
            anm.AnimationName = "attack2";

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
                    anm.AnimationName = "attack";
                    
                    yield break;
                }

                //else
                //{
                //    print("레이더 오류 아이들");
                //    SetIdle();
                //}
            }

            yield return null;
        }
    }


    //  공격
    public override void Attack()
    {
        //  화살 생성 후 마나 업
        CreateArrow();

        //  스킬 카운트
        Count++;
    }

    public void CreateArrow()
    {
        //Arrow arr = Instantiate(Arrow, transform.position, transform.rotation).GetComponent<Arrow>();
        arr.Init(this, TargetEnemy);
        cArrow.SetActive(true);
    }


    //  사망
    public void Die()
    {
        gameObject.SetActive(false);
    }
}