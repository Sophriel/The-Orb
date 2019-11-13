using System.Collections;
using UnityEngine;
using System.Xml;
using Spine.Unity;

#region 소환수 기본 분류

public enum Status
{
    None = -1,
    Idle,
    Attack,
    Skill,
    Dead
}

public enum Class
{
    None = -1,
    Tanker,
    Dealer,
    Healer
}

public enum Tier
{
    None = -1,
    Low,
    Common,
    Rear,
    Unique
}

#endregion

public class PlayerCharacter : MonoBehaviour, IDamageable
{
    #region 소환수 기본 변수
  
    //  컴포넌트 초기화
    protected RaycastHit2D hit;
    protected CapsuleCollider2D cld;
    protected SkeletonAnimation anm;
    protected GameObject hpbarMask;

    //  생성 시 초기화
    public Status cStatus = Status.None;
    public Class cClass = Class.None;
    public Tier cTier = Tier.None;
    public int Number;

    //  XML에 의한 초기화
    public float HP;
    public float MaxHP;
    public bool isDamaged = false;
    public float HitTime = 0.5f;  //  피격 후 무적시간

    public float Range;  //  사거리
    protected IDamageable TargetEnemy;  //  공격 대상
    protected Vector2 Detector;  //  탐지 벡터
    protected float AngleZ;

    protected bool isAttacked = false;
    public float AttackDamage;  //  딜
    public float AttackSpeed;  //  공격 애니메이션 속도

    public int Count;  //  스킬 쓰기위한 마나
    public int MaxCount;  //  마나 100

    #endregion

    #region IDamageable 인터페이스 구현

