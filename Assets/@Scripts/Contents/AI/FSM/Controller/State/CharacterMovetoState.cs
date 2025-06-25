using System;
using UnityEngine;
public class AIMoveToTargetState : AIState
{
    private Vector3 targetPosition;
    private Action onArrived;
    private bool isArrived = false;

    public AIMoveToTargetState(Vector3 target, Action onArrivedCallback)
    {
        this.targetPosition = target;
        this.onArrived = onArrivedCallback;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        state = Define.EAIState.MoveTo;
        character.Controller.Move(targetPosition);
        Debug.Log("이동 시작");
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        // 일정 거리 이내면 도착한 것으로 판단
        if (Vector3.Distance(character.transform.position, targetPosition) < 2f)
        {
            isArrived = true;
            Debug.Log("목적지 도착");
            character.OnAnimalArrived(); // 도착 처리 메소드 호출
            onArrived?.Invoke(); // 도착 콜백 실행
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        isArrived = false; // 상태 종료 시 도착 여부 초기화
    }


}