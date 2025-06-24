namespace Scripts.Contents.AI.FSM.State
{
    public class CharacterFarmingState : AIState
    {
        public override void Init(AICharacter owner)
        {
            base.Init(owner);
            this.character = owner;
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