namespace Scripts.Contents.AI.CharatcerState
{
    public class CharacterIdleState : CharacterState
    {
        public override void Init(Character owner)
        {
            base.Init(owner);
            state = Define.CharacterState.Idle;
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