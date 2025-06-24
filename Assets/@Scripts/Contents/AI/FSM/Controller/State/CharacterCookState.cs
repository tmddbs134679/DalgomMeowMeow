using UnityEngine;

namespace Scripts.Contents.AI.FSM.State
{
    public class CharacterCookState : AIState
    {
        public override void Init(AICharacter owner)
        {
            base.Init(owner);
            state = Define.EAIState.Cooking;
        }
        
        public override void OnEnter()
        {
            base.OnEnter();

            character.UseStamina(33);
            


            


        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
        }
        
        public override void OnExit()
        {
            base.OnExit();
            //character.OnAnimalLeaved();
        }
        
    }
}