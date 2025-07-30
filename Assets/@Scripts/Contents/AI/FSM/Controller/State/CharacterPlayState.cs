namespace Scripts.Contents.AI.FSM.State
{
    public class CharacterPlayState : AIState
    {
        public override void Init(AICharacter owner)
        {
            base.Init(owner);
            state = Define.EAIState.Play;
        }
        
        public override void OnEnter()
        {
            base.OnEnter();
            character.SetAnimation(50); // Play 애니메이션 설정
            character.currentBuilding.ConnectToAnimal(character);
            character.OnAnimalArrived(); // 도착 처리 메소드 호출
            character.Controller.NavRotateFalse(); // 회전 비활성화
        }
        
        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            if (elapsedTime > 7f)
            {
                character.characterAction.Idle();
                return;
            }
        }

        public override void OnExit()
        {
            character.OnAnimalLeaved();
            character.currentBuilding.DisconnectAnimal();
            base.OnExit();
        }
    }
}