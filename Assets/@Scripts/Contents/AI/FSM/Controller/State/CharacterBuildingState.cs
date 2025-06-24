namespace Scripts.Contents.AI.FSM.State
{
    public class CharacterBuildingState : AIState
    {
        public override void Init(AICharacter owner)
        {
            base.Init(owner);
            state = Define.EAIState.Building;
        }
        
        public override void OnEnter()
        {
            base.OnEnter();
        }
        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
        }
    }
}