using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAction : MonoBehaviour
{
    public event Action<Define.EAIState> OnAction;
    [ContextMenu("Idle")]
    public void Idle() => OnAction?.Invoke(Define.EAIState.Idle);
    [ContextMenu("Cook")]
    public void Cook() => OnAction?.Invoke(Define.EAIState.Cooking);

    [ContextMenu("Play")]
    public void Play() => OnAction?.Invoke(Define.EAIState.Playing);

    [ContextMenu("Rest")]
    public void Rest() => OnAction?.Invoke(Define.EAIState.Resting);

    [ContextMenu("Deliver")]
    public void Deliver() => OnAction?.Invoke(Define.EAIState.Delivery);

    [ContextMenu("Collect")]
    public void Collect() => OnAction?.Invoke(Define.EAIState.Collecting);
    [ContextMenu("Farming")]
    public void Farm()    => OnAction?.Invoke(Define.EAIState.Farming);
    [ContextMenu("Building")]
    public void Build()   => OnAction?.Invoke(Define.EAIState.Building);
    [ContextMenu("Hello")]
    public void Hello() => OnAction?.Invoke(Define.EAIState.Hello);
}
