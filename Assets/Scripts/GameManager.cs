using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

//  모든 씬의 매니저들이 상호간 데이터 이동하고 파일로 저장할때 사용하는 총괄 스크립트

public enum Difficulty
{
    New = 0,  //  게임 켰을때 저장된 데이터가 없으면
    Easy,
    Normal,
    Hard
}

public sealed class GameManager : MonoBehaviour
{

    #region Singleton

    private static volatile GameManager instance;
    private static object _lock = new System.Object();

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                //  하나의 스레드로만 접근 가능하도록 GameManager를 lock
                lock (_lock)
                    instance = new GameManager();


            return instance;
        }
    }

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    #endregion

    string savepath = "./SaveData.dat";
    FileStream savefile = null;
    StreamReader sr = null;
    string savedata =  "";

    public Difficulty PreStageDifficulty = Difficulty.New;
    public Difficulty Difficulty = Difficulty.New;
    public int Round = 0;
    public int Soul = 0;

    //  ★이전에 클리어한 데이터
    //  클래스랑 티어 15개

    //  ★게임이 진행중이였다면
    //  15개 유닛 + 얘네들 섹터까지

    public StageManager stageManager;

    void Start()
    {
        //  게임 데이터 로드 (없을시 생성)
        savefile = new FileStream(savepath, FileMode.OpenOrCreate);
        sr = new StreamReader(savefile);
        savedata = sr.ReadToEnd();

        if (savefile.Length > 1)
            LoadData();

        else
            SaveData();
    }

    #region string 관련 함수

    //  파일에 텍스트 추가하는 함수
    private void AddText(FileStream fs, string value)
    {
        byte[] info = new UTF8Encoding(true).GetBytes(value);
        fs.Write(info, 0, info.Length);
    }

    //  특정 문자열과 문자열 사이의 문자열을 반환
    private string SubstringBetween(string str, string start, string end)
    {
        //  start의 시작 index
        int a = str.IndexOf(start);
        //  end의 시작 index (start보다 뒤에서 탐색)
        int b = str.IndexOf(end, a + start.Length);

        string s = str.Substring(a + start.Length, b - (a + start.Length));

        return s;
    }

    #endregion

    #region 데이터 관련 함수

    public void SaveData()
    {
        FileStream newSaveFile = new FileStream(savepath, FileMode.CreateNew);

        AddText(savefile, "[Stage]\n");
        AddText(savefile, "PreStageDifficulty = " + (int)PreStageDifficulty + "\n");
        AddText(savefile, "Difficulty = " + (int)Difficulty + "\n");
        AddText(savefile, "Round = " + Round + "\n");
        AddText(savefile, "Soul = " + Soul + "\n");

        //  ★필드랑 인벤토리 저장
        AddText(savefile, "[Field]\n");
        AddText(savefile, "[Inventory]\n");
    }

    private void LoadData()
    {
        PreStageDifficulty = (Difficulty)int.Parse(SubstringBetween(savedata, "\nPreStageDifficulty = ", "\n"));
        Difficulty = (Difficulty)int.Parse(SubstringBetween(savedata, "\nDifficulty = ", "\n"));
        Round = int.Parse(SubstringBetween(savedata, "\nRound = ", "\n"));
        Soul = int.Parse(SubstringBetween(savedata, "\nSoul = ", "\n"));
    }

    #endregion
}
