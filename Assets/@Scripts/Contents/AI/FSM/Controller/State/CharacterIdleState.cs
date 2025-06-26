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
            character.animator.SetInteger("animation", 21); // Idle 애니메이션 설정
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);

            character.Controller.PatrolMove(2f);

            if (character.Stat.Stamina <= 29f && character.Controller.FindAvailableBuilding(Define.BuildingType.Resting) != null)
            {
                character.characterAction.Rest();
                return;
            }

            if (character.Stat.Stamina >= 30 && character.Controller.FindAvailableBuilding(Define.BuildingType.Cooking) != null)
            {
                character.characterAction.Cook();
                return;
            }

        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}