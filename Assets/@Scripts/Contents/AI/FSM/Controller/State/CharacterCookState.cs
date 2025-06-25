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
            character.UseStamina(30);
            character.currentBuilding.ConnectToAnimal(character);
            character.OnAnimalArrived(); // 도착 처리 메소드 호출
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            if (elapsedTime > character.currentBuilding.BuildingData.Interval)
            {
                character.Controller.ChangeState(nameof(CharacterIdleState));
            }
          
        }

        public override void OnExit()
        {
            base.OnExit();
            if (character.currentBuilding != null)
            {
                character.OnAnimalLeaved();
                character.currentBuilding.DisconnectAnimal();
                character.currentBuilding = null;
            }

        }
        
    }
}