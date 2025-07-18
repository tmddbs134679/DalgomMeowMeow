
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AIManager
{
    public static AIManager Instance;

    public List<AICharacter> AllCharacters = new List<AICharacter>();
    public Material[] EmotionMaterials;

    public void Init()
    {

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

    public void ValidateNavMeshPosition(AICharacter character)
    {
        if (!character.nav.isOnNavMesh || character.nav.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            Managers.Debug.Log("[AIManager] NavMesh 위치가 유효하지 않음. 재위치 조정 중...", Define.EDebugType.AI);
            RelocateToNearestNavMesh(character);
        }
    }

    public void AllRelocateToNearestNavMesh()
    {
        foreach (AICharacter character in AllCharacters)
        {
            RelocateToNearestNavMesh(character);
        }

    }

}
