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
            
        }
        
        public override void OnUpdate(float deltaTime)
        {
            
        }

        public override void OnExit()
        {
            
        }
    }
}