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
            character.View.Nav.isStopped = true;
            character.View.Nav.ResetPath();
            character.View.SetSpeed(0);
            character.View.SetAnimation(3); 
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);

            if (elapsedTime > 5f)
            {
                character.Action.Idle();
                return;
            }

        }

        public override void OnExit()
        {
            base.OnExit();
            character.View.Nav.isStopped = false;
        }
    }
}