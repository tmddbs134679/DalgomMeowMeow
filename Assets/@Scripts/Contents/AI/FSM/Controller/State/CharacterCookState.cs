using UnityEngine;
using UnityEngine.TextCore.Text;

namespace Scripts.Contents.AI.FSM.State
{
    public class CharacterCookState : AIState
    {
        public override void Init(AICharacter owner)
        {
            base.Init(owner);
            state = Define.EAIState.Cooking;
        }
        
        public override void OnEnter()
        {
            base.OnEnter();
            character.currentBuilding.ConnectToAnimal(character);
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            if (elapsedTime > 1f && character.Stat.Stamina > 5f)
            {
                character.UseStamina(5f);
                elapsedTime = 0;
                return;
            }
            

            if (character.Stat.Stamina <= 5)
            {
                character.Controller.ChangeState(nameof(CharacterIdleState));

            }
        }

        public override void OnExit()
        {
            base.OnExit();
            if (character.currentBuilding != null)
            {
                character.currentBuilding.DisconnectAnimal();
                character.currentBuilding = null;
            }

        }
        
    }
}