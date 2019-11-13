using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum StageStatus
{
    None = -1,
    Day,
    Night
}

public class StageManager : MonoBehaviour
{
    public GameObject Fade;
    public Material FadeMaterial;

    public StageStatus sStatus = StageStatus.None;

    //  매니저님들
    //  ★public Summoner MainCharacter;
    public FieldManager fieldManager;
    public EnemyManager enemyManager;

    public TextMeshPro RoundNumber;
    public int MaxRound = 15;

    public Light UIDirect, UISpot, ObjectDirect, ObjectSpot;
    public Color DayColor, NightColor, DarkColor;
    public GameObject Day, Night;
    public float Daytime = 20.0f;

    #region 페이딩

    public void FadeIn(float time)
    {
        FadeMaterial.color = Color.black;

        iTween.ColorTo(Fade, iTween.Hash("alpha", 0.0f,
            "easetype", "easeInSine", "time", time));
    }

    public void FadeOut(float time)
    {
        FadeMaterial.color = Color.clear;

        iTween.ColorTo(Fade, iTween.Hash("alpha", 1.0f,
            "easetype", "easeInSine", "time", time));
    }

    #endregion

    void Start()
    {
        GameManager.Instance.stageManager = this;

        fieldManager = FindObjectOfType<FieldManager>();
        enemyManager = FindObjectOfType<EnemyManager>();

        RoundNumber.text = GameManager.Instance.Round.ToString();

        //DayColor = new Color(252.0f, 255.0f, 231.0f, 255.0f);
        //NightColor = new Color(101.0f, 148.0f, 188.0f, 255.0f);
        //DarkColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);

        FadeMaterial = Fade.GetComponent<SpriteRenderer>().material;
        FadeIn(1.0f);

        StartDay();
    }

    void Update()
    {
        switch (sStatus)
        {
            case StageStatus.None:
                break;
            case StageStatus.Day:
                break;
            case StageStatus.Night:
                //  ★얻는 소울 인벤토리로 보내주기
                break;
            default:
                break;
        }
    }

    #region 낮밤 변경함수

    public void StartDay()
    {
        sStatus = StageStatus.Day;

        //  낮 종료되면 밤으로 전환
        iTween.MoveAdd(gameObject, iTween.Hash("position", Vector3.zero,
            "time", Daytime, "oncomplete", "EndDay"));

        //  아이콘 회전
        iTween.RotateAdd(Day,
            iTween.Hash("amount", new Vector3(0.0f, 0.0f, -180.0f), "time", Daytime, "easetype", "linear"));
        iTween.FadeTo(Day,
            iTween.Hash("alpha", 0.0f, "time", Daytime, "easetype", "easeInCubic"));
        iTween.RotateAdd(Night,
            iTween.Hash("amount", new Vector3(0.0f, 0.0f, -180.0f), "time", Daytime, "easetype", "linear"));
        iTween.FadeTo(Night,
            iTween.Hash("alpha", 1.0f, "time", Daytime, "easetype", "easeInCubic"));

        //  UI 디렉셔널, 스팟 라이팅
        iTween.ColorTo(UIDirect.gameObject,
            iTween.Hash("color", DarkColor, "time", Daytime, "easetype", "easeInCubic"));
        iTween.ColorTo(UISpot.gameObject,
            iTween.Hash("color", NightColor, "time", Daytime, "easetype", "easeInCubic"));

        //  오브젝트 디렉셔널, 스팟 라이팅
        iTween.ColorTo(ObjectDirect.gameObject,
            iTween.Hash("color", DarkColor, "time", Daytime, "easetype", "easeInCubic"));
        iTween.ColorTo(ObjectSpot.gameObject,
            iTween.Hash("color", NightColor, "time", Daytime, "easetype", "easeInCubic"));


        //  게임 클리어
        if (GameManager.Instance.Round > MaxRound)
            StageClear();
    }

