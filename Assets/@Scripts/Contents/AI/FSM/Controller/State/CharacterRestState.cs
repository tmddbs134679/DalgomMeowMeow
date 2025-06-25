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
            if (character.Stat.Stamina <= 100 && elapsedTime > 1)
            {
                character.UseStamina(-5f);
                elapsedTime = 0; 
                return;
            }
            if (character.Stat.Stamina == 100)
            {
                character.Controller.ChangeState(nameof(CharacterIdleState)); // Transition to idle state when fully rested
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            character.transform.rotation = Quaternion.Euler(0, 0, 0); // Reset rotation to default when exiting rest state
            if (character.currentBuilding != null)
            {
                character.currentBuilding.DisconnectAnimal();
                character.currentBuilding = null; // Clear current building reference
            }
        }

    }
}