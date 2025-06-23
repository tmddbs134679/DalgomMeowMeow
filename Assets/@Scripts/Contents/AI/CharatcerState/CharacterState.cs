namespace Scripts.Contents.AI.CharatcerState
{
    public class CharacterState : State<Character>
    {
        protected Define.CharacterState state;
        protected Character character;

        public override void Init(Character owner)
        {
            this.character = owner;
        }
        
        public Define.CharacterState State { get { return state; } }
        
        
    }
    
    
}