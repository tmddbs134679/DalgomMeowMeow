using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController<T> where T : MonoBehaviour
{
    public Dictionary<Define.EAIState, BaseState<T>> registedState = new Dictionary<Define.EAIState, BaseState<T>>();

    protected BaseState<T> currentState;

    protected BaseState<T> previousState;

    public BaseController(BaseState<T> initState, T owner, Define.EAIState StatdID)
    {
       RegisterState(initState, owner, StatdID);

        currentState = initState;
        currentState.OnEnter();

    }

    public virtual void RegisterState(BaseState<T> state, T owner, Define.EAIState StateID)
    {
        state.Init(owner);
        registedState[StateID] = state;
    }

    public virtual void OnUpdate(float deltaTime)
    {
        currentState?.OnUpdate(deltaTime);  
    }
    

    public virtual void ChangeState(Define.EAIState newState)
    {
        if (currentState == registedState[newState]) return;

        currentState?.OnExit();

        previousState = currentState;

        currentState = registedState[newState];
        currentState.OnEnter(); 
    }

    public virtual void ChangeState(BaseState<T> dynamicState)
    {
        currentState?.OnExit();
        previousState = currentState;
        currentState = dynamicState;
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
