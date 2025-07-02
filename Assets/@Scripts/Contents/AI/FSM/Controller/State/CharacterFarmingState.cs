using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace Scripts.Contents.AI.FSM.State
{
    public class CharacterFarmingState : AIState
    {
        public override void Init(AICharacter owner)
        {
            base.Init(owner);
            state = Define.EAIState.Farming;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            character.UseStamina(25);
            character.currentBuilding.ConnectToAnimal(character);
            character.OnAnimalArrived(); // 도착 처리 메소드 호출
            character.animator.SetInteger("animation", 29); // Farming 애니메이션 설정
            character.Controller.NavRotateFalse();
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            if (elapsedTime >= character.currentBuilding.BuildingData.Interval)
            {
                character.characterAction.Deliver();
                return;
            }

        }

        public override void OnExit()
        {
            base.OnExit();
            if (character.currentBuilding != null)
            {
                character.currentBuilding.DisconnectAnimal();
                character.OnAnimalLeaved();
                character.currentBuilding = null;
            }

        }

    }
}