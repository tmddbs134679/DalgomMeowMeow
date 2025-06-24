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
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}