using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using UnityEngine.UI;
using System.Xml;
using System.Xml.Serialization;
public enum EnemyState
{
    none,
    move,
    attack,
    dead,
    wait
}
public enum EnemyClass
{
    None = -1,
    Knight

}
public enum EnemyType
{
    None= -1,
    stats,
    nomal,
    hard

}

public class Enemy : MonoBehaviour, IDamageable
{
    public EnemyState currentState = EnemyState.none;// 적상태
    public EnemyClass enemyClass = EnemyClass.None;// 
    public EnemyType enemyType = EnemyType.None;//
    public float MoveSpeed = 1.0f;//이동속도
    public float currentHP;// 현재 체력
    public float maxHP;//최대체력
    public float AttackDamage ;//공격데미지
    public float damagedPower;// 파워
    public float AttackSpeed = 1.0f;// 공격 속도
    public float HitTime;// 공격횟수
    public float Range;//사거리
    public SkeletonAnimation skeleton; //스켈레톤 애니메이션
    public string cur_animation = "";//현재 실행중인 애니메이션 이름
    public Transform hpbarposition;// 체력이 깍였을때를 표시하기위한 체력바위치값
    public GameObject Hpbar;//체력이 가득있는 상태를 나타내는 오브젝트 
    Vector3 tempvector;//체력바 조정할 벡터..;
    
    protected Vector2 linevecter;// 적(플레이어캐릭) 탐지 범위
    protected Vector2 detecter;// 근처 에너미(왕국군) 캐릭 탐지
    protected RaycastHit2D playerchar;// 레이 캐스트
    protected RaycastHit2D enemychar;// 적군
    protected bool enableAttack = true;


    public void SetEnemyclass(EnemyClass input)
    {
       enemyClass = input;
    }

    public void SetType(EnemyType input)
    {
        enemyType = input;
    }

    private void Awake()
    {
        //애니메이션
        skeleton = GetComponent<SkeletonAnimation>();
        // 타입
        SetType(EnemyType.stats);
        //클래스
        SetEnemyclass(EnemyClass.Knight);

        currentState = EnemyState.move;
    }

    //  IDamageable
    public bool isAlive()
    {
        if (currentState == EnemyState.dead || currentState == EnemyState.none)
            return false;

        return true;
    }

    public Transform GetTransform()
    {
        return transform;
    }


    //훗날 초기화및 설정
    public void SaveData()
    {
        XmlDocument database = new XmlDocument();

        database.AppendChild(database.CreateXmlDeclaration("1.0", "UTF-8", "yes"));

        XmlNode root = database.CreateNode(XmlNodeType.Element, "Character", string.Empty);
        database.AppendChild(root);

        XmlNode child = database.CreateNode(XmlNodeType.Element, "Class", string.Empty);
        root.AppendChild(child);

        XmlElement stats = database.CreateElement("stats");
        child.AppendChild(stats);

        XmlElement HP = database.CreateElement("HP");
        HP.InnerText = "150";
        stats.AppendChild(HP);
        XmlElement AD = database.CreateElement("AttackDamage");
        AD.InnerText = " 10";
        stats.AppendChild(AD);
        XmlElement AS = database.CreateElement("AttackSpeed");
        AS.InnerText = "1.2";
        stats.AppendChild(AS);
        XmlElement HitTime = database.CreateElement("HitTime");
        HitTime.InnerText = "1";
        stats.AppendChild(HitTime);
        XmlElement Range = database.CreateElement("Range");
        Range.InnerText = "1";
        stats.AppendChild(Range);
        XmlElement aa = database.CreateElement("aa");
        aa.InnerText = "aa";

        //database.Save("./Assets/Resources/EnemyStat1.xml");
        
    
    }
    public void InitEnemy(Vector2 spawnposition)
    {
        //생성될 캐릭터의 위치
        transform.position = spawnposition;
        //애니메이션
        SetAnimation("walk", true, 1.0f);
        //상태
        currentState = EnemyState.move;

        //xml 불러오기
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load("./Assets/Scripts/Player Character/EnemyStats.xml");

        //해당 클래스 타입 값 
        XmlNode node = xmlDoc.SelectSingleNode("Character/" + enemyClass.ToString() + "/" + enemyType.ToString());


        maxHP = float.Parse(node.SelectSingleNode("HP").InnerText);
        currentHP = maxHP;
        AttackDamage = float.Parse(node.SelectSingleNode("AttackDamage").InnerText);
        AttackSpeed = float.Parse(node.SelectSingleNode("AttackSpeed").InnerText);
        HitTime = float.Parse(node.SelectSingleNode("HitTime").InnerText);
        //추후 추가 및 관리

        //hpbarposition.localPosition = Vector3.right * 1.0f + transform.position;
        //Count = 0;
        //MaxCount = int.Parse(node.SelectSingleNode("MaxCount").InnerText);

    }
    public bool IsEnabled()
    {
        return enabled;
    }
    void Update()
    {
        switch (currentState)
        {
            //none 상태일때
            case EnemyState.none:
                break;
            //move상태일때
            case EnemyState.move:
                Finder();
                break;
                //공격
            case EnemyState.attack:
                Attack();
                break;
                //대기
            case EnemyState.wait:
                Finder();
                break;
            case EnemyState.dead:
                break;
        }

    }
    //IEnumerator AttackCoroutine()
    //{
    //    float preDelay = 0.0f;
    //    float afterDelay = 0.0f;

    //    //  선딜레이
    //    while (preDelay < AttackSpeed)
    //    {
    //        preDelay += Time.deltaTime;

    //        yield return null;
    //    }

    //    //  공격부
    //    print("Hit");
    //    Attack();

    //    //  후딜레이
    //    while (afterDelay < AttackSpeed)
    //    {
    //        afterDelay += Time.deltaTime;