    public void SkipDay()
    {
        //  연출 전부 스킵
        iTween.Stop(gameObject, true);

        iTween.RotateAdd(Day,
            iTween.Hash("amount", new Vector3(0.0f, 0.0f, -(180.0f + Day.transform.rotation.eulerAngles.z)), "time", 0.5f, "easetype", "linear"));
        iTween.FadeTo(Day,
            iTween.Hash("alpha", 0.0f, "time", 0.5f, "easetype", "easeInCubic"));
        iTween.RotateAdd(Night,
            iTween.Hash("amount", new Vector3(0.0f, 0.0f, -(0.0f + Night.transform.rotation.eulerAngles.z)), "time", 0.5f, "easetype", "linear"));
        iTween.FadeTo(Night,
            iTween.Hash("alpha", 1.0f, "time", 0.5f, "easetype", "easeInCubic"));

        iTween.ColorTo(UIDirect.gameObject,
            iTween.Hash("color", DarkColor, "time", 0.5f, "easetype", "easeInCubic"));
        iTween.ColorTo(UISpot.gameObject,
            iTween.Hash("color", NightColor, "time", 0.5f, "easetype", "easeInCubic"));

        iTween.ColorTo(ObjectDirect.gameObject,
            iTween.Hash("color", DarkColor, "time", 0.5f, "easetype", "easeInCubic"));
        iTween.ColorTo(ObjectSpot.gameObject,
            iTween.Hash("color", NightColor, "time", 0.5f, "easetype", "easeInCubic"));

        EndDay();
    }

    public void EndDay()
    {
        //  ★필드매니저 배치 락걸기


        StartNight();
    }

    public void StartNight()
    {
        sStatus = StageStatus.Night;

        //  ★에너미매니저 웨이브 시작

        EndNight();
    }

    public void EndNight()
    {
        GameManager.Instance.Round++;
        RoundNumber.text = GameManager.Instance.Round.ToString();

        //  게임 데이터 저장
        GameManager.Instance.SaveData();

        //  종료되면 낮으로 전환
        iTween.MoveAdd(gameObject, iTween.Hash("position", Vector3.zero,
           "time", 1.0f, "oncomplete", "StartDay"));

        //  아이콘 회전
        iTween.RotateAdd(Day,
            iTween.Hash("amount", new Vector3(0.0f, 0.0f, -180.0f), "time", 1.0f, "easetype", "linear"));
        iTween.FadeTo(Day,
            iTween.Hash("alpha", 1.0f, "time", 1.0f, "easetype", "easeInCubic"));
        iTween.RotateAdd(Night,
            iTween.Hash("amount", new Vector3(0.0f, 0.0f, -180.0f), "time", 1.0f, "easetype", "linear"));
        iTween.FadeTo(Night,
            iTween.Hash("alpha", 0.0f, "time", 1.0f, "easetype", "easeInCubic"));

        //  UI 디렉셔널, 스팟 라이팅
        iTween.ColorTo(UIDirect.gameObject,
            iTween.Hash("color", DayColor, "time", 1.0f, "easetype", "easeInCubic"));
        iTween.ColorTo(UISpot.gameObject,
            iTween.Hash("color", DarkColor, "time", 1.0f, "easetype", "easeInCubic"));

        //  오브젝트 디렉셔널, 스팟 라이팅
        iTween.ColorTo(ObjectDirect.gameObject,
            iTween.Hash("color", DayColor, "time", 1.0f, "easetype", "easeInCubic"));
        iTween.ColorTo(ObjectSpot.gameObject,
            iTween.Hash("color", DarkColor, "time", 1.0f, "easetype", "easeInCubic"));


        //  ★필드매니저 배치 락 해제

    }

    #endregion

    //  ★에너미 매니저에서 받아온 소울 인벤토리로 지급
    public void EarnSoul()
    {

    }

    //  ★라운드 끝나면 소울 이자 지급
    public void SoulInterest()
    {

    }

    #region 스테이지 종료 함수

    public void StageClear()
    {
        iTween.Stop();

        GameManager.Instance.PreStageDifficulty = GameManager.Instance.Difficulty;
        GameManager.Instance.Difficulty = Difficulty.New;
        GameManager.Instance.Round = 0;

        GameManager.Instance.SaveData();

        GameManager.Instance.stageManager = null;

        //  ★얼라이브 창 띄우기

    }

    public void StageFail()
    {
        iTween.Stop();

        GameManager.Instance.PreStageDifficulty = Difficulty.New;
        GameManager.Instance.Difficulty = Difficulty.New;
        GameManager.Instance.Round = 0;

        GameManager.Instance.SaveData();

        GameManager.Instance.stageManager = null;

        //  ★유다희 창 띄우기
    }

    #endregion
}
