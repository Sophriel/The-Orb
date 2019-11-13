using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//sealed 한정자를 통해 해당클래스가 상속이 불가능하도록 조치
public sealed class GameData
{
    //싱글톤 인스턴스 저장
    private static volatile GameData unipueInstance;
    private static object _lock = new System.Object();

    //생성자
    private GameData() { }

    public EnemyManager enemyManager;

    //외부에서 접근가능하도록함
    public static GameData Instance
    {
        get
        {

            if(unipueInstance==null)
            {
                lock(_lock)
                {   
                    //lock으로 지정된 블록안의 코드를 하나의 쓰레드로만 접근하도록 만듬
                    if (unipueInstance == null)
                        unipueInstance = new GameData();
                }

            }
            return unipueInstance;
        }



    }
    
}

