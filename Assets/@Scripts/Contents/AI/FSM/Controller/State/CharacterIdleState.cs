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
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);

            character.Controller.PatrolMove(2f);

            if (character.Stat.Stamina <= 29f)
            {
                character.characterAction.Rest();
                return;
            }

            if (character.Stat.Stamina >= 30)
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