namespace Scripts.Contents.AI.FSM.State
{
    public class CharacterCabbageFarmingState : AIState
    {
        public override void Init(AICharacter owner)
        {
            base.Init(owner);
            state = Define.EAIState.CabbageFarm;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            character.Stat.UseStamina(25);
            character.currentBuilding.ConnectToAnimal(character);
            character.OnAnimalArrived(); // 도착 처리 메소드 호출
            character.View.SetAnimation(29);
            character.Controller.NavRotateFalse();
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            if (elapsedTime < character.currentBuilding.BuildingData.Interval)
                return;

            character.Controller.ProcessHarvestAndDelivery();

        }

        public override void OnExit()
        {
            base.OnExit();
            if (character.currentBuilding != null)
            {
                character.currentBuilding.DisconnectAnimal();
                character.OnAnimalLeaved();
                character.currentBuilding = null;
            }

        }

    }
}