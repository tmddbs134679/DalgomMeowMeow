using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : BaseObject
{
    [SerializeField] StageSO[] stages;
    [SerializeField] private GameObject[] _parent; //적들이 생성될 부모 오브젝트들


    private int currentStageIndex;  //현재 스테이지 캐싱용
    private int _playerStageProceed; //플레이어의 진행 중인 스테이지(저장 가능한 정보) json으로 세이브 로드 가능
    private int _playerSelectedStage;

    public int StageNumber;
    public string[] EnemyID;
    public float[] EnemySpawnRate;



    private void Awake() 
    {
        EnemyCharacter[] enemyCharacters = GetComponentsInChildren<EnemyCharacter>();
        _parent = new GameObject[enemyCharacters.Length];

        for (int i = 0; i < enemyCharacters.Length; i++)
        {
            _parent[i] = enemyCharacters[i].gameObject;
        }
        System.Array.Sort(_parent, (a, b) => string.Compare(a.name, b.name));

        currentStageIndex = _playerSelectedStage;
        _playerStageProceed = Managers.Game.CurrentStage;    //플레이어의 진행 중인 스테이지(저장 가능한 정보)
        StageNumber = stages[currentStageIndex].StageNumber;    //스테이지 넘버 비교용(클리어 여부)
        EnemySpawnRate = stages[currentStageIndex].EnemySpawnRate;    //적들 확률


        RandomSet(); //적들 랜덤 생성
    }

    private void Start()
    {
        
    }

    public void RandomSet() 
    {
        for (int k = 0; k < _parent.Length; k++)
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
                    /*
                     * var enemy = Managers.Object.Spawn<BattleCharacter>(Vector3.zero, stages[currentStageIndex].EnemyID[i]);
                     */
                    GameObject enemy = Managers.Resource.Instantiate(Managers.Data.CreatureDic[stages[currentStageIndex].EnemyID[i]].PrefabLabel, _parent[k].transform , false); //적 생성
                    enemy.transform.SetParent(_parent[k].transform, false); //부모 오브젝트 설정
                    SetData(Managers.Data.CreatureDic[stages[currentStageIndex].EnemyID[i]], k); //적 데이터 설정
                    break;
                }
            }
        }
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

    public void SetData(CreatureData data , int k)
    {
        _parent[k].GetComponent<BattleCharacter>().Init(data); //적 캐릭터 초기화
    }

    public override void OnClick()
    {
        throw new System.NotImplementedException();
    }
}
