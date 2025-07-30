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
            character.nav.isStopped = true;
            character.nav.ResetPath();
            character.SetSpeed(0);
            character.SetAnimation(3); 
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
            character.nav.isStopped = false;
        }
    }
}