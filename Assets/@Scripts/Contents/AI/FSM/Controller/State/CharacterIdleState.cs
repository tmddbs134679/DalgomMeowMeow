using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace Scripts.Contents.AI.FSM.State
{
    public class CharacterIdleState : AIState
    {
        public override void Init(AICharacter owner)
        {
            base.Init(owner);
            state = Define.EAIState.Idle;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            character.renderer.material.color = Color.white; // 색상 초기화
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);

            character.Controller.PatrolMove(2f);

            if (character.Stat.Stamina <= 29f && character.Controller.FindAvailableBuilding(BuildingType.Resting) != null)
            {
                character.renderer.material.color = Color.red; // 색상 변경
                character.characterAction.Rest();
                return;
            }

            if (character.Stat.Stamina >= 30 && character.Controller.FindAvailableBuilding(BuildingType.Cooking) != null)
            {
                character.characterAction.Cook();
                character.renderer.material.color = Color.green;
                return;
            }

        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}