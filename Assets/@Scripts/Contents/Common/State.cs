namespace Scripts.Contents.AI.CharatcerState
{
    public abstract class State<T>
    {
        protected float elpasedTime;

        public State() {}

        public virtual void Init(T owner) {}
        
        public virtual void OnEnter()
        {
            elpasedTime = 0;
        }

        public virtual void OnUpdate(float deltaTime)
        {
            elpasedTime += deltaTime;
        }
        
        public virtual void OnFixedUpdate() {}
        public virtual void OnExit() {}
        

    }
}