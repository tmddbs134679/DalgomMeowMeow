using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class AIState : BaseState<AICharacter>
{
    protected Define.EAIState state;
    protected AICharacter character;
    public override void Init(AICharacter owner)
    {
        this.character = owner;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        character.CharacterData.CurrentState = state;
        character.CurrentState = character.CharacterData.CurrentState;
    }

    public Define.EAIState GetState() { return state; }
}
