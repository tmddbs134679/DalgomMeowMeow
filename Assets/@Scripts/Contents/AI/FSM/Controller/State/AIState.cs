
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
        character.Data.CurrentState = state;
        character.CurrentState = character.Data.CurrentState;
    }

    public Define.EAIState GetState() { return state; }
}
