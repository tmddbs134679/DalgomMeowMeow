using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAction : MonoBehaviour
{
    public event Action<Define.EAIState> OnAction;
    
    //public void Cook()    => OnAction?.Invoke(Define.EAIState.Cooking);
    //public void Play()    => OnAction?.Invoke(Define.EAIState.Playing);
    //public void Rest()    => OnAction?.Invoke(Define.EAIState.Resting);
    //public void Deliver() => OnAction?.Invoke(Define.EAIState.Delivery);
    //public void Collect() => OnAction?.Invoke(Define.EAIState.Collecting);
    //public void Farm()    => OnAction?.Invoke(Define.EAIState.Farming);
    //public void Build()   => OnAction?.Invoke(Define.EAIState.Building);
}
