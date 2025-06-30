using UnityEngine;

namespace Scripts.Contents.AI.FSM.State
{
    public class CharacterDeliverState : AIState
    {
        public override void Init(AICharacter owner)
        {
            base.Init(owner);
            state = Define.EAIState.Resting;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            character.nav.speed = character.Stat.MoveSpeed / 2f;
            character.animator.SetInteger("animation", 18); // Cooking 애니메이션 설정
        }


        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            if (elapsedTime > 2)
            {
                character.UseStamina(5f);
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