    //  IDamageable
    public bool isAlive()
    {
        if (cStatus == Status.Dead || cStatus == Status.None)
            return false;

        return true;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public virtual void Damage(float damageTaken)
    {

    }

    //  피격시 HitTime초 만큼 무적
    public IEnumerator DamageCoroutine()
    {
        float delay = 0.0f;

        while (delay < HitTime)
        {
            delay += Time.deltaTime;

            yield return null;
        }

        isDamaged = false;

    }

    #endregion

    #region 초기화 함수

    //  배치할때 불러오는 함수
    public void SetPosition(Transform input)
    {
        transform.position = input.position;
    }

    public void SetClass(Class input)
    {
        cClass = input;
    }

    public void SetTier(Tier input)
    {
        cTier = input;
    }

    public void SetNumber(int num)
    {
        Number = num;

        //  구역 넘버와 일치하면 시너지
        //  Synerge();
    }


    //  데이터 저장 (사용안함)
    public void SaveData()
    {
        XmlDocument database = new XmlDocument();

        database.AppendChild(database.CreateXmlDeclaration("1.0", "utf-8", "yes"));

        XmlNode root = database.CreateNode(XmlNodeType.Element, "Character", string.Empty);
        database.AppendChild(root);

        XmlNode child = database.CreateNode(XmlNodeType.Element, "Class", string.Empty);
        root.AppendChild(child);

        XmlElement tier = database.CreateElement("Low");
        child.AppendChild(tier);

        XmlElement hp = database.CreateElement("hp");
        hp.InnerText = "150";
        tier.AppendChild(hp);

        XmlElement AS = database.CreateElement("AttackSpeed");
        AS.InnerText = "1.2";
        tier.AppendChild(AS);

        //database.Save("./Assets/Scripts/Player Character/Character data.xml");
    }

    //  스탯 초기화
    public void Init()
    {
        cStatus = Status.Idle;
        anm.initialSkinName = cTier.ToString();
        anm.Skeleton.SetSkin(cTier.ToString());

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load("./Assets/Scripts/Ingame/PlayerCharacter/Character data.xml");

        XmlNode node = xmlDoc.SelectSingleNode("Character/" + cClass.ToString() + "/" + cTier.ToString());

        MaxHP = int.Parse(node.SelectSingleNode("MaxHP").InnerText);
        HP = MaxHP;
        HitTime = float.Parse(node.SelectSingleNode("HitTime").InnerText);
        Range = float.Parse(node.SelectSingleNode("Range").InnerText);
        AttackDamage = float.Parse(node.SelectSingleNode("AttackDamage").InnerText);
        AttackSpeed = float.Parse(node.SelectSingleNode("AttackSpeed").InnerText);
        Count = 0;
        MaxCount = int.Parse(node.SelectSingleNode("MaxCount").InnerText);
    }

    #endregion

    #region 소환수 기본 함수

    //  Idle로 상태전환하는 함수.
    public virtual void SetIdle()
    {

    }

    //  각 구하기
    public float GetAngle(Vector2 start, Vector2 end)
    {
        Vector2 v2 = start - end;
        return -Mathf.Atan2(v2.x, v2.y) * Mathf.Rad2Deg;
    }

    //  ★IDamageable의 Damage함수를 대체해야함!
    //  HP에 변화를 주고 이에 따른 처리
    public void SetHP(float variation)
    {
        //HP += variation;

        //  HP바 scale 곱해서 크기 가변적으로 * HP 비율만큼
        float maskpositionX = 8.1f * hpbarMask.transform.localScale.x * (1.0f - ((MaxHP - HP) / MaxHP));
        iTween.MoveTo(hpbarMask, iTween.Hash("position", Vector3.right * maskpositionX,
            "time", 0.1f, "islocal", true));

        //  피격 당한상태가 아닌 경우, 살아있을때
        if (!isDamaged && cStatus != Status.Dead)
        {
            isDamaged = true;
            StartCoroutine(DamageCoroutine());

            //  데미지 가하기
            HP += variation;

        }

        if (HP <= 0)
        {
            HP = 0;

            //  상태 교체
            cStatus = Status.Dead;

            //  레이어 교체
            cld.enabled = false;
            gameObject.layer = 1 << LayerMask.NameToLayer("Default");

            //  애니메이션 실행
            anm.timeScale = 1.0f;
            iTween.FadeTo(gameObject, iTween.Hash("time", 1.0f, "alpha", 0.0f, "oncomplete", "Die"));

        }
    }

    //  공격 대상 지속 탐지
    public bool CheckDistance(Vector2 start, Vector2 target, float range)
    {
        // 딜러부터 왕국군까지 가는 방향벡터 만들어서 range만큼 길이 뽑고 라인캐스트
        Vector2 dest = Quaternion.Euler(0, 0, GetAngle(start, target)) * Vector2.down * range
            + transform.position;
        Debug.DrawLine(start, dest, Color.red, 0.3f);

        hit = Physics2D.Linecast(start, dest, 1 << LayerMask.NameToLayer("Enemy"));

        //  탐지한 대상이 이전과 다를경우
        if (hit && (IDamageable)hit.transform.GetComponent(typeof(IDamageable)) != TargetEnemy)
        {
            print("ReTarget");

            //  새로 탐지한 적 공격
            TargetEnemy = (IDamageable)hit.transform.GetComponent(typeof(IDamageable));
        }

        //  대상이 범위 밖으로 나갈경우
        else if (!hit)
        {
            print("측정 아이들");
            SetIdle();
        }

        return false;
    }

    public virtual void Attack()
    {

    }

    //  사용안함.
    IEnumerator AttackCoroutine()
    {
        float preDelay = 0.0f;
        float afterDelay = 0.0f;

        //  선딜레이
        while (preDelay < AttackSpeed)
        {
            preDelay += Time.deltaTime;

            yield return null;
        }


        //  공격부
        //print("Hit");
        //Attack();

        //  후딜레이
        while (afterDelay < AttackSpeed)
        {
            afterDelay += Time.deltaTime;

            yield return null;
        }

        //FindEnemy();
    }

    #endregion
}