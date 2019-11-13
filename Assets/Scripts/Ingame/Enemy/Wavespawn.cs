using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Spawnstate
{
    none,
    idle,
    spawning,
    ready
}
public partial class Wavespawn : MonoBehaviour
{
    public Spawnstate spawnstate = Spawnstate.none;
    public int CurrentNum = 0;
    //결과창이 나타날 때 적 캐릭터를 정지하는 목적으로 사용된다.
    //public event System.Action OnFreeze;
    public List<GameObject> GameObjectPool;

    private void OnEnable()
    {
        spawnstate = Spawnstate.idle;
    }
    //오브젝트 생성
    public void InitobjectPools(GameObject EnemyObj, int Count)
    {
        CreateGameObject(EnemyObj,Count,transform);//(적 오브젝트, 갯수, 부모게임오브젝트)
    }
    //오브젝트 생성
    void CreateGameObject(GameObject targetObj,
                        int amount,
                        Transform parent,
                        Vector3 localScale = default(Vector3))// 게임오브젝트 생성 메서드
    {
      
        for (int j = 0; j < amount; j++)
        {
            // 게임 오브젝트 생성.
            GameObject tempObj =
                Instantiate(targetObj, transform.position, Quaternion.identity, parent);

            if (localScale != Vector3.zero)
            {
                tempObj.transform.localScale = localScale;
            }

            // 게임 오브젝트를 게임 오브젝트 풀에 등록.
            GameObjectPool.Add(tempObj);//pool 딕셔너리에 obj 타켓오브젝트 이름과 풀 저장
        }
    }


    //스폰 상태로 상태 변경
    public void SetupGameStateToSpawning(int SpawnNum)
    {
        //아이들상태
        spawnstate = Spawnstate.idle;
        //초기화
        CurrentNum = 0;
        StartCoroutine(SpawnCoroutine(SpawnNum));
        print("SetupGame SpawnNum" + SpawnNum);
    }
    //스폰 코루틴
    IEnumerator SpawnCoroutine(int input)
    {
        // ★왕국군 종류가 늘어날거 생각하면 foreach로 소대별 편성을 해야함

        for (int i = 0; i < input; i++)
        {
            SpawnEnemy();
            print("input: " + input);
            yield return new WaitForSeconds(3.0f);
          
        }
    }

    // 스폰 생성
    public void SpawnEnemy()
    {
        //게임오브젝트 활성화
        GameObjectPool[CurrentNum].SetActive(true);
        Enemy currentEnemy = GameObjectPool[CurrentNum].GetComponent<Enemy>();//에너미 스크립트 받아오기//스탯
        currentEnemy.InitEnemy(transform.position);//xml
        
        CurrentNum++;
        print("SpawnEnemy currentNuM : " + CurrentNum);
        //생성된 게임오브젝트 갯수를 오버할시
        if (GameObjectPool[CurrentNum] == null)
        {
            CancelInvoke("SpawnEnemy");//ChckSpawnEnemy 캔슬
        }
    }
    
    void Update()
    {
        switch (spawnstate)
        {
            case Spawnstate.idle:
                break;
            case Spawnstate.spawning:
               // SetupGameStateToSpawning();
                break;
            case Spawnstate.ready:
                break;
                
        }


    }
    ////모든 에너미가 멈추는 함수 
    //void SetupAllEnemyFreeze()
    //{
    //    int j = 0;
    //    GameObject tempObj;
    //    Enemy tempEnemyScript;
    //    for (int i = 0; i < spawnEnemyObjs.Count; ++i)
    //    {
    //        j = 0;
    //        while (j < gameObjectPools[spawnEnemyObjs[i].name].LastIndex)
    //        {
    //            if (gameObjectPools[spawnEnemyObjs[i].name].GetObject(j, out tempObj))
    //            {
    //                tempEnemyScript = tempObj.GetComponent<Enemy>();
    //                OnFreeze += tempEnemyScript.FreezeEnemy;
    //            }
    //            ++j;
    //        }
    //    }
    //}
}
