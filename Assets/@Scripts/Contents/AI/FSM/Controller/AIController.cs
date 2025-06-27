using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Scripts.Contents.AI.FSM.State;

public class AIController : BaseController<AICharacter>
{
    protected AICharacter aiCharacter;
    private float patrolTimer = 0f;

    public AIController(BaseState<AICharacter> initState, AICharacter aiCharacter)
        : base(initState, aiCharacter)
    {
        this.aiCharacter = aiCharacter;
        aiCharacter.characterAction.OnAction += OnActionPerformed;
    }

    public void Dispose()
    {
        if (aiCharacter?.characterAction != null)
        {
            aiCharacter.characterAction.OnAction -= OnActionPerformed;
        }
    }

    #region FSM - Action 처리

    private void OnActionPerformed(Define.EAIState action)
    {
        if (action == Define.EAIState.Idle)
        {
            ChangeState(nameof(CharacterIdleState));
            return;
        }
        var targetPos = FindNearestBuilding(action);

        var stateMap = new Dictionary<Define.EAIState, string>
        {
            { Define.EAIState.MoveTo, nameof(AIMoveToTargetState) },
            { Define.EAIState.Cooking, nameof(CharacterCookState) },
            { Define.EAIState.Playing, nameof(CharacterPlayState) },
            { Define.EAIState.Resting, nameof(CharacterRestState) },
            { Define.EAIState.Farming, nameof(CharacterFarmingState) },
            { Define.EAIState.Building, nameof(CharacterBuildingState) }
        };

        var moveState = new AIMoveToTargetState(targetPos, () =>
        {
            if (stateMap.TryGetValue(action, out var nextState))
            {
                ChangeState(nextState);
            }
        });

        RegisterState(moveState, aiCharacter);
        ChangeState(moveState.GetType().Name);
    }

    #endregion

    #region 건물 탐색

    private Vector3 FindNearestBuilding(Define.EAIState action)
    {
        var type = GetBuildingType(action);
        var building = FindAvailableBuilding(type);

        aiCharacter.currentBuilding = building;

        if (building == null)
        {
            Debug.LogWarning($"[{type}] 타입 건물을 찾을 수 없습니다.");
            return aiCharacter.transform.position;
        }

        return building.transform.position;
    }

    public BuildingBase FindAvailableBuilding(Define.BuildingType type)
    {
        var allAssigned = new HashSet<BuildingBase>(
            AIManager.Instance.AllCharacters
                .Select(c => c.currentBuilding)
                .Where(b => b != null)
        );

        return BuildingManager.Instance._buildings
            .Where(b => b.BuildingData.BuildingType == type && !allAssigned.Contains(b))
            .OrderBy(b => Vector3.Distance(b.transform.position, aiCharacter.transform.position))
            .FirstOrDefault();
    }

    private Define.BuildingType GetBuildingType(Define.EAIState action)
    {
        return action switch
        {
            Define.EAIState.Cooking => Define.BuildingType.Cooking,
            Define.EAIState.Farming => Define.BuildingType.Farm,
            Define.EAIState.Resting => Define.BuildingType.Resting,
            
        };
    }

    #endregion

    #region 이동 / 순찰

    public void Move(Vector3 destination)
    {
        aiCharacter.nav.ResetPath();
        aiCharacter.nav.SetDestination(destination);
    }

    public void PatrolMove(float patrolDelay)
    {
        if (aiCharacter.nav.isPathStale)
        {
            aiCharacter.nav.ResetPath();
            return;
        }

        patrolTimer += Time.deltaTime;

        if (patrolTimer >= patrolDelay)
        {
            Patrol();
            patrolTimer = 0f;
        }
    }

    private void Patrol()
    {
        var destination = GetRandomNavPosition(aiCharacter.transform.position, new Vector3(10f, 0f, 10f));
        aiCharacter.nav.SetDestination(destination);
    }

    private Vector3 GetRandomNavPosition(Vector3 origin, Vector3 range, int areaMask = NavMesh.AllAreas)
    {
        for (int i = 0; i < 10; i++)
        {
            var randomPoint = origin + new Vector3(
                Random.Range(-range.x, range.x),
                0f,
                Random.Range(-range.z, range.z)
            );

            if (NavMesh.SamplePosition(randomPoint, out var hit, 1f, areaMask))
                return hit.position;
        }

        return origin;
    }

    #endregion
    
}
