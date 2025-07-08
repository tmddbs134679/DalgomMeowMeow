using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace Scripts.Contents.AI.FSM.State
{
    public class CharacterResetState : AIState
    {
       
        public override void Init(AICharacter owner)
        {
            base.Init(owner);
            state = Define.EAIState.None;
        }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            if (elapsedTime > 0.1f)
            {
                character.characterAction.Idle();
                return;

            }
            
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}