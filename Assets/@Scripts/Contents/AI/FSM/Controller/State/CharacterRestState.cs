using UnityEngine;

namespace Scripts.Contents.AI.FSM.State
{
    public class CharacterRestState : AIState
    {
        public override void Init(AICharacter owner)
        {
            base.Init(owner);
            state = Define.EAIState.Resting;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            character.SetEmotion(4);
            character.animator.SetInteger("animation", 25); // Cooking 애니메이션 설정
        }


        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            if (character.Stat.Stamina <= 100 && elapsedTime > 1)
            {
                character.UseStamina(-5f);
                elapsedTime = 0; 
                return;
            }
            if (character.Stat.Stamina == 100)
            {
                character.characterAction.Idle(); // Transition to idle state when fully rested
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            character.SetEmotion(Random.Range(0, character.emo.Length)); // Reset emotion to a random value
            if (character.currentBuilding != null)
            {
                character.currentBuilding.DisconnectAnimal();
                character.currentBuilding = null; // Clear current building reference
            }
        }

    }
}