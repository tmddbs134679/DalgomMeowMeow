using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using Scripts.Contents.AI.FSM.State;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AIController : BaseController<AICharacter>
{
    protected AICharacter aiCharacter;

    private float time = 0;

    public AIController(BaseState<AICharacter> initState, AICharacter aiCharacter)
        : base(initState, aiCharacter)
    {
        this.aiCharacter = aiCharacter;
        aiCharacter.characterAction.OnAction += OnActionPerformed;
    }

    public void Dispose()
    {
        if (aiCharacter != null && aiCharacter.characterAction != null)
        {
            aiCharacter.characterAction.OnAction -= OnActionPerformed;
        }
    }


    private void OnActionPerformed(Define.EAIState action) // 상태 변경에 따른 행동 처리
    {

        Vector3 targetPos = FindNearestBuilding(action);


        var stateMap = new Dictionary<Define.EAIState, string>
        {
            { Define.EAIState.Cooking, nameof(CharacterCookState) },
            { Define.EAIState.Playing, nameof(CharacterPlayState) },
            { Define.EAIState.Resting, nameof(CharacterRestState) },
            { Define.EAIState.Farming, nameof(CharacterFarmingState) },
            { Define.EAIState.Building, nameof(CharacterBuildingState) }
        };

        var moveState = new AIMoveToTargetState(targetPos, () =>
        {
            if (stateMap.TryGetValue(action, out string nextState))
            {
                ChangeState(nextState);
            }
        });

        RegisterState(moveState, aiCharacter);
        ChangeState(moveState.GetType().Name); // 이동 상태로 전환
    }


    private Vector3 FindNearestBuilding(Define.EAIState action) //  행동에 따른 가장 가까운 건물 위치 찾기
    {
        // 추후 BuildingManager 등에서 가져오면 됨
        if (action == Define.EAIState.Cooking)
        {

            var nearestCookingBuilding = FindBuilding(BuildingType.Cooking);
            aiCharacter.currentBuilding = nearestCookingBuilding;
            if (nearestCookingBuilding == null)
            {
                Debug.LogWarning("해당 건물을 찾을 수 없습니다.");
                return aiCharacter.transform.position;
            }
            return nearestCookingBuilding.transform.position;
        }

        if (action == Define.EAIState.Farming)
        {
            var nearestCookingBuilding = FindBuilding(BuildingType.Farm);
            aiCharacter.currentBuilding = nearestCookingBuilding;
            if (nearestCookingBuilding == null)
            {
                Debug.LogWarning("해당 건물을 찾을 수 없습니다.");
                return aiCharacter.transform.position; 
            }
            return nearestCookingBuilding.transform.position;
        }


        if (action == Define.EAIState.Resting)
        {
            var nearestCookingBuilding = FindBuilding(BuildingType.Resting);
            aiCharacter.currentBuilding = nearestCookingBuilding;
            if (nearestCookingBuilding == null)
            {
                Debug.LogWarning("해당 건물을 찾을 수 없습니다.");
                return aiCharacter.transform.position; 
            }
            return nearestCookingBuilding.transform.position;
        }
        return Vector3.zero; 
    }

    public BuildingBase FindBuilding(BuildingType type) // 가장 가까운 건물 찾기
    {
        var allAssignedBuildings = new HashSet<BuildingBase>(
        AIManager.Instance.AllCharacters
            .Select(c => c.currentBuilding)
            .Where(b => b != null)
             );  

         return BuildingManager.Instance._buildings.Where(x => x.BuildingData.BuildingType == type && !allAssignedBuildings.Contains(x)).
                OrderBy(x => Vector3.Distance(x.transform.position, aiCharacter.transform.position)).
                FirstOrDefault();
    }

    public void Move(Vector3 buildingPosition) // 이동 메소드
    {
        aiCharacter.nav.ResetPath();
        aiCharacter.nav.SetDestination(buildingPosition);
    }

    public void PatrolMove(float _patrolDelay) // 순찰 이동 메소드
    {
        if (aiCharacter.nav.isPathStale)
        {
            aiCharacter.nav.ResetPath();
        }
        else
        {
            time += Time.deltaTime;
            if (time < _patrolDelay)
                return;
            Patrol();
            time = 0f;
        }
    }

    private void Patrol()
    {
        aiCharacter.nav.SetDestination(RandomDestination(aiCharacter.transform.position, new Vector3(10f, 0f, 10f)));
    }

    private Vector3 RandomDestination(Vector3 curPos, Vector3 halfExtents, int areaMask = NavMesh.AllAreas)
    {
        for (int i = 0; i < 10; i++)
        {
            var random = curPos + new Vector3(
                Random.Range(-halfExtents.x, halfExtents.x),
                0f,
                Random.Range(-halfExtents.z, halfExtents.z)
            );

            if (NavMesh.SamplePosition(random, out var hit, 1f, areaMask))
                return hit.position;
        }
        return curPos;
    }
}






