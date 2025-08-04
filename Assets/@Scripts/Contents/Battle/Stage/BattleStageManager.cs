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
        SetAspectRatio(); //화면 비율 설정
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
                    _parent[k].GetComponent<BattleCharacter>().Init(Managers.Data.CreatureDic[stages.EnemyID[i]]);

                    break;
                }
            }
        }
    }
    

    public override void OnClick()
    {
        throw new System.NotImplementedException();
    }


    void SetAspectRatio()
    {
        float targetAspect = 20f / 9f;
        float windowAspect = (float)Screen.width / (float)Screen.height;
        float scaleHeight = windowAspect / targetAspect;

        Camera camera = Camera.main;

        if (scaleHeight < 1.0f)
        {
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;

            camera.rect = rect;
        }
        else
        {
            float scaleWidth = 1.0f / scaleHeight;

            Rect rect = camera.rect;

            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }
    }
}
