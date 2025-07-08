using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor.AddressableAssets.GUI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AI;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AIManager 
{
    public static AIManager Instance;

    public List<AICharacter> AllCharacters  = new List<AICharacter>();
    public Material[] EmotionMaterials;

    public void Init()
    {
        //await LoadEmotionMaterials();
        
       
        foreach (var character in AllCharacters)
        {
            ValidateNavMeshPosition(character);
        }

    }

    public void Register(AICharacter character)
    {
        if (character == null) return;
        if (!AllCharacters.Contains(character))
            AllCharacters.Add(character);
    }

    public void Unregister(AICharacter character)
    {
        if (character == null) return;
        AllCharacters.Remove(character);
    }

    private void RelocateToNearestNavMesh(AICharacter character)
    {
        if (!NavMesh.SamplePosition(character.transform.position, out NavMeshHit hit, 5f, NavMesh.AllAreas))
            return;

        character.transform.position = hit.position;
        character.nav.Warp(hit.position); // NavMeshAgent에 위치 강제 적용
    }

    private void ValidateNavMeshPosition(AICharacter character)
    {
        if (!character.nav.isOnNavMesh || character.nav.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            Debug.LogWarning($"[{character.name}] NavMesh에서 이탈! 복구 시도");
            RelocateToNearestNavMesh(character);
        }
    }

    private async Task LoadEmotionMaterials()
    {
        // Addressables 로드: 여러 개면 Label 또는 Asset Group으로 관리 권장
        var handle = Addressables.LoadAssetsAsync<Material>(
            "CharacterEmotion",  // Address 또는 Label
            null);               // 개별 로드 콜백(없으면 null)

        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            EmotionMaterials = handle.Result.ToArray();
            Debug.Log($"[AIManager] CharacterEmotion 로드 완료: {EmotionMaterials.Length}개");
        }
        else
        {
            Debug.LogError("[AIManager] CharacterEmotion 로드 실패!");
        }
    }

}
