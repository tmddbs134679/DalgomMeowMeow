using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Scripts.Contents.AI.FSM.State;
using System.Threading.Tasks;
using UnityEditor;

public class AIController : BaseController<AICharacter>
{
    protected AICharacter character;
    private float patrolTimer = 0f;
    private float helloTimer = 0f;

    public AIController(AIState initState, AICharacter owner, Define.EAIState reset) : base(initState, owner, reset)
    {
        character = owner;
        Setup();
        ControllerRegister();
    }

    public void ControllerRegister()
    {
        RegisterState(new CharacterIdleState(), character, Define.EAIState.Idle);
        RegisterState(new CharacterBuildingState(), character, Define.EAIState.Build);
        RegisterState(new CharacterCookState(), character, Define.EAIState.Cook);
        RegisterState(new CharacterCabbageFarmingState(), character, Define.EAIState.CabbageFarm);
        RegisterState(new CharacterOnionFarmingState(), character, Define.EAIState.OnionFarm);
        RegisterState(new CharacterPotatoFarmingState(), character, Define.EAIState.PotatoFarm);
        RegisterState(new CharacterPumpkinFarmingState(), character, Define.EAIState.PumpkinFarm);
        RegisterState(new CharacterCarrotFarmingState(), character, Define.EAIState.CarrotFarm);
        RegisterState(new CharacterPlayState(), character, Define.EAIState.Play);
        RegisterState(new CharacterRestState(), character, Define.EAIState.Rest);
        RegisterState(new CharacterMoveToState(), character, Define.EAIState.MoveTo);
        RegisterState(new CharacterDeliverState(), character, Define.EAIState.Deliver);
        RegisterState(new CharacterHelloState(), character, Define.EAIState.Hello);
        RegisterState(new CharacterFishingState(), character, Define.EAIState.Fishing);

    }

