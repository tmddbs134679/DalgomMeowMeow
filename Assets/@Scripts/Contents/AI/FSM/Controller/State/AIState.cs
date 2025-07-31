public class AIState : BaseState<AICharacter>
{
    protected Define.EAIState state;
    protected AICharacter character;
    public override void Init(AICharacter owner)
    {
        this.character = owner;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        character.Stat.data.CurrentState = state; 
        character.CurrentState = character.Stat.data.CurrentState;
    }

    public Define.EAIState GetState() { return state; }
}
