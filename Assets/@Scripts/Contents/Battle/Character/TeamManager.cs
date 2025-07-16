using Data;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class TeamManager : MonoBehaviour
{
    [SerializeField] private CreatureData[] _creatureData; // 캐릭터 데이터
    [SerializeField] private BattleCharacter[] _battleCharacters; // 전투 캐릭터 배열
    [SerializeField] private AnimatorController _catAnim;
    [SerializeField] private AnimatorController _bearAnim;

    public string[] CatDataKey;     // 선택된 고양이 어드레서블 키(프리펩 이름)ID


    //catdatakey만 먼저 가져와서 넣기


    private void Awake()
    {
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
        }
    }

    private void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            _creatureData[i] = Managers.Data.CreatureDic[CatDataKey[i]];    //id별 데이터 등록
            LoadPrefab(i, _creatureData[i].PrefabLabel);//임시로 달아준 값 데이터 넘겨받을것
        }
    }

    public void LoadPrefab(int index, string prefabKey)
    {
        Addressables.LoadAssetAsync<GameObject>(prefabKey).Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                GameObject prefab = handle.Result;

                // 필요한 자식 가져오기
                Transform chibi = prefab.transform.GetChild(0); // "Chibi_Cat"
                Transform root = prefab.transform.GetChild(1);  // "root"

                if (chibi == null || root == null)
                {
                    Debug.LogError("❌ 필요한 자식 오브젝트를 찾지 못함");
                    return;
                }

                Transform parent = _battleCharacters[index].transform.GetChild(0);
                if (parent == null)
                {
                    Debug.LogError("❌ 부모 오브젝트 'Chibi_Cat_01'을 찾을 수 없습니다.");
                    return;
                }

                GameObject chibiInstance = Instantiate(chibi.gameObject, parent);
                chibiInstance.transform.localPosition = Vector3.zero;
                chibiInstance.transform.localRotation = Quaternion.identity;
                chibiInstance.name = chibiInstance.name.Replace("(Clone)", "");

                GameObject rootInstance = Instantiate(root.gameObject, parent);
                rootInstance.transform.localPosition = Vector3.zero;
                rootInstance.transform.localRotation = Quaternion.identity;
                chibiInstance.name = chibiInstance.name.Replace("(Clone)", "");

                if ( chibiInstance.name.Contains("Cat"))
                {
                    //고양이 애니메이션
                    parent.GetComponent<Animator>().runtimeAnimatorController = _catAnim;
                }
                else
                {
                    //곰 애니메이션
                    parent.GetComponent<Animator>().runtimeAnimatorController = _bearAnim;
                }
            }
            else
            {
                Debug.LogError($"❌ 프리팹 로드 실패: {prefabKey}");
            }
        };
    }


}
