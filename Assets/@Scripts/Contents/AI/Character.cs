using Scripts.Contents.AI.CharatcerState;
using UnityEngine;
namespace Scripts.Contents.AI
{
    public class Character : MonoBehaviour
    {
        public CharacterAIController AIController
        {
            get { return _AIController; }
        }
        private CharacterAIController _AIController;

        public CharacterStatSo stat;


        private void Awake()
        {
            ControllerRegister();
        }

        private void OnUpdate()
        {
            AIController.OnUpdate(Time.deltaTime);
        }

        public void ControllerRegister()
        {
            _AIController = new CharacterAIController(new CharacterIdleState(), this);
        }
    }
}