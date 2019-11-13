using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
//랜덤 위치 생성 함수 만들기
// 몇마리 생성했는지 게임매니저에 보내고 적이 죽을시 남은갯수 보내기
public enum GameState
{
    start,
    ready,
    idle,
    wait,
}


public class EnemyManager : MonoBehaviour
{
    public List<GameObject> spawnEnemyObjs = new List<GameObject>();
    public List<Wavespawn> Spawners = new List<Wavespawn>();
    //게임 상황 판단
    public GameState nowGameState = GameState.idle;
    
    public int Round = 0;
    public int Normal = 0;
    public int Magic = 0;
    public int boss = 0;
    public int count = 0;
    public int amount = 0;
    public int WaveNum = 0;
    public int DeadEnemy = 0;
    public int normalcount = 0;
    //스크립트 연결
    void Awake()
    {
        //★ GameData가 아닌 gameplaymanager로 바뀔예정//
        GameData.Instance.enemyManager = this;
    }
    //스크립트 연결해제
    void OnDestroy()
    {
        //★ GameData가 아닌 gameplaymanager로 바뀔예정//
        GameData.Instance.enemyManager = null;
    }
    //XML 웨이브 데이터 
    void OnEnable()
    {
        //총 생성해야할 캐릭터
        SpawnNum();

        Spawners.AddRange(FindObjectsOfType<Wavespawn>());
        //★훗날 캐릭터가 늘어날걸 생각해서 배열로 정리할것
        foreach (Wavespawn i in Spawners)
        {
            i.InitobjectPools(spawnEnemyObjs[0],Normal / Spawners.Count);
            //ㅁ임시 아직 캐릭없음
            //i.InitobjectPools(spawnEnemyObjs[1], Magic);
            //i.InitobjectPools(spawnEnemyObjs[2], boss);
            //ㅁ
        }
    }


    //난이도
    public void LevelofDifficulty()
    {
        //노말



        //어려움 


    }

    void Update()
    {
        switch (nowGameState)
        {
            //준비상태일때
            case GameState.ready:
                break;
            //스폰
            case GameState.start:
                SpawnOrder();
                break;
                //대기상태
            case GameState.idle:
                break;          
        }
    }
    //게임 스폰 인원체크
    public void SpawnNum()
    {
        XmlDocument xDoc = new XmlDocument();
        xDoc.Load("./Assets/Resources/EnemyWaveData.xml");
        // XML파일 안에 EnemyWaveData란 XmlNode를 모두 읽어드린다.
        XmlNodeList nodeList = xDoc.DocumentElement.SelectNodes("/EnemyWaveDatas/Difficulty/Round");
            
            Normal = int.Parse(nodeList[nodeList.Count-1].SelectSingleNode("Normal").InnerText);
            print("Normal : " + Normal);
            //법사 캐릭(아직 캐릭터없음)
            Magic = int.Parse(nodeList[nodeList.Count - 1].SelectSingleNode("magic").InnerText);
            //보스캐릭 (아직 캐릭터없음)
            boss = int.Parse(nodeList[nodeList.Count - 1].SelectSingleNode("Boss").InnerText);
            print("Boss : " + boss);
    } 
    // 스폰 명령
    public void SpawnOrder()
    {
        XmlDocument xmldoc = new XmlDocument();
        xmldoc.Load("./Assets/Resources/EnemyWaveData.xml");
        // XML파일 안에 EnemyWaveData란 XmlNode를 모두 읽어드린다.
        XmlNodeList nodeList = xmldoc.DocumentElement.SelectNodes("/EnemyWaveDatas/Difficulty/Round");
        
        Round = int.Parse(nodeList[count].Attributes["round"].Value);
        normalcount = int.Parse(nodeList[count].SelectSingleNode("Normal").InnerText);
        // 마법사 캐릭 미구현
        int magiccount = int.Parse(nodeList[count].SelectSingleNode("magic").InnerText);
        //보스 캐릭 미구현
        int Bosscount = int.Parse(nodeList[count].SelectSingleNode("Boss").InnerText);

        //소환된 몬스터 합계
        amount = normalcount + magiccount + Bosscount;

        foreach (Wavespawn j in Spawners)
        { 
            j.SetupGameStateToSpawning(normalcount / Spawners.Count);
            print("라운드별 normal 숫자 :"+normalcount);
        }

        //count++;
        print("라운드 카운트 증가 :" + count);
        //상태 변경
        nowGameState = GameState.idle;
       
    }  
// 일단 죽은 에너미 갯수 처리
    public void AddDeadEnemyAndSoul(int getSoul = 0)
    {
        DeadEnemy++;
        print(DeadEnemy);


        //★normal 저거 전체로 값 바꿔야 함 
        //죽은 enemy와 소환된 normal 캐릭이 같을때
        if (DeadEnemy >= normalcount)
        {

            print("라운드 종료 ");
            //밤낮으로 바뀐다.
            //★ GameData가 아닌 gamemanager로 바뀔예정//
            GameManager.Instance.stageManager.EndNight();

            //증가
            count++;

            //라운드가 변하고 저장되어있는 enemy값 초기화
            DeadEnemy = 0;
            //SpawnOrder();

            
        }
    }






}// class

