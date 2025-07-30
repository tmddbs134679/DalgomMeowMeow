using System;
using UnityEngine;

public class CharacterAction : MonoBehaviour
{
    public event Action<Define.EAIState> OnAction;

    public void TryState(Define.EAIState state)
    {
        OnAction?.Invoke(state);
    }


    public void Idle() => OnAction?.Invoke(Define.EAIState.Idle);
    public void Cook() => OnAction?.Invoke(Define.EAIState.Cook);
    public void Play() => OnAction?.Invoke(Define.EAIState.Play);
    public void Rest() => OnAction?.Invoke(Define.EAIState.Rest);
    public void Deliver() => OnAction?.Invoke(Define.EAIState.Deliver);
    public void Collect() => OnAction?.Invoke(Define.EAIState.Collect);
    public void CabbageFarm() => OnAction?.Invoke(Define.EAIState.CabbageFarm);
    public void OnionFarm() => OnAction?.Invoke(Define.EAIState.OnionFarm);
    public void PotatoFarm() => OnAction?.Invoke(Define.EAIState.PotatoFarm);
    public void PumpkinFarm() => OnAction?.Invoke(Define.EAIState.PumpkinFarm);
    public void CarrotFarm() => OnAction?.Invoke(Define.EAIState.CarrotFarm);
    public void Build()   => OnAction?.Invoke(Define.EAIState.Build);
    public void Hello() => OnAction?.Invoke(Define.EAIState.Hello);
    public void Fishing() => OnAction?.Invoke(Define.EAIState.Fishing);
}
