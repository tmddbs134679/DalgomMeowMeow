using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;
namespace Scripts.Contents.AI.FSM.State
{
    public class CharacterMoveToState : AIState
    {
        private Vector3 targetPosition;
        private Action onArrived;
        private bool isArrived = false;

        public void SetDestination(Vector3 target, Action onArrivedCallback)
        {
            this.targetPosition = target;
            this.onArrived = onArrivedCallback;
            this.isArrived = false;
        }

   

        public override void OnEnter()
        {
            base.OnEnter();
            state = Define.EAIState.MoveTo;
            character.SetAnimation(18); // 이동 애니메이션 설정
            character.Controller.NavRotateTrue(); // 회전 활성화
            character.Controller.Move(targetPosition);
            character.SetSpeed(character.Data.MoveSpeed);

        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            // 일정 거리 이내면 도착한 것으로 판단
            if (character.currentBuilding == null)
            {
                character.characterAction.Idle();
            }
            if (Vector3.Distance(character.transform.position, targetPosition) < 0.5f)
            {
                isArrived = true;

                onArrived?.Invoke(); // 도착 콜백 실행
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            isArrived = false; // 상태 종료 시 도착 여부 초기화
        }


    }
}