    //        yield return null;
    //    }

    //    Finder();
    //}

    // 캐릭터 탐색 
    public void Finder()
    {
        //탐지범위
        detecter = Vector3.up * 2.0f + transform.position;
        linevecter = Vector3.up * 1.4f + transform.position;// 탐지 범위 
        //이동
        iTween.MoveTo(this.gameObject, iTween.Hash("easeType", "linear", "Islocal", true, "y", 15f, "time", 10.0f));
        enemychar = Physics2D.Linecast(transform.position + Vector3.up * 2.0f, detecter, 1 << LayerMask.NameToLayer("Enemy"));
        playerchar = Physics2D.Linecast(transform.position, linevecter,1 << LayerMask.NameToLayer("Player Character"));
        //플레이어 발견시
        if (playerchar)
        {
            print("플레이어 캐릭터가 있다");
            //공격
            currentState = EnemyState.attack;
          
            FreezeEnemy();
            
        }
        else
        {

            if (currentState == EnemyState.attack)
            {
                ChangeStateToMove();
            }
                //예외처리 할거 생기면 적자
        }
        //왕국군 캐릭터 발견시
        if(enemychar)
        {

            currentState = EnemyState.wait;
            FreezeEnemy();

        }

        // 화면밖으로 멀리 나갈시 구현한 함수 하지만 필요없을듯
        //ResetEenmey();


    }
    //공격
    public virtual void Attack()
    {
        //찾기
        playerchar = Physics2D.Linecast(transform.position, linevecter, 1 << LayerMask.NameToLayer("Player Character"));
        if(playerchar)
        {
            SetAnimation("attack", true, 1.0f);//공격 애니이션 활성화
            //데미지 주기
            IDamageable damageTarget =
                (IDamageable)playerchar.transform.GetComponent(typeof(IDamageable));
            damageTarget.Damage(AttackDamage);
        }
        else
        {
            //이동
            ChangeStateToMove();

        }
    }


    //스파인 애니메이션 설정
    void SetAnimation(string name, bool loop, float speed)
    {
        if (name == cur_animation)
        {
            return;
        }
        else
        {
            skeleton.state.SetAnimation(0, name, loop).timeScale = speed;
            cur_animation = name;
        }
    }


    //데미지를 입었을때
    public void Damage(float damageTaken)
    {

        if (currentState == EnemyState.dead || currentState == EnemyState.none)
        {
            if (IsInvoking("ChangeStateToMove"))
            {
                CancelInvoke("ChangeStateToMove");
            }
            return;
        }
        currentHP -= damageTaken; //hp소진
        float value = (maxHP - currentHP) / maxHP;
        float percent = 1.000f - value;//위치를 조정하기위해 남은체력만큼 위치값 조정
        

        //tempvector = hpbarposition.localPosition;//마스크로 가릴 위치값
        tempvector.x = percent;//percent만큼의 값으로 x값 좌표 이동
        //hpbarposition.localPosition = tempvector;// 로컬포지션으로 다시 
        //체력이 0이거나 0보다 작다면 
        if (currentHP <= 0)
        {
            //HP= 0
            currentHP = 0;
            // 풀체력바와 마스크 위치값이 같아져서 피가 없게 보이게 만듬
            //hpbarposition.localPosition = Hpbar.transform.localPosition;
            // 함수 실행
            Dead();
            //변경사항
            //이벤트값으로 해결 
            //2초뒤 캐릭터가 초기화되며 소울을 주고 사라짐 
            Invoke("DeadEnd", 2.0f);

            //모든 에너미와 보스가 죽을때
            if (gameObject.tag == "Enemy")//이건 따로 둬서 디버프나 게임내 효과용으로 두자.
            {

                SetAnimation("die", false, 1.0f);
                
            }
        }
        else// 체력이 0이 아닐시
        {
            //예외처리?
           

        }
    }


    //죽었을때
    public void Dead()
    {
        FreezeEnemy();
        enableAttack = false;
        currentState = EnemyState.dead;
        SetAnimation("die", false, 1.0f);
    }
    // 이동상태로 변경
    void ChangeStateToMove()
    {
        currentState = EnemyState.move;
        SetAnimation("walk", true, 1.0f);
    }
    // idle 애니메이션만 재생하는 상태로 변경.
    //멈춤
    public void FreezeEnemy()
    {
        //이동멈추기
        iTween.Stop(gameObject);
    }
   //죽은뒤 드랍할 소울및 에너미 카운터 걧수 증가
    public void DeadEnd()
    {
       //★ 수정 makesoul값 바꾸기
        // 임의의 확률로 소울을 생성한다.
        int makesoul = UnityEngine.Random.Range(0, 10);
        if (makesoul >= 7)
        {
            //GameData.Instance.enemyManager.SpawnSoul(transform.position);
            ////처치한 적 숫자 및 획득한 소울 숫자 전달.
            //GameData.Instance.enemyManager.AddDeadEnemyAndSoul(10);
        }
        else
        {
            //처치한 적 숫자 전달.
            //★ GameData가 아닌 gameplaymanager로 바뀔예정 합칠시 오류 날듯
            GameData.Instance.enemyManager.AddDeadEnemyAndSoul();
        }
        // 적 캐릭터를 초기 위치로 이동시킨다.
        currentState = EnemyState.none;
        gameObject.SetActive(false);

    }
    //리셋및 방어가 뚤리고 마녀가 공격받고 에너미는 초기화 변경할 예정
    public void ResetEenmey()
    {
        if (transform.position.y == 15.0f)
        {
            currentState = EnemyState.none;
            gameObject.SetActive(false);
            //transform.position = transform.position =
            //    GameData.Instance.enemyManager.gameObjectPoolPosition.position;
        }
    }
}//class

