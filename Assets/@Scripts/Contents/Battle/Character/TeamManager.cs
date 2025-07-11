using Data;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class TeamManager : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer[] _characterRenderer;
    [SerializeField] private SkinnedMeshRenderer[] sourceRenderers;
    [SerializeField] private Material[] sourceMaterials;
    [SerializeField] private CreatureData[] _creatureData; // 캐릭터 데이터
    [SerializeField] private BattleCharacter[] _battleCharacters; // 전투 캐릭터 배열

    public string[] CatDataKey;     // 선택된 고양이 어드레서블 키(프리펩 이름)ID
    public Material[] materials;


    //catdatakey만 먼저 가져와서 넣기


    private void Awake()
    {
        _battleCharacters = GetComponentsInChildren<BattleCharacter>();
        _characterRenderer = GetComponentsInChildren<SkinnedMeshRenderer>();
        _creatureData = new CreatureData[3];
        for (int k = 0; k < _creatureData.Length; k++)
            CatDataKey[k] = StageDataManager.Instance.PlayerCharacter[k].DataId;


        for (int k = 0; k < _battleCharacters.Length; k++)
        {
            _battleCharacters[k].AttackDamage = StageDataManager.Instance.PlayerCharacter[k].Atk;
            _battleCharacters[k].MaxHP = StageDataManager.Instance.PlayerCharacter[k].Hp;
            _battleCharacters[k].MoveSpeed = StageDataManager.Instance.PlayerCharacter[k].MoveSpeed;
            _battleCharacters[k].SkillID = StageDataManager.Instance.PlayerCharacter[k].Data.SkillIcon.Replace(".sprite", "");
        }
    }

    private void Start()
    {
        for (int i = 0; i < _characterRenderer.Length; i++)
        {
            _creatureData[i] = Managers.Data.CreatureDic[CatDataKey[i]];    //id별 데이터 등록
        }

        for (int i = 0; i < _characterRenderer.Length; i++)
        {
            LoadPrefab(i, _creatureData[i].PrefabLabel);//임시로 달아준 값 데이터 넘겨받을것
        }

        
    }

    public void LoadPrefab(int k, string catPrefabKey)
    {
        Addressables.LoadAssetAsync<GameObject>(catPrefabKey).Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                GameObject prefab = handle.Result;

                // 프리팹 안의 SkinnedMeshRenderer 가져오기
                sourceRenderers[k] = prefab.GetComponentInChildren<SkinnedMeshRenderer>();
                sourceMaterials = sourceRenderers[k].sharedMaterials;

                // 머티리얼이 2개일 것이라고 가정
                if (sourceMaterials.Length >= 2)
                {
                    // 복사해서 새 배열 생성
                    Material[] clonedMaterials = new Material[2];
                    clonedMaterials[0] = new Material(sourceMaterials[0]);
                    clonedMaterials[1] = new Material(sourceMaterials[1]);

                    // 전투씬 고양이에 적용
                    _characterRenderer[k].materials = clonedMaterials;
                }
                else
                {
                    Debug.LogWarning($"프리팹 렌더러 {k}에 머티리얼이 2개 이상 존재하지 않습니다.");
                }
                
            }
            else
            {
                Debug.LogError("프리팹 로드 실패: " + catPrefabKey);
            }
        };
    }
}
