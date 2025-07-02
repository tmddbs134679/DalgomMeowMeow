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
            character.nav.speed = character.CharacterData.MoveSpeed / 2; 
            character.animator.SetInteger("animation", 47); // Cooking 애니메이션 설정
            character.Controller.NavRotateTrue(); // 회전 활성화

        }


        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            if (elapsedTime > 2)
            {
                character.UseStamina(2f);
                elapsedTime = 0f; // Reset elapsed time after using stamina
            }
            // 일정 거리 이내면 도착한 것으로 판단
            if (Vector3.Distance(character.transform.position, targetPosition) < 0.5f)
            {
                isArrived = true;
                character.animator.SetInteger("animation", 3);
                if (elapsedTime > 1f)
                {
                    character.OnAnimalDelivered(); // Notify that the animal has been delivered
                    character.characterAction.Idle();
                }
            }

        }

        public override void OnExit()
        {
            base.OnExit();
            if (character.currentBuilding != null)
            {
                isArrived = false; // Reset arrival status
                character.currentBuilding = null; // Clear current building reference
            }
        }

    }
}