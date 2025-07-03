using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Scripts.Contents.AI.FSM.State;
using System.Collections;
using Unity.VisualScripting;

public class AIController : BaseController<AICharacter>
{
    protected AICharacter character;
    private float patrolTimer = 0f;
    private float helloTimer = 0f;

    public AIController(AIState initState, AICharacter owner, Define.EAIState idle) : base(initState, owner, idle)
    {
        character = owner;
        character.characterAction.OnAction += OnActionPerformed;
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        HelloTimerReset(deltaTime);
    }

    public override void OnLateUpdate(float deltaTime)
    {
        base.OnLateUpdate(deltaTime);
    }

    public void Dispose()
    {
        if (character?.characterAction != null)
        {
            character.characterAction.OnAction -= OnActionPerformed;
        }
    }

    #region FSM - Action 처리

    private void OnActionPerformed(Define.EAIState action)
    {
        if (action == Define.EAIState.Idle)
        {
            ChangeState(Define.EAIState.Idle);
            return;
        }
        if (action == Define.EAIState.Hello)
        {
            ChangeState(Define.EAIState.Hello);
            return;
        }

        Vector3 targetPos = FindNearestBuilding(action);


        if (action == Define.EAIState.Delivery &&
          registedState.TryGetValue(Define.EAIState.Delivery, out BaseState<AICharacter> deliverBase) &&
            deliverBase is CharacterDeliverState deliveryTo)
        {
            deliveryTo.SetDestination(targetPos - new Vector3(0,0,1.2f));
            ChangeState(Define.EAIState.Delivery);
            return;
        }

        if (action == Define.EAIState.Playing)
        {
            targetPos -= new Vector3(0.9f, 0, 0); // 플레이 위치 조정
        }


        if (registedState.TryGetValue(Define.EAIState.MoveTo, out BaseState<AICharacter> moveBaseState) &&
            moveBaseState is CharacterMoveToState moveToState)
        {
            moveToState.SetDestination(targetPos, () => ChangeState(action));
            ChangeState(Define.EAIState.MoveTo);
            return;
        }

    }

    #endregion

    #region 건물 탐색

    private Vector3 FindNearestBuilding(Define.EAIState action)
    {
        var type = GetBuildingType(action);

        if (action == Define.EAIState.Delivery)
        {
            var nearbuilding = FineOnlyBuilding(type);

            return nearbuilding.transform.position;
        }

        var building = FindAvailableBuilding(type);
        

        character.currentBuilding = building;

        if (building == null)
        {
            Debug.LogWarning($"[{type}] 타입 건물을 찾을 수 없습니다.");
            ChangeState(Define.EAIState.Idle);
            return character.transform.position;
        }

        Debug.Log(building.transform.position);
        return building.transform.position;
    }

    public BuildingBase FineOnlyBuilding(Define.BuildingType type)
    {
        return BuildingManager.Instance._buildings
            .FirstOrDefault(b => b.BuildingData.BuildingType == type);
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
            .OrderBy(b => Vector3.Distance(b.transform.position, character.transform.position))
            .FirstOrDefault();
    }

    private Define.BuildingType GetBuildingType(Define.EAIState action)
    {
        return action switch
        {
            Define.EAIState.Cooking => Define.BuildingType.Cooking,
            Define.EAIState.Farming => Define.BuildingType.Farm,
            Define.EAIState.Resting => Define.BuildingType.Resting,
            Define.EAIState.Playing => Define.BuildingType.Playing,
            Define.EAIState.Delivery => Define.BuildingType.Cooking,

        };
    }

    #endregion

    #region 이동 / 순찰
    public void NavRotateFalse()
    {
        character.nav.updateRotation = false;
        character.transform.eulerAngles = new Vector3(0f, -146f, 0f); // 기본 회전값 설정
    }

    public void NavRotateTrue()
    {
        character.nav.updateRotation = true;
    }

    public void Move(Vector3 destination)
    {
        character.nav.ResetPath();
        character.nav.SetDestination(destination);
    }

    public void PatrolMove(float patrolDelay)
    {
        patrolTimer += Time.deltaTime;

        if (patrolTimer >= patrolDelay)
        {
            Patrol();
            character.SetAnimation(21);
            patrolTimer = Random.Range(0f, 10f);
            return;
        }

        if (character.nav.isPathStale)
        {
            character.nav.ResetPath();
            return;
        }

        if (HasArrived())
        {
            character.SetAnimation(36);
            return;
        }

    }

    private bool HasArrived()
    {
        return !character.nav.pathPending &&
               character.nav.remainingDistance <= character.nav.stoppingDistance &&
               (!character.nav.hasPath || character.nav.velocity.sqrMagnitude == 0f);
    }

    private void Patrol()
    {
        var destination = GetRandomNavPosition(character.transform.position, new Vector3(5f, 0f, 5f));
        character.nav.SetDestination(destination);
    }

    private Vector3 GetRandomNavPosition(Vector3 origin, Vector3 range )
    {
        for(int i = 0; i < 100; i++) // 최대 30번 시도
        {
            var randomPoint = origin + new Vector3(
                Random.Range(-range.x, range.x),
                0f,
                Random.Range(-range.z, range.z)
            );

            //if (IsNearWorkBuilding(randomPoint))
            //    continue;

            if (NavMesh.SamplePosition(randomPoint, out var hit, 1f, NavMesh.AllAreas))
                return hit.position;
        }

        return origin;
    }

    private bool IsNearWorkBuilding(Vector3 point)
    {
        foreach (var building in BuildingManager.Instance._buildings)
        {
            float distance = Vector3.Distance(building.transform.position, point);
            if (distance < 2f)
                return true;
        }
        return false;
    }

    #endregion

    #region 서로 인사하기
    public void TryHelloNearbyCharacter()
    {
        if (!character._isHelloReady)
            return;
        if (character.isClicked)
            return;

        // 주변 모든 캐릭터 탐색
        foreach (var other in AIManager.Instance.AllCharacters)
        {
            if (other == character) continue; // 자기 자신 제외
            if (!other._isHelloReady) continue;
            if(other.isClicked) continue; // 클릭된 캐릭터 제외


            float distance = Vector3.Distance(character.transform.position, other.transform.position);
            if (distance > 2.5f) continue; // 인사 거리 제한

            Vector3 dirToOther = (other.transform.position - character.transform.position).normalized;
            Vector3 dirToSelf = (character.transform.position - other.transform.position).normalized;

            // 나 → 상대방 : 내가 상대방을 향하고 있는지
            float dotToOther = Vector3.Dot(character.transform.forward, dirToOther);
            // 상대방 → 나 : 상대방이 나를 향하고 있는지
            float dotToSelf = Vector3.Dot(other.transform.forward, dirToSelf);

            float angleToOther = Mathf.Acos(dotToOther) * Mathf.Rad2Deg;
            float angleToSelf = Mathf.Acos(dotToSelf) * Mathf.Rad2Deg;
            if (other.Controller.currentState is CharacterIdleState)
            {
                if (angleToOther < 45f && angleToSelf < 45f) // 마주보는 각도 제한 (45도)
                {
                    // 인사 상태로 변경
                    character.characterAction.Hello();
                    other.characterAction.Hello(); // 상대방도 인사
                    break; // 한 번만 실행
                }
            }
        }
    }

    private void HelloTimerReset(float deltaTime)
    {
        if (!character._isHelloReady)
        {
            if (helloTimer > 30f)
            {
                character._isHelloReady = true;
                helloTimer = 0f; // 타이머 초기화
                return;
            }
            helloTimer += deltaTime;
        }
    }


    #endregion

}