    public Dictionary<Define.EBuildingType, Define.EAIState> farmActions = new()
{
    { Define.EBuildingType.CabbageFarm, Define.EAIState.CabbageFarm },
    { Define.EBuildingType.OnionFarm,   Define.EAIState.OnionFarm },
    { Define.EBuildingType.PotatoFarm,  Define.EAIState.PotatoFarm },
    { Define.EBuildingType.PumpkinFarm, Define.EAIState.PumpkinFarm },
    { Define.EBuildingType.CarrotFarm,  Define.EAIState.CarrotFarm },
};

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        HelloTimerReset(deltaTime);
    }

    public override void OnLateUpdate(float deltaTime)
    {
        base.OnLateUpdate(deltaTime);
    }

    public void Setup()
    {
        character.Action.OnAction += OnActionPerformed;
    }

    public void Dispose()
    {
        if (character?.Action != null)
        {
            character.Action.OnAction -= OnActionPerformed;
        }
    }

    #region FSM - Action 처리

    public void OnActionPerformed(Define.EAIState action)
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


        if (action == Define.EAIState.Deliver &&
          registedState.TryGetValue(Define.EAIState.Deliver, out BaseState<AICharacter> deliverBase) &&
            deliverBase is CharacterDeliverState deliveryTo)
        {
            if (targetPos == null)
            {
                ChangeState(Define.EAIState.Idle);
                return;
            }

            deliveryTo.SetDestination(targetPos - new Vector3(1.5f, 0, 1.5f));
            ChangeState(Define.EAIState.Deliver);
            return;
        }

        if (action == Define.EAIState.Play)
        {
            targetPos -= new Vector3(0.9f, 0, 0); // 플레이 위치 조정
        }

        if (action == Define.EAIState.Rest)
        {
            targetPos -= new Vector3(0f, 0f, 0.3f);
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

    #region 상태 변경

    public bool TryRest()
    {
        if (character.Stat.data.CurrentStamina <= 19f &&
            FindAvailableBuilding(Define.EBuildingType.Resting) != null)
        {
            character.Action.Rest();
            return true;
        }
        return false;
    }

    public bool TryCook()
    {
        if (character.Stat.data.CurrentStamina >= 20f &&
            FindAvailableBuilding(Define.EBuildingType.Cooking) != null)
        {
            character.Action.Cook();
            return true;
        }
        return false;
    }

    public bool TryFish()
    {
        if (character.Stat.data.CurrentStamina >= 100f &&
            FindAvailableBuilding(Define.EBuildingType.Fishing))
        {
            character.Action.Fishing();
            return true;
        }
        return false;
    }

    public bool TryFarm()
    {
        if (character.Stat.data.CurrentStamina < 40f)
            return false;



        foreach (var (type, action) in farmActions)
        {
            if (FindAvailableBuilding(type))
            {
                character.Action.TryState(action);
                return true;
            }
        }

        return false;
    }

    public bool TryPlay()
    {
        if (FindAvailableBuilding(Define.EBuildingType.Playing))
        {
            character.Action.Play();
            return true;
        }

        return false;
    }

    public void ProcessHarvestAndDelivery()
    {
        if (character.currentBuilding is not FarmBuilding farm)
        {
            Debug.LogWarning("Current building is not a FarmBuilding.");
            return;
        }

        // 1. 수확 처리
        character.DistinguishCrops(farm.CropType);

        // 2. 배달할 곳이 있는지 확인
        bool hasDeliverTarget = FindNearestBuilding(Define.EAIState.Deliver) != null;

        // 3. 상태 전환
        if (hasDeliverTarget)
        {
            character.Action.Deliver();
            return;
        }
        else
        {
            character.Action.Idle();
            return;
        }
    }





    #endregion

    #region 건물 탐색

    public Vector3 FindNearestBuilding(Define.EAIState action)
    {
        var type = GetBuildingType(action);

        if (action == Define.EAIState.Deliver)
        {
            var nearbuilding = FineOnlyBuilding(Define.EBuildingType.Cooking);

            return nearbuilding.transform.position;
        }

        var building = FindAvailableBuilding(type);


        character.currentBuilding = building;

        if (building == null)
        {
            ChangeState(Define.EAIState.Idle);
            return character.transform.position;
        }

        return building.transform.position;
    }

    public BuildingBase FineOnlyBuilding(Define.EBuildingType type)
    {

        return BuildingManager.Instance._buildings
      .Where(b => b != null && b.gameObject != null)
      .Where(b => b.BuildingData.BuildingType == type)
      .OrderBy(b => Vector3.Distance(b.transform.position, character.transform.position))
      .FirstOrDefault();
    }

    public BuildingBase FindAvailableBuilding(Define.EBuildingType type)
    {
        var allAssigned = new HashSet<BuildingBase>(
            Managers.AI.AllCharacters
                .Select(c => c.currentBuilding)
                .Where(b => b != null && b.gameObject != null)
        );

        return BuildingManager.Instance._buildings
            .Where(b => b != null && b.gameObject != null)
            .Where(b => b.BuildingData.BuildingType == type && !allAssigned.Contains(b))
            .OrderBy(b => Vector3.Distance(b.transform.position, character.transform.position))
            .FirstOrDefault();
    }

    private Define.EBuildingType GetBuildingType(Define.EAIState action)
    {
        return action switch
        {
            Define.EAIState.Cook => Define.EBuildingType.Cooking,
            Define.EAIState.CabbageFarm => Define.EBuildingType.CabbageFarm,
            Define.EAIState.OnionFarm => Define.EBuildingType.OnionFarm,
            Define.EAIState.PotatoFarm => Define.EBuildingType.PotatoFarm,
            Define.EAIState.PumpkinFarm => Define.EBuildingType.PumpkinFarm,
            Define.EAIState.CarrotFarm => Define.EBuildingType.CarrotFarm,
            Define.EAIState.Rest => Define.EBuildingType.Resting,
            Define.EAIState.Play => Define.EBuildingType.Playing,
            Define.EAIState.Deliver => Define.EBuildingType.Cooking,
            Define.EAIState.Fishing => Define.EBuildingType.Fishing,
            _ => Define.EBuildingType.None,

        };
    }

    #endregion

    #region 이동 / 순찰
    public void NavRotateFalse()
    {
        character.View.Nav.updateRotation = false;

        if (currentState is CharacterRestState)
        {
            character.transform.eulerAngles = new Vector3(0f, -180f, 0f);
        }

        else
        {
            character.transform.eulerAngles = new Vector3(0f, -146f, 0f); // 기본 회전값 설정
        }

    }

    public void NavRotateTrue()
    {
        character.View.Nav.updateRotation = true;
    }

public async void Move(Vector3 destination)
{
    if (character.View.Nav == null)
        return;

    if (!character.View.Nav.enabled || !character.View.Nav.isOnNavMesh)
        return;

    await Task.Yield(); //  WebGL 안정화 1프레임
    character.View.Nav.ResetPath();
    await Task.Yield(); //  ResetPath 후 반드시 1프레임 대기
    character.View.Nav.SetDestination(destination);
}

    public void PatrolMove(float patrolDelay)
    {
        if (!character.View.Nav.enabled || !character.View.Nav.isOnNavMesh) return;
        patrolTimer += Time.deltaTime;

        if (patrolTimer >= patrolDelay)
        {
            Patrol();
            character.View.SetAnimation(21);
            patrolTimer = Random.Range(0f, 10f);
            return;
        }

        if (character.View.Nav.isPathStale)
        {
            character.View.Nav.ResetPath();
            return;
        }

        if (HasArrived())
        {
            character.View.SetAnimation(36);
            return;
        }

    }

    private bool HasArrived()
    {
        return !character.View.Nav.pathPending &&
               character.View.Nav.remainingDistance <= character.View.Nav.stoppingDistance &&
               (!character.View.Nav.hasPath || character.View.Nav.velocity.sqrMagnitude == 0f);
    }

    private void Patrol()
    {
        var destination = GetRandomNavPosition(character.transform.position, new Vector3(5f, 0f, 5f));
        character.View.Nav.SetDestination(destination);
    }

    private Vector3 GetRandomNavPosition(Vector3 origin, Vector3 range)
    {
        for (int i = 0; i < 100; i++) // 최대 30번 시도
        {
            var randomPoint = origin + new Vector3(
                Random.Range(-range.x, range.x),
                0f,
                Random.Range(-range.z, range.z)
            );

            if (NavMesh.SamplePosition(randomPoint, out var hit, 1f, NavMesh.AllAreas))
                return hit.position;
        }

        return origin;
    }

    #endregion

    #region 서로 인사하기
    public void TryHelloNearbyCharacter()
    {
        if (!character._isHelloReady)
            return;
        if (character.Interaction.isClicked)
            return;
        if (character.Interaction.isFollowing) return;

        // 주변 모든 캐릭터 탐색
        foreach (var other in Managers.AI.AllCharacters)
        {
            if (other == character) continue; // 자기 자신 제외
            if (!other._isHelloReady) continue;
            if (other.Interaction.isClicked) continue;
            if (other.Interaction.isFollowing) continue;// 클릭된 캐릭터 제외


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
                    character.Action.Hello();
                    other.Action.Hello(); // 상대방도 인사
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
