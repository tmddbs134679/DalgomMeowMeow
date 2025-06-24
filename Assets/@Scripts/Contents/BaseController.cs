using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController<T> where T : MonoBehaviour
{
    public Dictionary<string, BaseState<T>> registedState = new Dictionary<string, BaseState<T>>();

    protected BaseState<T> currentState;

    protected BaseState<T> previousState;

    public BaseController(BaseState<T> initState, T owner)
    {
       RegisterState(initState, owner);

        currentState = initState;
        currentState.OnEnter();

    }

    public virtual void RegisterState(BaseState<T> state, T owner)
    {
        state.Init(owner);
        registedState[state.GetType().Name] = state;
    }

    public virtual void OnUpdate(float deltaTime)
    {
        currentState?.OnUpdate(deltaTime);  
    }

    public virtual void OnFixedUpdate()
    {
        currentState?.OnFixedUpdate();
    }

    public virtual void ChangeState(string newState)
    {
        if (currentState.ToString() == newState) return;

        currentState?.OnExit();

        previousState = currentState;

        currentState = registedState[newState];
        currentState.OnEnter();
    }

    public BaseState<T> PreviousState()
    {
        return previousState;
    }

    public BaseState<T> CurrentState()
    {
        return currentState;
    }

}
