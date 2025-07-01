using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class TeamManager : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer[] _characterRenderer;
    [SerializeField] private string catPrefabKey = "Chibi_Cat_00";     // 선택된 고양이 어드레서블 키(프리펩 이름)


    public Material[] materials;

    private void Awake()
    {
        _characterRenderer = GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    private void Start()
    {
        LoadPrefab();
    }



    public void LoadPrefab()
    {
        Addressables.LoadAssetAsync<GameObject>(catPrefabKey).Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                GameObject prefab = handle.Result;

                // 프리팹 안의 SkinnedMeshRenderer 가져오기
                SkinnedMeshRenderer[] sourceRenderers = prefab.GetComponentsInChildren<SkinnedMeshRenderer>();

                // 전투씬 고양이 수만큼 반복
                for (int i = 0; i < _characterRenderer.Length; i++)
                {
                    if (i >= sourceRenderers.Length) break;

                    Material[] sourceMaterials = sourceRenderers[i].sharedMaterials;

                    // 머티리얼이 2개일 것이라고 가정
                    if (sourceMaterials.Length >= 2)
                    {
                        // 복사해서 새 배열 생성
                        Material[] clonedMaterials = new Material[2];
                        clonedMaterials[0] = new Material(sourceMaterials[0]);
                        clonedMaterials[1] = new Material(sourceMaterials[1]);

                        // 전투씬 고양이에 적용
                        _characterRenderer[i].materials = clonedMaterials;
                    }
                    else
                    {
                        Debug.LogWarning($"프리팹 렌더러 {i}에 머티리얼이 2개 이상 존재하지 않습니다.");
                    }
                }
            }
            else
            {
                Debug.LogError("프리팹 로드 실패: " + catPrefabKey);
            }
        };
    }
}
