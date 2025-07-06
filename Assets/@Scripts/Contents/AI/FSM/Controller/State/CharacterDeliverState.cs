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
            state = Define.EAIState.Delivery;
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
            character.SetSpeed(character.CharacterData.MoveSpeed / 2);
            character.SetAnimation(47);
            character.Controller.NavRotateTrue(); // 회전 활성화

        }


        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            if (elapsedTime > 2 && !isArrived)
            {
                character.UseStamina(2f);
                elapsedTime = 0f; // Reset elapsed time after using stamina
                return;
            }

            if (elapsedTime > 1.2f && isArrived)
            {
                character.characterAction.Idle();
                return;
            }
            // 일정 거리 이내면 도착한 것으로 판단
            if (Vector3.Distance(character.transform.position, targetPosition) < 2.5f &&
                !isArrived)
            {
                isArrived = true;
                character.animator.SetInteger("animation", 50);
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