using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    private BattleCharacter _battleCharacter;
    private void Awake()
    {
        _battleCharacter = GetComponentInParent<BattleCharacter>();
    }

    public void OFF()
    {
        _battleCharacter.SetOff();
    }
}