namespace Scripts.Contents.AI.CharatcerState
{
    public class CharacterAIController : BaseFSM<Character>
    {
        Character character;
        
        public CharacterAIController(State<Character> initState, Character owner) : base(initState, owner)
        {
            this.character = owner;
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
        }
    }
}