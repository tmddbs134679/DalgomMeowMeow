using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;

namespace Scripts.Contents.AI.FSM.State
{
    public class CharacterIdleState : AIState
    {
        private float randomPlayTime = 0f;

        public override void Init(AICharacter owner)
        {
            base.Init(owner);
            state = Define.EAIState.Idle;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            character.nav.areaMask = NavMesh.AllAreas; // 모든 네비메시 영역을 사용
            randomPlayTime = Random.Range(10f, 20f); // 랜덤한 플레이 시간 설정
            character.nav.speed = character.CharacterData.WalkSpeed;
            character.animator.SetInteger("animation", 21); // Idle 애니메이션 설정
            character.Controller.PatrolMove(0f); // 초기 순찰 이동 설정
            character.Controller.NavRotateTrue(); // 회전 활성화

        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);


            if (elapsedTime > randomPlayTime &&
                character.Controller.FindAvailableBuilding(Define.BuildingType.Playing))
            {
                character.characterAction.Play();
                return;
            }

            if (character.CharacterData.CurrentStamina <= 19f && 
                character.Controller.FindAvailableBuilding(Define.BuildingType.Resting) != null)
            {
                character.characterAction.Rest();
                return;
            }

            if (character.CharacterData.CurrentStamina >= 20 && 
                character.Controller.FindAvailableBuilding(Define.BuildingType.Cooking) != null)
            {
                character.characterAction.Cook();
                return;
            }

            if (character.CharacterData.CurrentStamina >= 25f && 
                character.Controller.FindAvailableBuilding(Define.BuildingType.Farm))
            {
                character.characterAction.Farm(); 
                return;
            }



            character.Controller.PatrolMove(10f);
            character.Controller.TryHelloNearbyCharacter();
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}