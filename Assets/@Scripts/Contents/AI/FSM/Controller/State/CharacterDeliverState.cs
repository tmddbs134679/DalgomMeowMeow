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
            state = Define.EAIState.Resting;
        }

        public void SetDestination(Vector3 target)
        {
            this.targetPosition = target;
            this.isArrived = false;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            character.nav.speed = character.CharacterData.MoveSpeed / 2; 
            character.animator.SetInteger("animation", 47); // Cooking 애니메이션 설정
            
        }


        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            if (elapsedTime > 2)
            {
                character.UseStamina(5f);
                elapsedTime = 0f; // Reset elapsed time after using stamina
            }

            // 일정 거리 이내면 도착한 것으로 판단
            if (Vector3.Distance(character.transform.position, targetPosition) < 0.5f)
            {
                isArrived = true;
                character.animator.SetInteger("animation", 3);
                if (elapsedTime > 1f)
                {
                    character.characterAction.Idle();
                    character.OnAnimalDelivered(); // Notify that the animal has been delivered
                }
            }

        }

        public override void OnExit()
        {
            base.OnExit();
            if (character.currentBuilding != null)
            {
                character.currentBuilding = null; // Clear current building reference
            }
        }

    }
}