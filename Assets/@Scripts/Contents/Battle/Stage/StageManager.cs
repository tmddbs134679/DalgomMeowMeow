using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] StageSO[] stages;
    [SerializeField] private GameObject[] _parent; //적들이 생성될 부모 오브젝트들
    [SerializeField] private BattleCharacter[] _enemyStats;


    private int currentStageIndex;  //현재 스테이지 캐싱용
    private int _playerStageProceed; //플레이어의 진행 중인 스테이지(저장 가능한 정보)
    private int _playerSelectedStage;

    public int StageNumber;
    public int[] EnemyID;
    public float[] EnemySpawnRate;



    private void Awake() 
    {
        currentStageIndex = _playerSelectedStage;
        _playerStageProceed = Managers.Game.CurrentStage;    //플레이어의 진행 중인 스테이지(저장 가능한 정보)
        StageNumber = stages[currentStageIndex].StageNumber;    //스테이지 넘버 비교용(클리어 여부)
        //EnemyID = stages[currentStageIndex].enemydata.enemyID;    //적들 랜덤추출용
        EnemySpawnRate = stages[currentStageIndex].EnemySpawnRate;    //적들 확률
    }

    private void Start()
    {
        RandomSet();    //적들 랜덤 생성용 초기화
    }

    public void RandomSet() 
    {
        float totalWeight = 0f;

        foreach (float weight in EnemySpawnRate)
        {
            totalWeight += weight;
        }

        float randomValue = Random.Range(0f, totalWeight);

        float currentDum = 0f;
        for (int i = 0; i < EnemySpawnRate.Length; i++)
        {
            currentDum += EnemySpawnRate[i];
            if (randomValue <= currentDum)
            {
                //gameobjectInstantiate(stages[currentStageIndex].enemydata[i].PrefabLabel, Vector3.zero, Quaternion.identity, parent[i].transform);    //확률에 따라 적 생성
                SetStats(i); //적 스탯 설정
                return;
            }
        }
    }

    public void SetStats(int index)
    {
        _enemyStats[index] = _parent[index].GetComponent<BattleCharacter>();
        _enemyStats[index].Init(stages[currentStageIndex].enemydata[index]);
    }


    public void Reward()    //배틀매니저에서 호출
    {
        if(_playerStageProceed > currentStageIndex)
        {
            //반복 클리어 보상
        }
        else
        {
            //첫 클리어 보상
        }
    }
}
