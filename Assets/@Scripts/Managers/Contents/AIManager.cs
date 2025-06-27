using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIManager : MonoBehaviour
{
    public static AIManager Instance;

    public List<AICharacter> AllCharacters  = new List<AICharacter>();
    public Material[] EmotionMaterials;
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
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

}
