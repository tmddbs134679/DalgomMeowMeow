using System;
using UnityEngine;
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
            character.View.SetAnimation(18); // 이동 애니메이션 설정
            character.Controller.NavRotateTrue(); // 회전 활성화
            character.Controller.Move(targetPosition);
            character.View.SetSpeed(character.Stat.data.MoveSpeed);

        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);


            // 일정 거리 이내면 도착한 것으로 판단
            if (character.currentBuilding == null)
            {
                character.Action.Idle();
                return;
            }
            if (elapsedTime > 0.5f)
            {
                targetPosition = character.currentBuilding.transform.position;
                if (character.currentBuilding.BuildingData.BuildingType == Define.EBuildingType.Playing)
                {
                    targetPosition -= new Vector3(0.9f, 0, 0);
                }

                character.Controller.Move(targetPosition);
                elapsedTime = 0f; // 시간 초기화
            }


            if (Vector3.Distance(character.transform.position, targetPosition) < 0.5f && !character.Interaction.isFollowing)
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