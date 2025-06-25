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
            character.transform.rotation = Quaternion.Euler(0, 0, -90); // Reset rotation to face forward
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            character.UseStamina(-1f); 
            if (character.Stat.Stamina == 100)
            {
                character.Controller.ChangeState(nameof(CharacterIdleState)); // Transition to idle state when fully rested
            }
        }


    }
}