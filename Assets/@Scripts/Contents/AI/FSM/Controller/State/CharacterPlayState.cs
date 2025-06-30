namespace Scripts.Contents.AI.FSM.State
{
    public class CharacterPlayState : AIState
    {
        public override void Init(AICharacter owner)
        {
            base.Init(owner);
            state = Define.EAIState.Playing;
        }
        
        public override void OnEnter()
        {
            character.animator.SetInteger("animation", 50); // Play 애니메이션 설정
        }
        
        public override void OnUpdate(float deltaTime)
        {
            if (elapsedTime > 7f)
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