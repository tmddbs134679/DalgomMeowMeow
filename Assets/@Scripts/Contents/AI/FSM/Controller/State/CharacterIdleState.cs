using UnityEngine;

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
            randomPlayTime = Random.Range(10f, 20f); // 랜덤한 플레이 시간 설정
            character.View.SetSpeed(character.Stat.data.WalkSpeed);
            character.View.SetAnimation(21); // Idle 애니메이션 설정
            character.Controller.PatrolMove(0f); // 초기 순찰 이동 설정
            character.Controller.NavRotateTrue(); // 회전 활성화

        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);


            if (elapsedTime > randomPlayTime && character.Controller.TryPlay()) return;
            if (character.Controller.TryRest()) return;
            if (character.Controller.TryCook()) return;
            if (character.Controller.TryFarm()) return;
            if (character.Controller.TryFish()) return;



            character.Controller.PatrolMove(10f);
            character.Controller.TryHelloNearbyCharacter();
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}