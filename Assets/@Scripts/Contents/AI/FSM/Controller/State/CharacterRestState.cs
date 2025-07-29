using UnityEngine;

namespace Scripts.Contents.AI.FSM.State
{
    public class CharacterRestState : AIState
    {
        Vector3 sleepPos;
        public override void Init(AICharacter owner)
        {
            base.Init(owner);
            state = Define.EAIState.Rest;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            character.SetEmotion(4);
            character.currentBuilding.ConnectToAnimal(character);
            character.SetAnimation(25); // Cooking 애니메이션 설정
            character.Controller.NavRotateFalse(); // 회전 비활성화

            sleepPos = character.transform.position + new Vector3(0, 0.5284f, -0.34f); // 수면 위치 조정
        }


        public override void OnUpdate(float deltaTime)
        {
            character.transform.position = sleepPos; // 수면 위치 조정

            base.OnUpdate(deltaTime);
            if (character.Data.CurrentStamina >= character.Data.MaxStamina)
            {
                character.characterAction.Idle();
                return;
            }

            if (character.Data.CurrentStamina <= character.Data.MaxStamina && elapsedTime > 1)
            {
                character.RecoverStamina(10);
                elapsedTime = 0; 
                return;
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            character.currentBuilding.DisconnectAnimal();
            character.transform.position -= new Vector3(0, 0.5284f, -0.34f); // 약간 위로 올려서 수면 위치 조정

            character.SetEmotion(Random.Range(0,character.emo.EmotionMaterials.Length)); // Reset emotion to a random value
            if (character.currentBuilding != null)
            {
                character.currentBuilding = null; // Clear current building reference
            }
        }

    }
}