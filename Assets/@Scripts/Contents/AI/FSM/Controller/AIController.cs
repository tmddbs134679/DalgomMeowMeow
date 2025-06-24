using System.Collections;
using System.Collections.Generic;
using Scripts.Contents.AI.FSM.State;
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
    
    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
    }

    private void OnActionPerformed(Define.EAIState action)
    {
        Vector3 targetPos = FindNearestBuilding(action);

        var moveState = new AIMoveToTargetState(targetPos, () =>
        {
            switch (action)
            {
                case Define.EAIState.Cooking:
                    ChangeState(nameof(CharacterCookState));
                    break;
                case Define.EAIState.Playing:
                    ChangeState(nameof(CharacterPlayState));
                    break;
                case Define.EAIState.Resting:
                    ChangeState(nameof(CharacterRestState));
                    break;
                case Define.EAIState.Farming:
                    ChangeState(nameof(CharacterFarmingState));
                    break;
                case Define.EAIState.Building:
                    ChangeState(nameof(CharacterBuildingState));
                    break;
            }
        });

        RegisterState(moveState, aiCharacter); // 상태 등록
        ChangeState(moveState.GetType().Name); // 이동 상태로 전환
    }


    private Vector3 FindNearestBuilding(Define.EAIState action)
    {
        // 추후 BuildingManager 등에서 가져오면 됨
        if (action == Define.EAIState.Cooking)
            return GameObject.Find("CookBuilding").transform.position;

        return Vector3.zero; // fallback
    }

    public void PatrolMove(float _patrolDelay)
    {
        if (aiCharacter.nav.isPathStale)
        {
            aiCharacter.nav.ResetPath();
        }

        if (aiCharacter.nav.hasPath)
        {
            return;
        }

        else
        {
             time += Time.deltaTime;
            if (0 < _patrolDelay)
                return;
            Patrol();
            time = 0f;
        }
    }

    private void Patrol()
    {
        aiCharacter.nav.SetDestination(RandomDestination(aiCharacter.transform.position, new Vector3(5f, 0f, 5f)));
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
    
  

    
    
    
