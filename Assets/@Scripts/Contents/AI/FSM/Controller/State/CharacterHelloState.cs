using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace Scripts.Contents.AI.FSM.State
{
    public class CharacterHelloState : AIState
    {
        public override void Init(AICharacter owner)
        {
            base.Init(owner);
            state = Define.EAIState.Hello;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            character._isHelloReady = false; // Hello 준비 상태를 false로 설정
            character.nav.speed = 0;
            character.animator.SetInteger("animation", 3); // Idle 애니메이션 설정
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);

            if (elapsedTime > 5f)
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