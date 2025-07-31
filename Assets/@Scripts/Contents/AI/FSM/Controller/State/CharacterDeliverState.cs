using UnityEngine;

namespace Scripts.Contents.AI.FSM.State
{
    public class CharacterDeliverState : AIState
    {
        private Vector3 targetPosition;
        private bool isArrived = false;
        public override void Init(AICharacter owner)
        {
            base.Init(owner);
            state = Define.EAIState.Deliver;
        }

        public void SetDestination(Vector3 target)
        {
            this.targetPosition = target;
            this.isArrived = false;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            character.Controller.Move(targetPosition); // 이동할 위치 설정
            character.View.SetSpeed(character.Stat.data.MoveSpeed / 2);
            character.View.SetAnimation(47);
            character.Controller.NavRotateTrue(); // 회전 활성화

        }


        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            if (elapsedTime >= 0.5 && !isArrived)
            {
                character.Stat.UseStamina(0.5f);
                targetPosition = character.Controller.FindNearestBuilding(state) - new Vector3(1.5f, 0, 1.5f);
                character.Controller.Move(targetPosition );
                elapsedTime = 0f; 
                return;
            }

            if (elapsedTime > 1.3f && isArrived)
            {
                character.Action.Idle();
                return;
            }
            // 일정 거리 이내면 도착한 것으로 판단
            if (Vector3.Distance(character.transform.position, targetPosition) < 2.5f &&
                !isArrived)
            {
                isArrived = true;
                character.View.SetAnimation(50); // 배달 완료 애니메이션 설정
                character.OnAnimalDelivered();
                elapsedTime = 0f; // Reset elapsed time after delivery
                return;
            }


        }

        public override void OnExit()
        {
            base.OnExit();
            isArrived = false; // Reset arrival status
            if (character.currentBuilding != null)
            {
                character.currentBuilding = null; // Clear current building reference
            }
        }

    }
}