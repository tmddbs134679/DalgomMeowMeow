using Data;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AI;
using UnityEngine.ResourceManagement.AsyncOperations;

public class TeamManager : MonoBehaviour
{
    [SerializeField] private CreatureData[] _creatureData; // 캐릭터 데이터
    [SerializeField] private BattleCharacter[] _battleCharacters; // 전투 캐릭터 배열
    [SerializeField] private RuntimeAnimatorController _catAnim;
    [SerializeField] private RuntimeAnimatorController _bearAnim;

    [SerializeField] private EffectManager[] _effectManager; // 이펙트 매니저
    public string[] CatDataKey;     // 선택된 고양이 어드레서블 키(프리펩 이름)ID


    //catdatakey만 먼저 가져와서 넣기


    private void Awake()
    {
        _effectManager = GetComponentsInChildren<EffectManager>();
        _battleCharacters = GetComponentsInChildren<BattleCharacter>();
        _creatureData = new CreatureData[3];
        for (int k = 0; k < _creatureData.Length; k++)
            CatDataKey[k] = StageDataManager.Instance.PlayerCharacter[k].DataId;


        for (int k = 0; k < _battleCharacters.Length; k++)
        {
            _battleCharacters[k].AttackDamage = StageDataManager.Instance.PlayerCharacter[k].Atk;
            _battleCharacters[k].MaxHP = StageDataManager.Instance.PlayerCharacter[k].Hp;
            _battleCharacters[k].MoveSpeed = StageDataManager.Instance.PlayerCharacter[k].MoveSpeed;
            _battleCharacters[k].SkillID = StageDataManager.Instance.PlayerCharacter[k].Data.SkillID.Replace(".sprite", "");
            _battleCharacters[k].CharID = StageDataManager.Instance.PlayerCharacter[k].DataId;
        }

        for (int i = 0; i < _battleCharacters.Length; i++)
        {
            _creatureData[i] = Managers.Data.CreatureDic[CatDataKey[i]];    //id별 데이터 등록
            LoadPrefab(i, _creatureData[i].PrefabLabel);//임시로 달아준 값 데이터 넘겨받을것
        }

    }
    private void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            _battleCharacters[i]._effectManager = EffectHandler.Instance.effectManagers[i]; //이펙트 매니저 등록
        }
    }


    public void LoadPrefab(int index, string prefabKey)
    {
        Addressables.LoadAssetAsync<GameObject>(prefabKey).Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                GameObject prefab = handle.Result;

                GameObject modelInstance = Instantiate(prefab, _battleCharacters[index].transform, false);  //생성
                modelInstance.name = prefab.name.Replace("(Clone)", "");


                var agent = modelInstance.GetComponent<NavMeshAgent>();//컴포넌트 제거
                if (agent != null) agent.enabled = false;

                var collider = modelInstance.GetComponent<Collider>();//컴포넌트 제거
                if (collider != null) collider.enabled = false;

                var ai = modelInstance.GetComponent<MonoBehaviour>(); //컴포넌트 제거
                if (ai != null) ai.enabled = false;


                _battleCharacters[index].CharacterObject = modelInstance.transform;//값 세팅
                _battleCharacters[index].PivotSet(); //피벗 설정 (손, 머리 등)

                modelInstance.AddComponent<AnimationEvent>();

                var animator = modelInstance.GetComponent<Animator>();
                if (animator != null)
                {
                    if (modelInstance.name.Contains("Cat"))
                        animator.runtimeAnimatorController = _catAnim;
                    else
                        animator.runtimeAnimatorController = _catAnim;

                    _battleCharacters[index].SetAnimation();
                }
            }
            else
            {
                Managers.Debug.LogError($"❌ 프리팹 로드 실패: {prefabKey}",Define.EDebugType.None);
            }
        };
    }


}
