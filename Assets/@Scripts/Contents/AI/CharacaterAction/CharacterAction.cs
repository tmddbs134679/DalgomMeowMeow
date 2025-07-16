using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAction : MonoBehaviour
{
    public event Action<Define.EAIState> OnAction;

    public void PreviousState(Define.EAIState state)
    {
        OnAction?.Invoke(state);
    }


    [ContextMenu("Idle")]
    public void Idle() => OnAction?.Invoke(Define.EAIState.Idle);
    [ContextMenu("Cook")]
    public void Cook() => OnAction?.Invoke(Define.EAIState.Cook);

    [ContextMenu("Play")]
    public void Play() => OnAction?.Invoke(Define.EAIState.Play);

    [ContextMenu("Rest")]
    public void Rest() => OnAction?.Invoke(Define.EAIState.Rest);

    [ContextMenu("Deliver")]
    public void Deliver() => OnAction?.Invoke(Define.EAIState.Deliver);

    [ContextMenu("Collect")]
    public void Collect() => OnAction?.Invoke(Define.EAIState.Collect);
    [ContextMenu("Farm")]
    public void Farm()    => OnAction?.Invoke(Define.EAIState.Farm);
    [ContextMenu("Build")]
    public void Build()   => OnAction?.Invoke(Define.EAIState.Build);
    [ContextMenu("Hello")]
    public void Hello() => OnAction?.Invoke(Define.EAIState.Hello);
    [ContextMenu("Fishing")]
    public void Fishing() => OnAction?.Invoke(Define.EAIState.Fishing);
}
