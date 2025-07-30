
namespace Scripts.Contents.AI.FSM.State
{
    public class CharacterResetState : AIState
    {
       
        public override void Init(AICharacter owner)
        {
            base.Init(owner);
            state = Define.EAIState.None;
        }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            if (character.loadState != Define.EAIState.MoveTo &&
                character.loadState != Define.EAIState.None)
            {
                character.Controller.OnActionPerformed(character.loadState);
                return;
            }
            if (elapsedTime > 1f)
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