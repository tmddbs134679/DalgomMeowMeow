using UnityEngine;

namespace Scripts.Contents.AI.FSM.State
{
    public class CharacterRestState : AIState
    {
        public override void Init(AICharacter owner)
        {
            base.Init(owner);
            state = Define.EAIState.Rest;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            character.SetEmotion(4);
            character.SetAnimation(25); // Cooking 애니메이션 설정
            character.Controller.NavRotateFalse(); // 회전 비활성화
        }


        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            if (character.Data.CurrentStamina == character.Data.MaxStamina)
            {
                character.characterAction.Idle();
                return;
            }

            if (character.Data.CurrentStamina <= character.Data.MaxStamina && elapsedTime > 1)
            {
                character.RecoverStamina(10);
                elapsedTime = 0; 
                return;
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            character.SetEmotion(Random.Range(0, character.emo.Length)); // Reset emotion to a random value
            if (character.currentBuilding != null)
            {
                character.currentBuilding = null; // Clear current building reference
            }
        }

    }
}