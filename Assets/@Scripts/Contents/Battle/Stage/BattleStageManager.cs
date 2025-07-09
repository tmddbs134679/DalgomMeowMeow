using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStageManager : BaseObject
{
    public StageSO stages;
    [SerializeField] private GameObject[] _parent; //적들이 생성될 부모 오브젝트들


    public int currentStageIndex;  //현재 스테이지 캐싱용

    public int StageNumber;
    public float[] EnemySpawnRate { get; private set; }

    private void Awake() 
    {
        EnemyCharacter[] enemyCharacters = GetComponentsInChildren<EnemyCharacter>();
        _parent = new GameObject[enemyCharacters.Length];

        for (int i = 0; i < enemyCharacters.Length; i++)
        {
            _parent[i] = enemyCharacters[i].gameObject;
        }
        System.Array.Sort(_parent, (a, b) => string.Compare(a.name, b.name));

        stages = StageDataManager.Instance.SetStage(); //스테이지 데이터 가져오기

        EnemySpawnRate = stages.EnemySpawnRate;    //적들 확률


        RandomSet(); //적들 랜덤 생성
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
                    
                    Managers.Object.Spawn<BattleCharacter>(Vector3.zero, stages.EnemyID[i], _parent[k].transform);
                     
                    /*
                    GameObject enemy = Managers.Resource.Instantiate(Managers.Data.CreatureDic[stages[currentStageIndex].EnemyID[i]].PrefabLabel, _parent[k].transform , false); //적 생성
                    enemy.transform.SetParent(_parent[k].transform, false); //부모 오브젝트 설정
                    */
                    _parent[k].GetComponent<BattleCharacter>().Init(Managers.Data.CreatureDic[stages.EnemyID[i]]); //적 캐릭터 초기화

                    break;
                }
            }
        }
    }


    
    

    public override void OnClick()
    {
        throw new System.NotImplementedException();
    }
}
