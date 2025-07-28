using Data;
using System.Linq;
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
    [SerializeField] private BattleManager _battleManager; // 배틀 매니저
    [SerializeField] private EffectManager[] _effectManager; // 이펙트 매니저
    public string[] CatDataKey;     // 선택된 고양이 어드레서블 키(프리펩 이름)ID


    //catdatakey만 먼저 가져와서 넣기


    private void Awake()
    {
        _effectManager = GetComponentsInChildren<EffectManager>();
        _battleCharacters = GetComponentsInChildren<BattleCharacter>();

        // 3슬롯 고정
        _creatureData = new CreatureData[3];
        CatDataKey = new string[3];

        int playerCount = StageDataManager.Instance.PlayerCharacter?.Length ?? 0;

        for (int k = 0; k < 3; k++)
        {
            if (k < playerCount && StageDataManager.Instance.PlayerCharacter[k] != null)
            {
                var playerData = StageDataManager.Instance.PlayerCharacter[k];

                CatDataKey[k] = playerData.DataId;

                _battleCharacters[k].AttackDamage = playerData.Atk;
                _battleCharacters[k].MaxHP = playerData.Hp;
                _battleCharacters[k].MoveSpeed = 3.5f;
                _battleCharacters[k].SkillID = playerData.Data.SkillID.Replace(".sprite", "");
                _battleCharacters[k].CharID = playerData.DataId;

                _creatureData[k] = Managers.Data.CreatureDic[CatDataKey[k]];
                LoadPrefab(k, _creatureData[k].PrefabLabel);
            }
            else
            {
                // 데이터가 없으면 비활성화
                if (_battleCharacters.Length > k)
                {
                    _battleCharacters[k].BaseDie();
                    _battleCharacters[k].gameObject.SetActive(false);
                }
            }
        }
    }
    private void Start()
    {
        int count = Mathf.Min(_battleCharacters.Length, EffectHandler.Instance.effectManagers.Length);
        for (int i = 0; i < count; i++)
        {
            if (_battleCharacters[i].gameObject.activeSelf)
                _battleCharacters[i]._effectManager = EffectHandler.Instance.effectManagers[i];
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
