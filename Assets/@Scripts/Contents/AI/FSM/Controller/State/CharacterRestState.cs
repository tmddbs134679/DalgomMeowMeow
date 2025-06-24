using UnityEngine;

namespace Scripts.Contents.AI.FSM.State
{
    public class CharacterRestState : AIState
    {
        public override void Init(AICharacter owner)
        {
            base.Init(owner);
            state = Define.EAIState.Resting;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            //if (character.Stat.Stamina < 100)
            //    character.Stat.Stamina += 33;
            //else OnExit();
            character.transform.rotation = Quaternion.Euler(0, 0, -90); // Reset rotation to face forward
        }


    }
}